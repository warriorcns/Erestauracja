<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated)
    {
%>
<ul class="topka">
    <div class="ikonka">
    </div>
    

    <div id="label"> Witaj <strong> <%: Page.User.Identity.Name %></strong>!</div>

    <div id="label"><%: Html.ActionLink("Wyloguj", "LogOff", "Account") %></div>

</ul>
<%
    }
    else
    {
%>
        <ul class="topka">
    <div class="ikonka">
    </div>
    <div id="label"><%: Html.ActionLink("Zaloguj", "LogOn", "Account") %></div>

</ul>
        
<%
    }
%>
