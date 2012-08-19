<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Change Password
</asp:Content>

<asp:Content ID="changePasswordSuccessContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Zmiana hasła</h2>
    <p>
        Zmiana hasła została przeprowadzona pomyślnie.
        <p>
            <%: Html.ActionLink("Powrót do strony głównej.", "Index", "Home")%>
        </p>
    </p>
</asp:Content>
