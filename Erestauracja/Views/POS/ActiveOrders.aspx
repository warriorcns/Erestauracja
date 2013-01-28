<%@ Page Title="" Language="C#" MasterPageFile="~/Views/POS/POS.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.ServiceReference.AllOrders>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <meta charset="utf-8"/>
<META http-equiv="Content-Type" content="text/html; charset=UTF-8">

    <%--<script src="../../Scripts/jquery.tools.min.js" type="text/javascript"></script>--%>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.9.2/themes/base/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.8.3.js"></script>
    <script src="http://code.jquery.com/ui/1.9.2/jquery-ui.js"></script>

    <link href="../../Content/style/keyboard.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery.keyboard.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.keyboard.extension-navigation.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.keyboard.extension-typing.js" type="text/javascript"></script> 

    <script type="text/javascript">
        $(function () {
            $('#searchtxb').keyboard({
                layout: 'international',
                autoAccept: 'true'
            });
        });
    </script>
    
    
    <script type="text/javascript">
        $(function () {
            $(".order-container").accordion({
                collapsible: true,
                active: false,
                heightStyle: "content",
                event: "click hoverintent"
                
            });
           
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $("#tabs-nohdr").tabs({
                
            });
        });
	</script>
    <script type="text/javascript">
        $(function () {
            $("#searchButton")
            .button()
            .click(function (event) {
                event.preventDefault();
                var txt = $("#searchtxb").val();
                if (txt.length != 0) {
                    $('h3').show();
                    $('h3:not(h3:contains("' + txt + '"))').hide();
                }
            });
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            setInterval("location.reload(true)", 300000);
        });
    </script>
    <script type="text/javascript">
        function reload(){
            location.reload(true);
        }
    </script>
    <script type="text/javascript">
        function setSt(id, stat) {
            event.preventDefault();
            //var from = $("#fromTxb").val();
            //var to = $("#toTxb").val();

            var url = '<%: Url.Action("setStatus", "POS") %>';
            var data = { id: id, stat: stat };

            if (data.length != 0) {
                $.post(url, data, function (data) {
                    window.location.href = data.redirectToUrl;
                });
            }
        }
    </script>
    <script type="text/javascript">
        function printdiv(id) {
            $("#but" + id).print();

        }  
    </script>

    
    
    <div class="main">
        <div class="buttons-container">
            
            <div>
                <input type="button" onclick="reload()" class="button wood" value="Odswież zamówienia" />
            </div>
            <div>
                <%: Html.ActionLink("Cofnij", "Index", "POS", new{ @class="button wood"})%>
            </div>
            
        </div>
        <div id="tabs-nohdr">
            <ul>
                <li><a href="#tabs-1"><span>Oczekujące</span></a></li>
                <li><a href="#tabs-2"><span>Aktywne</span></a></li>
                <li><a href="#tabs-3"><span>Zakończone</span></a></li>
            </ul>
            
            <div class="orders-container" id="orders">
                <div class="header">
                    <span class="orders-header orders-header-phone">Telefon</span> <span class="orders-header orders-header-name">
                        Nazwisko</span> <span class="orders-header orders-header-adress">Adres</span>
                    <span class="orders-header orders-header-status">Status i data zamówienia: </span>
                </div>
                <div id="tabs-1">
                <%--accordion--%>
                    <div id="waiting" class="order-container">
            
                <% foreach (Erestauracja.ServiceReference.Order order in Model.Waiting)
                   {%>
               
                        <h3 class="orders-header-main">
                            <a class="order orders-header-phone"><%: order.UserTelephone %></a> <a class="order orders-header-name">
                            <%: order.UserName %></a> <a class="order orders-header-adress"><%: order.UserAdderss %> <%: order.UserTown %>  </a> <a class="order orders-header-status">
                            <%: order.Status %>: <%: order.OrderDate %></a>
                        </h3>
                        <div id="<%: order.OrderId  %>">
                        <p style="border-top: 2px black solid;">
                        Informacje o zamówieniu</p>
                        <div>Lista zamówionych produktów:</div>
                        <br />
                        <div id="order-list" >
                            <% int TotalCount = 0; foreach (Erestauracja.ServiceReference.OrderedProduct product in order.Products)
                               {
                                   TotalCount += product.Count; %>
                                   <% if (String.IsNullOrWhiteSpace(product.ProductName)) %>
                                   <% { %>
                                        <div style="text-indent: 20px;">Produkt usunięty z oferty restauracji.</div>
                                   <% } %>
                                   <% else %>
                                   <% { %>
                                        <div style="text-indent: 20px;"><%: product.ProductName%> - <%: product.PriceOption%> (<%: product.Count%> szt.) </div>
                                        <div style="text-indent: 40px;">Opcje: <%: product.NonPriceOption%> , <%: product.NonPriceOption2%></div>
                                        <div style="text-indent: 40px;"> <% if(product.Comment.Length > 0){%> Komentarz do produktu: <%: product.Comment%> <% }%></div>
                                    <% } %>
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
                            <%--<button>Drukuj rachunek.</button>--%>
                            <button onclick="setSt('<%: order.OrderId %>', 'W realizacji')">Przyjmij zamówienie</button>
                            <button onclick="setSt('<%: order.OrderId %>', 'Odrzucone')">Odrzuć zamówienie</button>
                            <%--<button onclick="printdiv(<%: order.OrderId %>)">print</button>--%>
                        </div>
                        <%--<%: Html.DropDownListFor(m = > m.Status, ViewData["status"] as IEnumerable<>) %>--%>
                        </div>
           
                <% } %>
                </div>
                </div>

                <div id="tabs-2">
                    <%--accordion--%>
                    <div id="Active" class="order-container">
            
                <% foreach (Erestauracja.ServiceReference.Order order in Model.Active)
                   {%>
               
                        <h3 class="orders-header-main">
                            <a class="order orders-header-phone"><%: order.UserTelephone %></a> <a class="order orders-header-name">
                            <%: order.UserName %></a> <a class="order orders-header-adress"><%: order.UserAdderss %> <%: order.UserTown %>  </a> <a class="order orders-header-status">
                            <%: order.Status %>: <%: order.OrderDate %></a>
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
                                   <% if (String.IsNullOrWhiteSpace(product.ProductName)) %>
                                   <% { %>
                                        <div style="text-indent: 20px;">Produkt usunięty z oferty restauracji.</div>
                                   <% } %>
                                   <% else %>
                                   <% { %>
                                        <div style="text-indent: 20px;"><%: product.ProductName%> - <%: product.PriceOption%> (<%: product.Count%> szt.) </div>
                                        <div style="text-indent: 40px;">Opcje: <%: product.NonPriceOption%> , <%: product.NonPriceOption2%></div>
                                        <div style="text-indent: 40px;"> <% if(product.Comment.Length > 0){%> Komentarz do produktu: <%: product.Comment%> <% }%></div>
                                    <% } %>
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
                            <%--<button>Drukuj rachunek.</button>--%><button onclick="setSt('<%: order.OrderId %>', 'Zakończone')">Wysłano</button>
                        </div>
                        <%--<%: Html.DropDownListFor(m = > m.Status, ViewData["status"] as IEnumerable<>) %>--%>
                        </div>
           
                <% } %>
                </div>
                </div>

                <div id="tabs-3">
                <%--accordion--%>
                    <div id="Finished" class="order-container">
            
                    <% foreach (Erestauracja.ServiceReference.Order order in Model.Finish)
                    {%>
               
                        <h3 class="orders-header-main">
                            <a class="order orders-header-phone"><%: order.UserTelephone %></a> <a class="order orders-header-name">
                            <%: order.UserName %></a> <a class="order orders-header-adress"><%: order.UserAdderss %> <%: order.UserTown %>  </a> <a class="order orders-header-status">
                            <%: order.Status %>: <%: order.OrderDate %></a>
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
                                   <% if (String.IsNullOrWhiteSpace(product.ProductName)) %>
                                   <% { %>
                                        <div style="text-indent: 20px;">Produkt usunięty z oferty restauracji.</div>
                                   <% } %>
                                   <% else %>
                                   <% { %>
                                        <div style="text-indent: 20px;"><%: product.ProductName%> - <%: product.PriceOption%> (<%: product.Count%> szt.) </div>
                                        <div style="text-indent: 40px;">Opcje: <%: product.NonPriceOption%> , <%: product.NonPriceOption2%></div>
                                        <div style="text-indent: 40px;"> <% if(product.Comment.Length > 0){%> Komentarz do produktu: <%: product.Comment%> <% }%></div>
                                    <% } %>
                                    <div>---------------------------------------</div>
                            <% } %>
                        </div>
                        <br />
                        <div>Data zamówienia: <%: order.OrderDate %></div>
                        <div>Data zakończenia: <%: order.FinishDate %></div>
                        <div>Sposób zapłaty: <% if (order.Payment == "cash")
                                                { %>Gotówka<%}
                                                else if (order.Payment.Contains("PayPal")) 
                                                {%> PayPal<%} %></div>
                        <div><%if (order.Comment.Length > 0)
                               { %> Komentarz do zamówienia: <%: order.Comment%><% } %></div>
                        <div>Adres dostawy: <%: order.UserAdderss %> <%: order.UserTown %> </div>
                        <div>Podsumowanie: (<%: TotalCount %> szt.) - <%: order.Price %> zł</div>
                        <div id="print-order">
                            <%--<button>Drukuj rachunek.</button>--%>
                        </div>
                        <%--<%: Html.DropDownListFor(m = > m.Status, ViewData["status"] as IEnumerable<>) %>--%>
                        </div>
           
                    <% } %>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div>
        <%: Html.TextBox("szukaj", null, new { @id = "searchtxb", @class = "search-textbox" })%>
        <button id="searchButton" onclick="search">Szukaj</button>
    </div>
    
    

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
