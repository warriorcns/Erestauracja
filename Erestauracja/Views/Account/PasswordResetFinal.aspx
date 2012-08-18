<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Odzyskiwanie hasła.
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>
Hasło zostało wysłane na adres email podany przy rejestracji.
Jeżeli nie dostałeś hasła w przeciągu 12 godzin, ponów próbę lub skontaktuj się z administratorem.

<p>
    <%: Html.ActionLink("Powrót do strony logowania.", "LogOn", "Account")%>
</p>
</h2>

</asp:Content>
