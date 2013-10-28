using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibrarySystem
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            string query = Request.Params["name"];
            if (query.Length <= 50)
            {
                if (String.IsNullOrEmpty(query))
                {
                    var books = context.Books.ToList();
                    this.RepeaterSearchResults.DataSource = books.OrderBy(b => b.Title);
                    this.RepeaterSearchResults.DataBind();
                    this.QueryName.Text = "all";
                }
                else
                {
                    var books =
                        (from book in context.Books
                         where (book.Author.Contains(query)) || (book.Title.Contains(query))
                         select book).ToList();

                    this.RepeaterSearchResults.DataSource = books.OrderBy(b => b.Title);
                    this.RepeaterSearchResults.DataBind();
                    this.QueryName.Text = query;
                }
            }
            else
            {
                Response.Redirect("SearchError");
            }
        }
    }
}