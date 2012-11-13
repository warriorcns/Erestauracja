﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/POS/POS.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="../../Scripts/jquery.tools.min.js" type="text/javascript"></script>

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

    <div class="main">
        <div class="buttons-container">
            <div>
                <%: Html.ActionLink("Pokaż zamawiane produkty", "ActiveOrders", "POS", new{ @class="button wood"})%></div>
        </div>

        <div class="orders-container">
            <span class="orders-header orders-header-phone">Telefon</span> <span class="orders-header orders-header-name">
                Nazwisko</span> <span class="orders-header orders-header-adress">Adres</span>
            <span class="orders-header orders-header-status">Status zamówienia</span>
            <%--accordion--%>
            <div class="order-container">
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

    <div>
        <%: Html.TextBox("szukaj", null, new { id = "searchtxb", @class = "search-textbox" })%>
        <button>Szukaj</button>
    </div>
    
    

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
