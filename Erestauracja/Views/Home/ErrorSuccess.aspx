<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<p>
        Dziękujemy za wysłanie zgłoszenia.
        <p>
            <%: Html.ActionLink("Powrót do strony głównej.", "Index", "Home")%>
        </p>
    </p>

</asp:Content>
