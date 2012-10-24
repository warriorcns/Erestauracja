<%@ Page Title="" Language="C#" MasterPageFile="~/Views/POS/POS.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="buttons-container">
        <div class="button wood">
            <div>
                <%: Html.ActionLink("Aktywne zamówienia", "ActiveOrders", "POS")%></div>
        </div>
        <div class="button wood">
            <div>
                <%: Html.ActionLink("Wszystkie zamówienia", "AllOrders", "POS")%></div>
        </div>
        <div class="button wood">
            <div>
                <%: Html.ActionLink("Dokumenty sprzedaży", "SalesDocuments", "POS")%></div>
        </div>
        <div class="button wood">
            <div>
                <%: Html.ActionLink("Koniec", "End", "POS")%></div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
