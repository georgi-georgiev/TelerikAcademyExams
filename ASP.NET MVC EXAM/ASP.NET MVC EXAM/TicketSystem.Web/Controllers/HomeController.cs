using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketSystem.Web.Models;

namespace TicketSystem.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (this.HttpContext.Cache["HomePageTickets"] == null)
            {
                var tickets = this.Data.Tickets.All().Select(ticket => new TicketViewModel()
                    {
                        Id = ticket.Id,
                        Title = ticket.Title,
                        Category = ticket.Category.Name,
                        Author = ticket.Author.UserName,
                        Comments = ticket.Comments.Count()
                    }).OrderByDescending(t => t.Comments).Take(6);

                this.HttpContext.Cache.Add("HomePageTickets", tickets.ToList(), null, DateTime.Now.AddHours(1), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Default, null);
            
            }
            return View(this.HttpContext.Cache["HomePageTickets"]);
        }
    }
}