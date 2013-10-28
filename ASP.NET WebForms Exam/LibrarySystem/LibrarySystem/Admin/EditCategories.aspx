<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditCategories.aspx.cs" Inherits="LibrarySystem.Admin.EditCategories" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="span12">
            <h1>Edit Categories</h1>
        </div>

        <div class="span12">
            <asp:GridView runat="server" 
                ID="GridViewCategories"
                class="gridview"
                ItemType="LibrarySystem.Models.Category"
                AllowPaging="true"
                DataKeyNames="Id"
                AllowSorting="true"
                AutoGenerateColumns="false"
                SelectMethod="GridViewCategories_GetData"
                DeleteMethod="GridViewCategories_DeleteItem"
                OnSelectedIndexChanged="GridViewCategories_SelectedIndexChanged"
                PageSize="5">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Category Name" SortExpression="Name" />
                    <asp:CommandField ShowDeleteButton="true" ShowSelectButton="true" SelectText="Edit"  ControlStyle-CssClass="link-button" HeaderText="Actions" />
                </Columns>
            </asp:GridView>
            <div class="create-link">
                <asp:LinkButton runat="server" id="MainContent_LinkButtonShowCreatePanel" OnClick="MainContent_LinkButtonShowCreatePanel_Click"
                     Text="Create New" class="link-button"></asp:LinkButton>
            </div>
            <div id="MainContent_PanelCreate" class="panel" runat="server" visible="false">
                <h2>Create New Category</h2>
                <label for="MainContent_TextBoxCategoryCreate">Category: <asp:TextBox runat="server" ID="MainContent_TextBoxCategoryCreate" placeholder="Enter category name..."></asp:TextBox></label>
                <asp:LinkButton runat="server" id="MainContent_LinkButtonCreate" class="link-button"
                    OnClick="MainContent_LinkButtonCreate_Click" Text="Crate"></asp:LinkButton>
                <asp:LinkButton runat="server" id="MainContent_LinkButtonCancelCreate" class="link-button"
                    OnClick="MainContent_LinkButtonCancelCreate_Click" Text="Cancel"></asp:LinkButton>
            </div>
            <div id="MainContent_PanelDelete" class="panel" runat="server" visible="false">
                <h2>Confirm Category Deletion?</h2>
                <asp:TextBox runat="server" ID="TextBoxDeleteItemid" Enabled="false" Visible="false"></asp:TextBox>
                <label for="TextBoxItemDelete">Category: <asp:TextBox runat="server" ID="TextBoxItemDelete"
                     Enabled="false"></asp:TextBox></label>
                <asp:LinkButton runat="server" id="MainContent_LinkButtonConfirmDelete" class="link-button"
                     OnClick="MainContent_LinkButtonConfirmDelete_Click" Text="Yes"></asp:LinkButton>
                <asp:LinkButton runat="server" id="MainContent_LinkButtonCancelDelete" class="link-button"
                    OnClick="MainContent_LinkButtonCancelDelete_Click" Text="No"></asp:LinkButton>
            </div>
            <div id="MainContent_PanelEdit" class="panel" runat="server" visible="false">
	
                <h2>Edit Category</h2>
                <asp:TextBox runat="server" ID="TextBoxCategoryEditId" Visible="false" Enabled="false" ></asp:TextBox>
                <label for="TextBoxCategoryEdit">Category: <asp:TextBox runat="server" 
                    ID="TextBoxCategoryEdit"></asp:TextBox></label>
                <asp:LinkButton runat="server" id="MainContent_LinkButtonEditSave"  OnClick="MainContent_LinkButtonEditSave_Click"
                    class="link-button" Text="Save"></asp:LinkButton>
                <asp:LinkButton runat="server" id="MainContent_LinkButtonCancelEdit" OnClick="MainContent_LinkButtonCancelEdit_Click"
                    class="link-button" text="Cancel"></asp:LinkButton>
            
            </div>
        </div>
    </div>
    <div class="back-link">
        <a href="/">Back to books</a>
    </div>
</asp:Content>
