using Error_Handler_Control;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibrarySystem.Admin
{
    public partial class EditBooks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<LibrarySystem.Models.Book> GridViewBooks_GetData()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            return context.Books.Include("Category").OrderBy(c => c.Id);
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void GridViewBooks_DeleteItem(int id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var selectedBook = context.Books.Find(id);
            this.MainContent_PanelDelete.Visible = true;
            this.TextBoxDeleteBookTitle.Text = selectedBook.Title;
            this.TextBoxDeleteBookId.Text = selectedBook.Id.ToString();
        }

        protected void GridViewBooks_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var item = context.Books.Find(this.GridViewBooks.SelectedValue);
            this.TextBoxBookEditId.Text = item.Id.ToString();
            this.TextBoxBookEditTitle.Text = item.Title;
            this.TextBoxBookEditContent.Text = item.Content;
            this.TextBoxBookEditISBN.Text = item.ISBN;
            this.TextBoxBookEditAuthor.Text = item.Author;
            this.TextBoxBookEditWebsite.Text = item.Website;

            var categories =
                from category in context.Categories
                select category;
            List<ListItem> categoriesList = new List<ListItem>();
            foreach (Category category in categories)
            {
                categoriesList.Add(new ListItem(category.Name, category.Id.ToString()));
            }
            this.DropDownListBooksEdit.DataTextField = "Text";
            this.DropDownListBooksEdit.DataValueField = "Value";
            this.DropDownListBooksEdit.SelectedValue = item.Category.Id.ToString();
            this.DropDownListBooksEdit.DataSource = categoriesList;
            this.DropDownListBooksEdit.DataBind();

            this.GridViewBooks.DataBind();

            this.MainContent_PanelEdit.Visible = true;
        }
        protected void MainContent_LinkButtonShowCreatePanel_Click(object sender, EventArgs e)
        {
            this.MainContent_PanelCreate.Visible = true;

            ApplicationDbContext context = new ApplicationDbContext();
            var categories =
                from category in context.Categories
                select category;
            List<ListItem> categoriesList = new List<ListItem>();
            foreach (Category category in categories)
            {
                categoriesList.Add(new ListItem(category.Name, category.Id.ToString()));
            }
            this.DropDownListCategories.DataTextField = "Text";
            this.DropDownListCategories.DataValueField = "Value";
            this.DropDownListCategories.DataSource = categoriesList;
            this.DropDownListCategories.DataBind();
        }

        protected void MainContent_LinkButtonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var selectedCategroyId = int.Parse(this.DropDownListCategories.SelectedValue);
                var selectedCategory = context.Categories.Find(selectedCategroyId);

                var newBook = new Book()
                {
                    Title = this.MainContent_TextBoxBookTitleCreate.Text,
                    Author = this.MainContent_TextBoxBookAuthor.Text,
                    Content = this.MainContent_TextBoxBookContent.Text,
                    Website = this.MainContent_TextBoxBookWebsite.Text,
                    ISBN = this.MainContent_TextBoxBookISBN.Text,
                    Category = selectedCategory
                };

                context.Books.Add(newBook);
                context.SaveChanges();

                this.GridViewBooks.DataBind();

                this.MainContent_PanelCreate.Visible = false;

                ErrorSuccessNotifier.AddInfoMessage("You have successfully created book with title: " + newBook.Title);
            }
            catch(Exception ex)
            { 
                ErrorSuccessNotifier.AddErrorMessage(ex);
            }
        }

        protected void MainContent_LinkButtonCancelCreate_Click(object sender, EventArgs e)
        {
            this.MainContent_PanelCreate.Visible = false;
            this.MainContent_TextBoxBookTitleCreate.Text = "";
            this.MainContent_TextBoxBookAuthor.Text = "";
            this.MainContent_TextBoxBookContent.Text = "";
            this.MainContent_TextBoxBookWebsite.Text = "";
            this.MainContent_TextBoxBookISBN.Text = "";
        }

        protected void MainContent_LinkButtonConfirmDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedBookId = int.Parse(this.TextBoxDeleteBookId.Text);
                ApplicationDbContext context = new ApplicationDbContext();
                var selectedBook = context.Books.Find(selectedBookId);
                context.Books.Remove(selectedBook);
                context.SaveChanges();
                this.GridViewBooks.DataBind();

                this.MainContent_PanelDelete.Visible = false;

                ErrorSuccessNotifier.AddInfoMessage("You have successfully deleted book with title: " + selectedBook.Title);
            }
            catch(Exception ex)
            { 
                ErrorSuccessNotifier.AddErrorMessage(ex);
            }
        }

        protected void MainContent_LinkButtonCancelDelete_Click(object sender, EventArgs e)
        {
            this.MainContent_PanelDelete.Visible = false;
        }

        protected void MainContent_LinkButtonEditSave_Click(object sender, EventArgs e)
        {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                int id = int.Parse(this.TextBoxBookEditId.Text);
                var selectedItem = context.Books.Find(id);
                selectedItem.Title = this.TextBoxBookEditTitle.Text;
                selectedItem.Content = this.TextBoxBookEditContent.Text;
                selectedItem.ISBN = this.TextBoxBookEditISBN.Text;
                selectedItem.Author = this.TextBoxBookEditAuthor.Text;
                selectedItem.Website = this.TextBoxBookEditWebsite.Text;
                int selectedCategoryId = int.Parse(this.DropDownListBooksEdit.SelectedValue);
                var selectedCategory = context.Categories.Find(selectedCategoryId);
                selectedItem.Category = selectedCategory;
                context.SaveChanges();

                this.GridViewBooks.DataBind();

                ErrorSuccessNotifier.AddInfoMessage("You have successfully edited book to " + TextBoxBookEditTitle.Text);
            
                this.MainContent_PanelEdit.Visible = false;

            }
            catch(Exception ex)
            { 
                ErrorSuccessNotifier.AddErrorMessage(ex);
            }
        }

        protected void MainContent_LinkButtonCancelEdit_Click(object sender, EventArgs e)
        {
            this.MainContent_PanelEdit.Visible = false;
        }
    }
}