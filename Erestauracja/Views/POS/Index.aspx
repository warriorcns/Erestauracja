<%@ Page Title="" Language="C#" MasterPageFile="~/Views/POS/POS.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="buttons-container">
        <div>
            <%: Html.ActionLink("Aktywne zamówienia", "ActiveOrders", "POS", new{ @class="button wood"})%></div>
        <div>
            <%: Html.ActionLink("Wszystkie zamówienia", "AllOrders", "POS", new { @class = "button wood" })%></div>
        <div>
            <%: Html.ActionLink("Dokumenty sprzedaży", "SalesDocuments", "POS", new { @class = "button wood" })%></div>
        <div>
            <%: Html.ActionLink("Koniec", "End", "POS", new { @class = "button wood" })%></div>
            
            <div> login:  <%: ViewData["loggg"]%> </div>
    </div>

    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
