<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    CancelFromPaypal
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Anulowałeś operację płatności za pomocą serwisu PayPal.</h2>
<br />
<div>
<p>Jeśli nie anulowałeś płatności i wystąpił błąd kliknij <%: Html.ActionLink("tutaj", "PayError", "Basket", new { id = (int)ViewData["id"] }, null)%></li>, aby dowiedzieć się jak należy postępować w przypadku nieudanej płatności.</p>
</div>



<%if(ViewData["alert"] != ""){ %> <div style="color: Red"> <%: ViewData["alert"] %></div> <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
