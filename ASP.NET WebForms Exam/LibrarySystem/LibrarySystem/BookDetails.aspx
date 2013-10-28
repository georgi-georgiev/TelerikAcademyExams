<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookDetails.aspx.cs" Inherits="LibrarySystem.BookDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Book Details</h1>
    <asp:Repeater runat="server" ID="RepeaterBook" ItemType="LibrarySystem.Models.Book">
        <ItemTemplate>
            <p class="book-title"><i><%#: Item.Title %></i></p>
            <p class="book-author"><i><%#: Item.Author %></i></p>
            <p class="book-isbn"><i><%#: Item.ISBN %></i></p>
            <p class="book-isbn"><i><a href="<%#: Item.Website %>"><%#: Item.Website %></a></i></p>
            <div class="row-fluid"><div class="span12 book-description"><%#: Item.Content %></div></div>
            <div class="back-link"><a href="/">Back to books</a></div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
