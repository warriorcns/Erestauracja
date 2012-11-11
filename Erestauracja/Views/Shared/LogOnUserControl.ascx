<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated)
    {
%>
<ul class="topka">
    <a class="ikonka" href="/Home">
    </a>
    

    <div id="label" style="z-index:-1;"> Witaj <strong> <%: Page.User.Identity.Name %></strong>!</div>

    <div id="label" style="z-index:-1;"><%: Html.ActionLink("Wyloguj", "LogOff", "Account") %></div>

</ul>
<%
    }
    else
    {
%>
    <ul class="topka">
    <a class="ikonka" href="/Home">
    </a>
    <div id="label" style="z-index:-1;"><%: Html.ActionLink("Zaloguj", "LogOn", "Account") %></div>

</ul>
        
<%
    }
%>
