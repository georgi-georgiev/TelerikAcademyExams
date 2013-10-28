<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="LibrarySystem.Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="span12">
            <h1>Search Results for Query <i>"<asp:Literal runat="server" ID="QueryName" Mode="Encode"></asp:Literal>"</i>:</h1>
        </div>
        
        <div class="span12 search-results">
            <ul>
            <asp:Repeater ID="RepeaterSearchResults" runat="server" ItemType="LibrarySystem.Models.Book">
                <ItemTemplate>
                    <li><a href="#"><%#: Item.Title %> by <%#: Item.Author %></a> (Category: <%#: Item.Category.Name %>)</li>
                </ItemTemplate>
            </asp:Repeater>
            </ul>
        </div>
    </div>
    <div class="back-link">
        <a href="/">Back to books</a>
    </div>
</asp:Content>
