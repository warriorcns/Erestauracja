<%@ Page Title="" Language="C#" MasterPageFile="~/Views/POS/POS.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <script type="text/javascript">
        $(function () {
            $("#setStatus")
            .button()
            .click(function () {
                //event.preventDefault();
                var st = $("#setStatus").text();
                if (st === "Online") {
                    st = "Offline";
                } else {
                    st = "Online";
                }
                var url = '<%: Url.Action("status", "POS") %>';
                var data = { st: st };
                $.post(url, data, function (data) {
                    $("#setStatus").text(st);
                });
                return false;
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            var url = '<%: Url.Action("getStatus", "POS") %>';
            var data = { };
            $.post(url, data, function (data) {
                $("#setStatus").text(data);
            });
        });
    </script>
    <div class="buttons-container">
        <div>
            <%: Html.ActionLink("Aktywne zamówienia", "ActiveOrders", "POS", new{ @class="button wood"})%></div>
        <div>
            <%: Html.ActionLink("Wszystkie zamówienia", "AllOrders", "POS", new { from = ( DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)) ).ToShortDateString(), to = DateTime.Now.ToShortDateString() }, new { @class = "button wood" })%></div>
        <%--<div>
            <%: Html.ActionLink("Dokumenty sprzedaży", "SalesDocuments", "POS", new { @class = "button wood" })%></div>--%>
        
        <div>
            <%: Html.ActionLink("Koniec", "End", "POS", new { @class = "button wood" })%></div>
        <div>
            <button id="setStatus" class="button wood">Online</button></div>
    </div>

    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.9.2/themes/base/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.8.3.js"></script>
    <script src="http://code.jquery.com/ui/1.9.2/jquery-ui.js"></script>
</asp:Content>
