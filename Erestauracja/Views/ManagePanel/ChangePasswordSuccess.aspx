<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    ChangePasswordSuccess
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Zmiana hasła</h2>
    <p>
        Zmiana hasła została przeprowadzona pomyślnie.
        <p>
            <%: Html.ActionLink("Powrót do strony głównej.", "Restaurant", "ManagePanel")%>
        </p>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
