using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibrarySystem
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var categories = context.Categories.ToList();
            this.ListViewCategory.DataSource = categories;
            this.ListViewCategory.DataBind();
        }

        protected void SearchByBookTitleOrAuthor_Command(object sender, CommandEventArgs e)
        {
            string query = this.TextBoxBookTitleOrAuthor.Text;
            Response.Redirect("Search?name=" + query);
        }
    }
}