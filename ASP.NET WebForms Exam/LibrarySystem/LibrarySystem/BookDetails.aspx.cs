using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibrarySystem
{
    public partial class BookDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request.Params["id"];
            if (!String.IsNullOrEmpty(id))
            {
                int bookId = int.Parse(id);
                ApplicationDbContext context = new ApplicationDbContext();
                var selectedBook = context.Books.FirstOrDefault(b => b.Id == bookId);
                this.RepeaterBook.DataSource = new List<Book> { selectedBook };
                this.RepeaterBook.DataBind();
            }
            else
            {
                Response.Redirect("NotingFound");
            }
        }
    }
}