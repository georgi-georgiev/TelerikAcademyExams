<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditBooks.aspx.cs" Inherits="LibrarySystem.Admin.EditBooks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="span12">
            <h1>Edit Books</h1>
        </div>

        <div class="span12">
            <asp:GridView runat="server" ID="GridViewBooks"
                class="gridview ellipsis"
                ItemType="LibrarySystem.Models.Book"
                AllowPaging="true"
                DataKeyNames="Id"
                AllowSorting="true"
                AutoGenerateColumns="false"
                SelectMethod="GridViewBooks_GetData"
                DeleteMethod="GridViewBooks_DeleteItem"
                OnSelectedIndexChanged="GridViewBooks_SelectedIndexChanged"
                PageSize="5">
                <Columns>
                    <asp:BoundField DataField="Title" ControlStyle-CssClass="ellipsis" HeaderText="Title" SortExpression="Title" />
                    <asp:BoundField DataField="Author" HeaderText="Author" SortExpression="Author" />
                    <asp:BoundField DataField="ISBN" HeaderText="ISBN" SortExpression="ISBN" />
                    <asp:HyperLinkField DataTextField="Website" DataNavigateUrlFields="Website" HeaderText="Web Site" SortExpression="Website" />
                    <asp:BoundField DataField="Category.Name" HeaderText="Category" />
                    <asp:CommandField ShowDeleteButton="true" ShowSelectButton="true" SelectText="Edit"
                          ControlStyle-CssClass="link-button" HeaderText="Actions" />
                </Columns>
            </asp:GridView>
            <div class="create-link">
                <asp:LinkButton runat="server" id="MainContent_LinkButtonShowCreatePanel" OnClick="MainContent_LinkButtonShowCreatePanel_Click" class="link-button" Text="Create New"></asp:LinkButton>
            </div>
            <div id="MainContent_PanelCreate" class="panel" runat="server" visible="false">
                <h2>Create New Book</h2>
                <label><span>Title:</span><asp:TextBox runat="server" id="MainContent_TextBoxBookTitleCreate"
                     placeholder="Enter book title ..."></asp:TextBox></label>
                <label><span>Author(s):</span><asp:TextBox runat="server" ID="MainContent_TextBoxBookAuthor" placeholder="Enter book author / authors ..."></asp:TextBox></label>
                <label><span>ISBN:</span><asp:TextBox runat="server" ID="MainContent_TextBoxBookISBN" placeholder="Enter book ISBN ..."></asp:TextBox></label>
                <asp:RangeValidator runat="server" Type="String" MaximumValue="10" MinimumValue="10" ErrorMessage="Must be 10 symbols"
                    EnableClientScript="false" ControlToValidate="MainContent_TextBoxBookISBN" Display="Dynamic" ID="RangeValidatorISBN">
                </asp:RangeValidator>
                <label><span>Web site:</span><asp:TextBox runat="server" ID="MainContent_TextBoxBookWebsite" placeholder="Enter book web site ..."></asp:TextBox></label>
                <asp:RegularExpressionValidator runat="server" ControlToValidate="MainContent_TextBoxBookWebsite"
                    ForeColor="Red" Display="Dynamic" EnableClientScript="false"
                    ErrorMessage="Incorrect url" ID="ReguralExpresssionUrl"
                     ValidationExpression="/((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/">
                </asp:RegularExpressionValidator>

                <label><span>Description:</span><asp:TextBox runat="server" ID="MainContent_TextBoxBookContent" TextMode="MultiLine" Rows="2" Columns="20"
                     placeholder="Enter book description ..." style="height:160px;"></asp:TextBox></label>
                <asp:DropDownList runat="server" ID="DropDownListCategories">
                </asp:DropDownList>
                
                <br />
                <asp:LinkButton runat="server" id="MainContent_LinkButtonCreate" class="link-button" OnClick="MainContent_LinkButtonCreate_Click" Text="Create"></asp:LinkButton>
                <asp:LinkButton runat="server" id="MainContent_LinkButtonCancelCreate" class="link-button" OnClick="MainContent_LinkButtonCancelCreate_Click" Text="Cancel"></asp:LinkButton>
                
            </div>
            <div id="MainContent_PanelDelete" class="panel" runat="server" visible="false">
	
                <h2>Confirm Book Deletion?</h2>
                <asp:TextBox runat="server" ID="TextBoxDeleteBookId" Enabled="false" Visible="false"></asp:TextBox>
                <label>Title: <asp:TextBox runat="server" ID="TextBoxDeleteBookTitle" Enabled="false" TextMode="MultiLine" Rows="2" Columns="20" >
                              </asp:TextBox></label>
                <asp:LinkButton runat="server" id="MainContent_LinkButtonConfirmDelete" class="link-button" OnClick="MainContent_LinkButtonConfirmDelete_Click" Text="Yes"></asp:LinkButton>
                <asp:LinkButton runat="server" id="MainContent_LinkButtonCancelDelete" class="link-button" OnClick="MainContent_LinkButtonCancelDelete_Click" Text="No"></asp:LinkButton>
            
            </div>
            <div id="MainContent_PanelEdit" class="panel" runat="server" visible="false">
                <h2>Edit Book</h2>
                <asp:TextBox runat="server" ID="TextBoxBookEditId" Visible="false" Enabled="false"></asp:TextBox>
                <label><span>Title:</span><asp:TextBox runat="server" id="TextBoxBookEditTitle"
                     placeholder="Enter book title ..."></asp:TextBox></label>
                <label><span>Author(s):</span><asp:TextBox runat="server" ID="TextBoxBookEditAuthor" 
                    placeholder="Enter book author / authors ..."></asp:TextBox></label>
                <label><span>ISBN:</span><asp:TextBox runat="server" ID="TextBoxBookEditISBN" 
                    placeholder="Enter book ISBN ..."></asp:TextBox></label>
                <label><span>Web site:</span><asp:TextBox runat="server" ID="TextBoxBookEditWebsite" 
                    placeholder="Enter book web site ..."></asp:TextBox></label>
                <asp:RegularExpressionValidator runat="server" ControlToValidate="TextBoxBookEditWebsite"
                    ForeColor="Red" Display="Dynamic" EnableClientScript="false"
                    ErrorMessage="Incorrect url" ID="RegularExpressionValidatorEdit"
                     ValidationExpression="/((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/">

                </asp:RegularExpressionValidator>
                <label><span>Description:</span><asp:TextBox runat="server" ID="TextBoxBookEditContent" 
                    TextMode="MultiLine" Rows="2" Columns="20"
                     placeholder="Enter book description ..." style="height:160px;"></asp:TextBox></label>
                <asp:DropDownList runat="server" ID="DropDownListBooksEdit">
                </asp:DropDownList>
                <br />
                <asp:LinkButton runat="server" id="MainContent_LinkButtonEditSave" 
                    class="link-button" OnClick="MainContent_LinkButtonEditSave_Click" Text="Save"></asp:LinkButton>
                <asp:LinkButton runat="server" id="MainContent_LinkButtonCancelEdit" 
                    class="link-button" OnClick="MainContent_LinkButtonCancelEdit_Click" Text="Cancel"></asp:LinkButton>
            </div>
        </div>
    </div>
    <div class="back-link">
        <a href="/">Back to books</a>
    </div>
</asp:Content>
