using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketSystem.Models;
using TicketSystem.Web.Models;
using Microsoft.AspNet.Identity;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Net;

namespace TicketSystem.Web.Controllers
{
    public class TicketsController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public JsonResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var tickets = this.Data.Tickets.All().Select(ticket => new ListTicketsViewModel()
                    {
                        Id = ticket.Id,
                        Title = ticket.Title,
                        Category = ticket.Category.Name,
                        Author = ticket.Author.UserName,
                        Priority = ticket.Priority
                    });
            return Json(tickets.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id)
        {
            var selectedTicket = this.Data.Tickets.All().Where(t => t.Id == id);
            var ticket = selectedTicket
                .Select(t => new TicketDetailsViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Category = t.Category.Name,
                    Author = t.Author.UserName,
                    Comments = t.Comments.Select(c => new CommentViewModel() { Content = c.Content, User = c.User.UserName }),
                    Description = t.Description,
                    Priority = t.Priority,
                    ScreenshotURL = t.ScreenshotURL
                }).FirstOrDefault();
            return View(ticket);
        }

        [Authorize]
        
        public ActionResult Add()
        {
            ViewBag.Categories = this.Data.Categories.All().ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            //ViewBag.Priorities = new List<SelectListItem>()
            //    {
            //        new SelectListItem { Text="Low", Value="Low" },
            //        new SelectListItem { Text="Medium", Value="Medium" },
            //        new SelectListItem { Text="High", Value="High" }
            //    };

            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Add(CreateTicketViewModel ticket)
        {
            ViewBag.Categories = this.Data.Categories.All().ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            //ViewBag.Priorities = new List<SelectListItem>()
            //    {
            //        new SelectListItem { Text="High", Value="High" },
            //        new SelectListItem { Text="Low", Value="Low" },
            //        new SelectListItem { Text="Medium", Value="Medium" },
                    
            //    };
            if (ticket.Priority != "Low" && ticket.Priority != "Medium" && ticket.Priority != "High")
            {
                ModelState.AddModelError("CustomErrorPriority", "Priority is invalid");
            }

            var selectedCategory = this.Data.Categories.All().Where(c => c.Id == ticket.CategoryId).FirstOrDefault();
            if(selectedCategory == null)
            {
                ModelState.AddModelError("CustomErrorCategory", "Invalid category");
            }

            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var selectedUser = this.Data.Users.All().Where(u => u.Id == userId).FirstOrDefault();
                var newTicket = new Ticket()
                {
                    Title = ticket.Title,
                    Description = ticket.Description,
                    ScreenshotURL = ticket.ScreenshotURL,
                    Category = selectedCategory,
                    Priority = ticket.Priority,
                    Author = selectedUser
                };

                selectedUser.Points++;

                this.Data.Tickets.Add(newTicket);
                this.Data.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(ticket);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment(CreateCommentViewModel model)
        {
            if(ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var selectedUser = this.Data.Users.All().Where(u => u.Id == userId).FirstOrDefault();

                var selectedTicket = this.Data.Tickets.All().Where(t => t.Id == model.TicketId).FirstOrDefault();

                this.Data.Comments.Add(new Comment()
                    {
                        Content = model.Content,
                        User = selectedUser,
                        Ticket = selectedTicket
                    });

                this.Data.SaveChanges();

                var commentViewModel = new CommentViewModel { User = selectedUser.UserName, Content = model.Content };
                return PartialView("_CommentPartial", commentViewModel);
            }

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, ModelState.Values.First().ToString());
        }
        [Authorize]
        public JsonResult ReadCategories()
        {
            var categories = this.Data.Categories.All().Select(x =>
                new {
                    CategoryName = x.Name,
                    CategoryId = x.Id
                });

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Search(int? Category)
        {
            if (Category == null)
            {
                var tickets = this.Data.Tickets.All().Select(ticket => new ListTicketsViewModel()
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Category = ticket.Category.Name,
                    Author = ticket.Author.UserName,
                    Priority = ticket.Priority
                }).ToList();

                return View(tickets);
            }
            else
            {
                var tickets = this.Data.Tickets.All().Where(t => t.Category.Id == Category).Select(ticket => new ListTicketsViewModel()
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Category = ticket.Category.Name,
                    Author = ticket.Author.UserName,
                    Priority = ticket.Priority
                }).ToList();

                return View(tickets);
            }
        }
    }
}