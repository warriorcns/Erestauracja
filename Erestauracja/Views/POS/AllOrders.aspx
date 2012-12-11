<%@ Page Title="" Language="C#" MasterPageFile="~/Views/POS/POS.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.ServiceReference.Order[]>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<meta charset="utf-8"/>
<script src="http://code.jquery.com/jquery-1.8.3.js"></script>
<script src="http://code.jquery.com/ui/1.9.2/jquery-ui.js"></script>
<link rel="stylesheet" href="http://code.jquery.com/ui/1.9.2/themes/base/jquery-ui.css" />


    



<script type="text/javascript">
    $(function () {
        $(".order-container").accordion({
            collapsible: true,
            active: false,
            autoHeight: false,
            event: "click"
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

                var url = '<%: Url.Action("FilterOrders", "POS") %>';
                var data = { from: from, to: to };

                if (data.length != 0) {
                    $.post(url, data, function (data) {
                        window.location.href = data.redirectToUrl;
                    });
                }
            });
        });

</script>

    <div class="main">
        <div class="buttons-container">
            <div>
                <%: Html.ActionLink("Cofnij", "Index", "POS", new{ @class="button wood"})%>
            </div>
            <div>
                <span>Od:
                    <%: Html.TextBox("od", ((DateTime)ViewData["from"]).ToShortDateString(), new { @id = "fromTxb"})%></span>
                <span>Do:
                    <%: Html.TextBox("do", ((DateTime)ViewData["to"]).ToShortDateString(), new { @id = "toTxb" })%></span>
                <span>
                    <input type="button" id="filterButton" value="Filtruj" onclick="filtr()" /></span>
            </div>
        </div>
        <div style="position:relative; top:50px; left: 20px;">
            <div class="orders-container">
            <span class="orders-header orders-header-phone">Telefon</span> <span class="orders-header orders-header-name">
                        Nazwisko</span> <span class="orders-header orders-header-adress">Adres</span>
                    <span class="orders-header orders-header-status">Status i data zamówienia: </span>
            <%--accordion--%>
            <div class="order-container">
            <% foreach (Erestauracja.ServiceReference.Order order in Model as IEnumerable)
               {%>
                    

                    <h3 class="orders-header-main">
                    <a class="order orders-header-phone">
                        <%: order.UserTelephone %></a> <a class="order orders-header-name">
                            <%: order.UserName %></a> <a class="order orders-header-adress">
                                <%: order.UserAdderss %>
                                <%: order.UserTown %>
                            </a><a class="order orders-header-status">
                                <%: order.Status %>:
                                <%: order.OrderDate %></a>
                </h3>
                    <div>
                        <p style="border-top: 2px black solid;">
                        Informacje o zamówieniu</p>
                        <div>Lista zamówionych produktów:</div>
                        <br />
                        <div id="order-list" >
                            <% int TotalCount = 0; foreach (Erestauracja.ServiceReference.OrderedProduct product in order.Products)
                               {
                                   TotalCount += product.Count; %>
                                <div style="text-indent: 20px;"><%: product.ProductName%> - <%: product.PriceOption%> (<%: product.Count%> szt.) </div>
                                <div style="text-indent: 40px;">Opcje: <%: product.NonPriceOption%> , <%: product.NonPriceOption2%></div>
                                <div style="text-indent: 40px;"> <% if(product.Comment.Length > 0){%> Komentarz do produktu: <%: product.Comment%> <% }%></div>
                                <div>---------------------------------------</div>
                            <% } %>
                        </div>
                        <br />
                        <div>Data zamówienia: <%: order.OrderDate %></div>
                        <div>Sposób zapłaty: <% if (order.Payment == "cash")
                                                { %>Gotówka<%}
                                                else if (order.Payment.Contains("PayPal")) 
                                                {%> PayPal<%} %></div>
                        <div><%if (order.Comment.Length > 0)
                               { %> Komentarz do zamówienia: <%: order.Comment%><% } %></div>
                        <div>Adres dostawy: <%: order.UserAdderss %> <%: order.UserTown %> </div>
                        <div>Podsumowanie: (<%: TotalCount %> szt.) - <%: order.Price %> zł</div>
                        <div id="print-order">
                            <button>Drukuj rachunek.</button>
                        </div>
                        
                        </div>
                    
                <% } %>
            </div>

        </div>
        </div>
    </div>

    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
