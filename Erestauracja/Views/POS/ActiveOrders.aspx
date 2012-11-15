<%@ Page Title="" Language="C#" MasterPageFile="~/Views/POS/POS.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="../../Scripts/jquery.tools.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $(".order-container").accordion({
                collapsible: true,
                active: false,
                autoHeight: true,
                event: "click"
            });
        });
    </script>


    <div class="main">
        <div class="buttons-container">
            <div>
                <%: Html.ActionLink("Pokaż zamawiane produkty", "ActiveOrders", "POS", new{ @class="button wood"})%></div>
        </div>
        
            <div class="orders-container" id="orders">
            <span class="orders-header orders-header-phone">Telefon</span> <span class="orders-header orders-header-name">
                Nazwisko</span> <span class="orders-header orders-header-adress">Adres</span>
            <span class="orders-header orders-header-status">Status zamówienia</span>
            <%--accordion--%>
            <div class="order-container">
                <h3 class="orders-header-main">
                    <a class="order orders-header-phone">1231231231</a> <a class="order orders-header-name">
                        Kowalski Jan</a> <a class="order orders-header-adress">ul. Zielona 222/B </a> <a class="order orders-header-status">
                                Aktywne</a>
                </h3>
                <div>
                    <p style="border-top: 2px black solid;">
                        Informacje o zamówieniu</p>
                        <div>Lista zamówionych produktów:</div>
                        <ul id="order-list">
                            
                            <li>1.Pizza Margerita (1 szt.) - 20zl</li>
                            <li>2. Kebab z frytkami (2 szt.) - 20zl</li>
                            
                        </ul>
                        <div>Podsumowanie: (3 szt.) - 30zl</div>
                        <div id="print-order">
                            <button>Drukuj rachunek.</button>
                        </div>
                        <%--<%: Html.DropDownListFor(m = > m.Status, ViewData["status"] as IEnumerable<>) %>--%>
                </div>

                 
                <h3 class="orders-header-main">
                    <a class="order orders-header-phone">56546456</a> <a class="order orders-header-name">
                        Nowak Anna</a> <a class="order orders-header-adress">ul. Morska 23 </a> <a class="order orders-header-status">
                                W realizacji</a>
                </h3>
                <div>
                    <p style="border-top: 2px black solid;">
                        Informacje o zamówieniu</p>
                        <div>Lista zamówionych produktów:</div>
                        <ul id="Ul1">
                            
                            <li>1.Pizza Margerita (1 szt.) - 20zl</li>
                            <li>2. Kebab z frytkami (2 szt.) - 20zl</li>
                            
                        </ul>
                        <div>Podsumowanie: (3 szt.) - 30zl</div>
                        <div id="Div1">
                            <button>Drukuj rachunek.</button>
                        </div>
                        <%--<%: Html.DropDownListFor(m = > m.Status, ViewData["status"] as IEnumerable<>) %>--%>
                </div>


                <h3 class="orders-header-main">
                    <a class="order orders-header-phone">56546456</a> <a class="order orders-header-name">
                        Nowak Anna</a> <a class="order orders-header-adress">ul. Morska 23 </a> <a class="order orders-header-status">
                                W realizacji</a>
                </h3>
                <div>
                    <p style="border-top: 2px black solid;">
                        Informacje o zamówieniu</p>
                        <div>Lista zamówionych produktów:</div>
                        <ul id="Ul2">
                            
                            <li>1.Pizza Margerita (1 szt.) - 20zl</li>
                            <li>2. Kebab z frytkami (2 szt.) - 20zl</li>
                            
                        </ul>
                        <div>Podsumowanie: (3 szt.) - 30zl</div>
                        <div id="Div2">
                            <button>Drukuj rachunek.</button>
                        </div>
                        <%--<%: Html.DropDownListFor(m = > m.Status, ViewData["status"] as IEnumerable<>) %>--%>
                </div>


                <h3 class="orders-header-main">
                    <a class="order orders-header-phone">56546456</a> <a class="order orders-header-name">
                        Nowak Anna</a> <a class="order orders-header-adress">ul. Morska 23 </a> <a class="order orders-header-status">
                                W realizacji</a>
                </h3>
                <div>
                    <p style="border-top: 2px black solid;">
                        Informacje o zamówieniu</p>
                        <div>Lista zamówionych produktów:</div>
                        <ul id="Ul3">
                            
                            <li>1.Pizza Margerita (1 szt.) - 20zl</li>
                            <li>2. Kebab z frytkami (2 szt.) - 20zl</li>
                            
                        </ul>
                        <div>Podsumowanie: (3 szt.) - 30zl</div>
                        <div id="Div3">
                            <button>Drukuj rachunek.</button>
                        </div>
                        <%--<%: Html.DropDownListFor(m = > m.Status, ViewData["status"] as IEnumerable<>) %>--%>
                </div>


                <h3 class="orders-header-main">
                    <a class="order orders-header-phone">56546456</a> <a class="order orders-header-name">
                        Nowak Anna</a> <a class="order orders-header-adress">ul. Morska 23 </a> <a class="order orders-header-status">
                                W realizacji</a>
                </h3>
                <div>
                    <p style="border-top: 2px black solid;">
                        Informacje o zamówieniu</p>
                        <div>Lista zamówionych produktów:</div>
                        <ul id="Ul4">
                            
                            <li>1.Pizza Margerita (1 szt.) - 20zl</li>
                            <li>2. Kebab z frytkami (2 szt.) - 20zl</li>
                            
                        </ul>
                        <div>Podsumowanie: (3 szt.) - 30zl</div>
                        <div id="Div4">
                            <button>Drukuj rachunek.</button>
                        </div>
                        <%--<%: Html.DropDownListFor(m = > m.Status, ViewData["status"] as IEnumerable<>) %>--%>
                </div>


                <h3 class="orders-header-main">
                    <a class="order orders-header-phone">56546456</a> <a class="order orders-header-name">
                        Nowak Anna</a> <a class="order orders-header-adress">ul. Morska 23 </a> <a class="order orders-header-status">
                                W realizacji</a>
                </h3>
                <div>
                    <p style="border-top: 2px black solid;">
                        Informacje o zamówieniu</p>
                        <div>Lista zamówionych produktów:</div>
                        <ul id="Ul5">
                            
                            <li>1.Pizza Margerita (1 szt.) - 20zl</li>
                            <li>2. Kebab z frytkami (2 szt.) - 20zl</li>
                            
                        </ul>
                        <div>Podsumowanie: (3 szt.) - 30zl</div>
                        <div id="Div5">
                            <button>Drukuj rachunek.</button>
                        </div>
                        <%--<%: Html.DropDownListFor(m = > m.Status, ViewData["status"] as IEnumerable<>) %>--%>
                </div>


                <h3 class="orders-header-main">
                    <a class="order orders-header-phone">56546456</a> <a class="order orders-header-name">
                        Nowak Anna</a> <a class="order orders-header-adress">ul. Morska 23 </a> <a class="order orders-header-status">
                                W realizacji</a>
                </h3>
                <div>
                    <p style="border-top: 2px black solid;">
                        Informacje o zamówieniu</p>
                        <div>Lista zamówionych produktów:</div>
                        <ul id="Ul6">
                            
                            <li>1.Pizza Margerita (1 szt.) - 20zl</li>
                            <li>2. Kebab z frytkami (2 szt.) - 20zl</li>
                            
                        </ul>
                        <div>Podsumowanie: (3 szt.) - 30zl</div>
                        <div id="Div6">
                            <button>Drukuj rachunek.</button>
                        </div>
                        <%--<%: Html.DropDownListFor(m = > m.Status, ViewData["status"] as IEnumerable<>) %>--%>
                </div>


               
            </div>

        </div>
        
    </div>

    <div>
        <%: Html.TextBox("szukaj", null, new { id = "searchtxb", @class = "search-textbox" })%>
        <button>Szukaj</button>
    </div>
    
    

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
