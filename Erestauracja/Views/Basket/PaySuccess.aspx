<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    PaySuccess
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Płatność zakończona powodzeniem</h2>
<br />
<div>Aktualne zamówienia oraz ich status możesz zobaczyć w zakładce "Aktualne zamówienia".</div>
<% if ((int)ViewData["id"] != null) %>
<% { %>
    <div>Numer zrealizowanego zamówienia: <%: ViewData["id"].ToString() %></div>
<% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
