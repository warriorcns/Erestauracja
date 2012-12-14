<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated)
    {
%>
<div class="topka">
    <a class="ikonka" href="/POS"></a>
    <div><%: Html.ActionLink("Wyloguj", "LogOff", "Account", null, new { @class = "loglink" })%></div>

</div>
<%
    }
    else
    {
%>
<div class="topka">
    <a class="ikonka" href="/POS"></a>
    <div><%: Html.ActionLink("Zaloguj", "LogOn", "Account", null, new { @class = "loglink" })%></div>

</div>
        
<%
    }
%>
