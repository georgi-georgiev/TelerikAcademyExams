<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LibrarySystem._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="span1">
            <h1>Books</h1>
        </div>
        <div class="search-button">
            <div class="form-search">
                <div class="input-append">
                    <asp:TextBox runat="server" ID="TextBoxBookTitleOrAuthor" class="span3 search-query"
                         placeholder="Search by book title / author...">
                    </asp:TextBox>
                    <asp:Button runat="server" CssClass="btn" ID="SearchByBookTitleOrAuthor" CommandName="Search"
                         OnCommand="SearchByBookTitleOrAuthor_Command" Text="Search">
                    </asp:Button>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
            <asp:ListView runat="server" ID="ListViewCategory" ItemType="LibrarySystem.Models.Category">
                <ItemTemplate>
                    <div class="span4">
                    <h2><%#: Item.Name %></h2>
                        <ul>
                            <asp:Repeater ID="RepeaterBooks" DataSource="<%# Item.Books %>" ItemType="LibrarySystem.Models.Book" runat="server">
                                <ItemTemplate>
                                    <li><a href='BookDetails?id=<%# Item.Id %>'><%#: Item.Title %> by <%#: Item.Author %></a></li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </ItemTemplate>
            </asp:ListView>
    </div>
</asp:Content>
