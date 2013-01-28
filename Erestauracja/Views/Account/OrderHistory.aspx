<%@ Page Language="C#" MasterPageFile="~/Views/Account/Account.master" Inherits="System.Web.Mvc.ViewPage<List<Erestauracja.ServiceReference.UserOrder>>" %>
<%@ Import Namespace="Erestauracja.ServiceReference" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AccountPlaceHolder" runat="server">

    <script src="../../Scripts/jquery.maskedinput-1.3.js" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.maskedinput-1.3.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jQuery.datepicker-pl.js") %>" type="text/javascript"></script>

    
    <link rel="Stylesheet" type="text/css" href="~/Content/style/jqueryui/ui-lightness/jquery-ui-1.7.2.custom.css" />

    <script type="text/javascript">
        $(function () {
            $("#accordion").accordion({
                collapsible: true,
                active: false
            });
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $("#fromTxb, #toTxb").datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: 'c-100:c+0',
                dateFormat: 'dd/mm/yy'
            });
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $("#filterButton").button()
                .click(function (event) {
                    event.preventDefault();
                    var from = $("#fromTxb").val();
                    var to = $("#toTxb").val();

                    var url = '<%: Url.Action("FilterOrderHistory", "Account") %>';
                    var data = { from: from, to: to };

                    if (data.length != 0) {
                        $.post(url, data, function (data) {
                            window.location.href = data.redirectToUrl;
                        });
                    }
                });
        });
    </script>

    <script type="text/javascript">
        $(function ($) {
            $(document).ready(function () {
                $("#fromTxb , #toTxb").mask("99/99/9999");
             });
        });
    </script>

<%: Html.ValidationSummary(true, "Błąd:")%>
<% if (Model == null) %>
<% { %>
    <h2>Pobieranie aktywnych zamówień nie powiodło się. Przepraszamy za problemy, spróbuj później.</h2>
<% } %>
<% else %>
<% { %>
        <div>
            <span>Od: <%: Html.TextBox("od", ((DateTime)ViewData["from"]).ToShortDateString(), new { @id = "fromTxb"})%></span>
            <span>Do: <%: Html.TextBox("do", ((DateTime)ViewData["to"]).ToShortDateString(), new { @id = "toTxb" })%></span>
            <span><input type="button" id="filterButton" value="Filtruj" onclick="filtr()"/></span>
        </div>
    <% if (Model.Count == 0) %>
    <% { %>
        <h2>Brak zamówień.</h2>
    <% } %>
    <% else %>
    <% { %>
        
        <div id="accordion">
        <% foreach (UserOrder order in Model) %>
        <% { %>
            <h3><a href="#"><span>Numer:<%:order.OrderId%></span> <span><%: order.DisplayName %></span> <span>Razem: <%: order.Price%> zł</span>
            <% if (order.Payment == "cash") %>
            <% { %>
                <span>Płatność przy odbiorze</span>
            <% } %>
            <% else if(order.Payment.Contains("PayPal")) %>
            <% { %>
                <span>Płatność PayPal</span>
            <% } %>
            <% else %>
            <% { %>
                <span>Płatność inna</span>
            <% } %>
            <span>Status: <%: order.Status%></span>
            </a></h3>
            <div>Dane restauracji:
                <div>Adres: <%: order.Address%> - <%: order.Town%> (<%: order.Postal%>)</div>
                <div>Kontakt: <%: order.Telephone%></div>
                <div>Przewidywany czas dostawy: <%: order.DeliveryTime%></div>
                <div>Koszt dostawy: <%: order.DeliveryPrice%> zł</div>
                </br>
                <div>Data złożenia zamówienia: <%: order.OrderDate.ToShortDateString()%> <%: order.OrderDate.ToLongTimeString()%></div>
                <% if (order.Status == "Zakończone") %>
                <% { %>
                    <div>Data realizacji zamówienia: <%: order.FinishDate.ToShortDateString()%> <%: order.FinishDate.ToLongTimeString()%></div>
                <% } %>
                </br>
                <div>Produkty:</div>
                <% foreach (OrderedProduct product in order.Products) %>
                <% { %>
                    <% if (String.IsNullOrWhiteSpace( product.ProductName)) %>
                    <% { %>
                        <div>
                            Produkt usunięty z oferty restauracji.
                        </div>
                    <% } %>
                    <% else %>
                    <% { %>
                        <div>
                        <span>
                            <%: product.ProductName%>  x <%: product.Count%>
                            <% if (!String.IsNullOrWhiteSpace(product.PriceOption)) %>
                            <% { %>
                                <%: "- " + product.PriceOption%> 
                            <% } %>
                            <% if (!String.IsNullOrWhiteSpace(product.NonPriceOption)) %>
                            <% { %>
                                <%: "- " + product.NonPriceOption%> 
                            <% } %>
                            <% if (!String.IsNullOrWhiteSpace(product.NonPriceOption2)) %>
                            <% { %>
                                <%: "- " + product.NonPriceOption2%> 
                            <% } %>
                        </span>
                        <span>
                            <% if (!String.IsNullOrWhiteSpace(product.Comment)) %>
                            <% { %>
                                <%: " (" + product.Comment + ") "%> 
                            <% } %>
                        </span>
                    </div>
                    <% } %>
                <% } %>
                </br>
                <% if (!String.IsNullOrWhiteSpace(order.Comment)) %>
                <% { %>
                    <div>Komentarz do zamówienia:</div>
                    <div><%: order.Comment %></div>
                <% } %>
            </div>
        <% } %>
        </div>
    <% } %>
<% } %>

</asp:Content>
