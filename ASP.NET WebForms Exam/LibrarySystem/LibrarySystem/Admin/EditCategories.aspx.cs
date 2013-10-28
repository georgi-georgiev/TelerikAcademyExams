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
    public partial class EditCategories : System.Web.UI.Page
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
        public IQueryable<LibrarySystem.Models.Category> GridViewCategories_GetData()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            return context.Categories.OrderBy(c => c.Id);
        }

        

        // The id parameter name should match the DataKeyNames value set on the control
        public void GridViewCategories_DeleteItem(int id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var selectedCaregory = context.Categories.Find(id);
            this.MainContent_PanelDelete.Visible = true;
            this.TextBoxItemDelete.Text = selectedCaregory.Name;
            this.TextBoxDeleteItemid.Text = selectedCaregory.Id.ToString();
        }

        protected void MainContent_LinkButtonShowCreatePanel_Click(object sender, EventArgs e)
        {
            this.MainContent_LinkButtonShowCreatePanel.Visible = false;
            this.MainContent_PanelCreate.Visible = true;
        }

        protected void MainContent_LinkButtonCancelCreate_Click(object sender, EventArgs e)
        {
            this.MainContent_LinkButtonShowCreatePanel.Visible = true;
            this.MainContent_PanelCreate.Visible = false;
            this.MainContent_TextBoxCategoryCreate.Text = "";
        }

        protected void MainContent_LinkButtonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                Category newCategory = new Category()
                {
                    Name = this.MainContent_TextBoxCategoryCreate.Text
                };
                context.Categories.Add(newCategory);
                context.SaveChanges();
                this.DataBind();

                MainContent_TextBoxCategoryCreate.Text = "";
                this.MainContent_LinkButtonShowCreatePanel.Visible = true;
                this.MainContent_PanelCreate.Visible = false;
                ErrorSuccessNotifier.AddInfoMessage("You have successfully create category with name: " + newCategory.Name);
            }
            catch(Exception ex)
            { 
                ErrorSuccessNotifier.AddErrorMessage(ex);
            }
        }
        protected void MainContent_LinkButtonConfirmDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedCategoryId = int.Parse(this.TextBoxDeleteItemid.Text);
                ApplicationDbContext context = new ApplicationDbContext();
                var selectedCaregory = context.Categories.Find(selectedCategoryId);
                context.Books.RemoveRange(selectedCaregory.Books);
                context.Categories.Remove(selectedCaregory);
                context.SaveChanges();
                this.GridViewCategories.DataBind();

                this.MainContent_PanelDelete.Visible = false;
                this.TextBoxItemDelete.Text = "";
                this.TextBoxDeleteItemid.Text = "";

                ErrorSuccessNotifier.AddInfoMessage("You have successfully deleted category with name: " + selectedCaregory.Name);
            }
            catch(Exception ex)
            { 
                ErrorSuccessNotifier.AddErrorMessage(ex);
            }
        }

        protected void MainContent_LinkButtonCancelDelete_Click(object sender, EventArgs e)
        {
            this.MainContent_PanelDelete.Visible = false;
            this.TextBoxItemDelete.Text = "";
            this.TextBoxDeleteItemid.Text = "";
        }

        protected void MainContent_LinkButtonEditSave_Click(object sender, EventArgs e)
        {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                int id = int.Parse(this.TextBoxCategoryEditId.Text);
                var selectedItem = context.Categories.Find(id);
                selectedItem.Name = this.TextBoxCategoryEdit.Text;
                context.SaveChanges();
                this.GridViewCategories.DataBind();

                ErrorSuccessNotifier.AddInfoMessage("You have successfully edited category to: " + TextBoxCategoryEdit.Text);

                this.TextBoxCategoryEditId.Text = "";
                this.TextBoxCategoryEdit.Text = "";
                this.MainContent_PanelEdit.Visible = false;
                
            }
            catch(Exception ex)
            { 
                ErrorSuccessNotifier.AddErrorMessage(ex);
            }
        }

        protected void MainContent_LinkButtonCancelEdit_Click(object sender, EventArgs e)
        {
            this.TextBoxCategoryEditId.Text = "";
            this.TextBoxCategoryEdit.Text = "";
            this.MainContent_PanelEdit.Visible = false;
        }

        protected void GridViewCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var item = context.Categories.Find(this.GridViewCategories.SelectedValue);
            this.TextBoxCategoryEditId.Text = item.Id.ToString();
            this.TextBoxCategoryEdit.Text = item.Name;
            this.MainContent_PanelEdit.Visible = true;

            this.GridViewCategories.DataBind();
        }
    }
}