<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Account
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% //to raczej nie bedzie potrzebne chyba że da sie wejsc tu bezpośrednio
    if (Request.IsAuthenticated) {
%>
<h2>dane konta</h2>
<%
    }
    else {
        %>
        błąd cofnij if zaloguj sie 
        <%
    }
%>
</asp:Content>
