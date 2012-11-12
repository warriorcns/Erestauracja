<%@ Page Title="" Language="C#" MasterPageFile="~/Views/POS/POS.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.LogOnModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm())
   { %>
    <div class="locked-login">
        <%--<%: Html.DropDownListFor(m=>m.Login, ViewData["logins"] as /IEnum /erable<>) %>--%>
        <%:Html.PasswordFor(m => m.Password)%>
    
        <input type="submit" value="Zaloguj"/>
    </div>
<% } %>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
