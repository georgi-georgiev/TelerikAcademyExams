using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using TicketSystem.Web.Models;
using TicketSystem.Models;
using System.Net;

namespace TicketSystem.Web.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminCategoriesController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ReadCategories([DataSourceRequest] DataSourceRequest request)
        {
            var categories = this.Data.Categories.All().Select(x => new CategoryViewModel
            {
                Id = x.Id,
                Name = x.Name
            });
            return Json(categories.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCategory([DataSourceRequest] DataSourceRequest request, CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                var selectedCategory = this.Data.Categories.All().FirstOrDefault(c => c.Id == category.Id);

                selectedCategory.Name = category.Name;

                this.Data.SaveChanges();

            }
            return Json(new[] { category }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);

        }

        public JsonResult DestroyCategory([DataSourceRequest] DataSourceRequest request, CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                var selectedCategory = this.Data.Categories.GetById(category.Id);

                for (int i = 0; i < selectedCategory.Tickets.Count(); i++)
                {
                    for (int j = 0; j < selectedCategory.Tickets.ElementAt(i).Comments.Count(); j++)
                    {
                        this.Data.Comments.Delete(selectedCategory.Tickets.ElementAt(i).Comments.ElementAt(j).Id);
                    }
                    this.Data.Tickets.Delete(selectedCategory.Tickets.ElementAt(i).Id);
                }

                this.Data.Categories.Delete(category.Id);

                this.Data.SaveChanges();
            }
            
            return Json(new[] { category }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateCategory([DataSourceRequest] DataSourceRequest request, CategoryViewModel category)
        {
            if(ModelState.IsValid)
            {
                this.Data.Categories.Add(new Category()
                    {
                        Name = category.Name
                    });
                this.Data.SaveChanges();
            }
            return Json(new[] { category }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }
    }
}