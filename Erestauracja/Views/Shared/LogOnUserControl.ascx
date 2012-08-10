<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Erestauracja.Models" %>



<link href="../../Content/Site.css" rel="stylesheet" type="text/css" />

<%
    if (Request.IsAuthenticated) {
%>
       Welcome <strong><%: Page.User.Identity.Name %></strong>!
         
        [ <%: Html.ActionLink("Log Off", "LogOff", "Account") %> ]

<%
    }
    else {
%>      <ul class="topka">
            <div class="ikonka">
            </div>
            <% using (Html.BeginForm())
               { %>
            <ul class="logowanie">
                <%--Tworzenie obiektu wewnatrz widoku, masakra.. nie dziala importowanie widoku -.-'--%>
                <%--<% Erestauracja.Models.LogOnModel m = new LogOnModel();%>--%>
                <li id="label">
                    <%: Html.Label("Login")%></li>
                <li>
                    <%: Html.TextBoxFor(m => m.UserName, new { @style = "width: 130px;" }) %>
                    <%: Html.ValidationMessageFor(m => m.UserName) %>
                </li>
                <ul class="logowanierememberme">
                    <li id="rememberme">
                        <%: Html.CheckBoxFor(m => m.RememberMe)%></li>
                    <li id="remembermelabel">
                        <%: Html.Label("Zapamiętaj mnie")%></li>
                </ul>
            </ul>
            <ul class="logowaniepassword">
                <li id="label">
                    <%: Html.Label("Haslo") %></li>
                <li>
                    <%: Html.PasswordFor(m => m.Password, new { @style = "width: 130px;" })%>
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </li>
            </ul>
            <ul class="zaloguj">
                <%--Przekierowuje na gotowa metode logowania.--%>
                <li>
                   <%--<%: Html.ActionLink("Zaloguj", "LogOn", "Account")%>--%>
                   <p><input type="submit" value="Log On" /></p>
                   </li>
            </ul>
            <% } %>

           
        </ul>
       <%-- [ <div class="zaloguj"> <%: Html.ActionLink("Log On", "LogOn", "Account") %></div> ]--%>
<%
    }
%>
