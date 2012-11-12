<%@ Page Title="" Language="C#" MasterPageFile="~/Views/POS/POS.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.9.1/jquery-ui.js"></script>

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
            $("#up, #down")
            .button()
            .click(function (event) {
                event.preventDefault();
                //alert("test");
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            // initialize scrollable
            $(".scrollable").scrollable({ vertical: true, mousewheel: true });
        });
    </script>

    <div class="main">
        <div class="buttons-container">
            <div>
                <%: Html.ActionLink("Pokaż zamawiane produkty", "ActiveOrders", "POS", new{ @class="button wood"})%></div>
        </div>
        
                <div class="orders-container">

                    <span class="orders-header orders-header-phone">Telefon</span> <span class="orders-header orders-header-name">
                        Nazwisko</span> <span class="orders-header orders-header-adress">Adres</span>
                    <span class="orders-header orders-header-status">Status zamówienia</span>

                    <%--pojedyncze zamowienie -accordion--%>
                    <div class="order-container">

                        <!-- root element for scrollable -->
                        <div class="scrollable vertical">

                            <!-- root element for the scrollable elements -->
                            <div class="items">
                                <%--tutaj petla wypelniajaca zamowienia odswiezana przez jquery chyba--%>
                                <!-- first element. contains three rows -->
                                <div>
                                    <!-- first row -->
                                    <div class="item">
                                        <h3 class="orders-header-main">
                                            <a class="order orders-header-phone">123123 123123 1231231 23123123</a> <a class="order orders-header-name">
                                                kowalski 1231231 23123123 123123 123123</a> <a class="order orders-header-adress">zielona
                                                    222 - dane statyczne 1231231 231231 2312312 3123123</a> <a class="order orders-header-status">
                                                        Aktywne 1231231 23123 12312312 3123123</a>
                                        </h3>
                                        <div>
                                            <p>
                                                INFO O ZAMOWIENIU</p>
                                        </div>
                                    </div>
                                    <!-- 2 row -->
                                    <div class="item">
                                        <h3 class="orders-header-main">
                                            <a class="order orders-header-phone">123123 123123 1231231 23123123</a> <a class="order orders-header-name">
                                                kowalski 1231231 23123123 123123 123123</a> <a class="order orders-header-adress">zielona
                                                    222 - dane statyczne 1231231 231231 2312312 3123123</a> <a class="order orders-header-status">
                                                        Aktywne 1231231 23123 12312312 3123123</a>
                                        </h3>
                                        <div>
                                            <p>
                                                INFO O ZAMOWIENIU</p>
                                        </div>
                                    </div>
                                    <!-- 3 row -->
                                    <div class="item">
                                        <h3 class="orders-header-main">
                                            <a class="order orders-header-phone">123123 123123 1231231 23123123</a> <a class="order orders-header-name">
                                                kowalski 1231231 23123123 123123 123123</a> <a class="order orders-header-adress">zielona
                                                    222 - dane statyczne 1231231 231231 2312312 3123123</a> <a class="order orders-header-status">
                                                        Aktywne 1231231 23123 12312312 3123123</a>
                                        </h3>
                                        <div>
                                            <p>
                                                INFO O ZAMOWIENIU</p>
                                        </div>
                                    </div>
                                </div>

                                <!-- 2 element. contains three rows -->
                                <div>
                                    <!-- first row -->
                                    <div class="item">
                                        <h3 class="orders-header-main">
                                            <a class="order orders-header-phone">123123 123123 1231231 23123123</a> <a class="order orders-header-name">
                                                kowalski 1231231 23123123 123123 123123</a> <a class="order orders-header-adress">zielona
                                                    222 - dane statyczne 1231231 231231 2312312 3123123</a> <a class="order orders-header-status">
                                                        Aktywne 1231231 23123 12312312 3123123</a>
                                        </h3>
                                        <div>
                                            <p>
                                                INFO O ZAMOWIENIU</p>
                                        </div>
                                    </div>
                                    <!-- 2 row -->
                                    <div class="item">
                                        <h3 class="orders-header-main">
                                            <a class="order orders-header-phone">123123 123123 1231231 23123123</a> <a class="order orders-header-name">
                                                kowalski 1231231 23123123 123123 123123</a> <a class="order orders-header-adress">zielona
                                                    222 - dane statyczne 1231231 231231 2312312 3123123</a> <a class="order orders-header-status">
                                                        Aktywne 1231231 23123 12312312 3123123</a>
                                        </h3>
                                        <div>
                                            <p>
                                                INFO O ZAMOWIENIU</p>
                                        </div>
                                    </div>
                                    <!-- 3 row -->
                                    <div class="item">
                                        <h3 class="orders-header-main">
                                            <a class="order orders-header-phone">123123 123123 1231231 23123123</a> <a class="order orders-header-name">
                                                kowalski 1231231 23123123 123123 123123</a> <a class="order orders-header-adress">zielona
                                                    222 - dane statyczne 1231231 231231 2312312 3123123</a> <a class="order orders-header-status">
                                                        Aktywne 1231231 23123 12312312 3123123</a>
                                        </h3>
                                        <div>
                                            <p>
                                                INFO O ZAMOWIENIU</p>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            
        <div class="scroll-buttons">
            <div>
                <button id="up" class="button arrow-up">
                    </button>
                <button id="down" class="button arrow-down">
                    </button>
            </div>
        </div>
        
    </div>
    <div>
        <%: Html.TextBox("szukaj", null, new { id = "searchtxb", @class = "search-textbox" })%>
    </div>
    <div id="actions">
    <a class="prev">&laquo; Back</a>
    <a class="next">More pictures &raquo;</a>
  </div>
    

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
