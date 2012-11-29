<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.PayPal>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    PostToPayPal
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var frm = $("#form1");
            frm.submit();
        });
    </script>
    <h2>
        Przekierowuje na strone PayPala..</h2>
    <form id="form1" action="<%:ViewBag.actionURL %>">
        <%:Html.HiddenFor(m => m.cmd) %>
        <%:Html.HiddenFor(m => m.business) %>
        <%:Html.HiddenFor(m => m.no_shipping) %>
        <%:Html.HiddenFor(m => m.@return) %>
        <%:Html.HiddenFor(m => m.cancel_return) %>
        <%:Html.HiddenFor(m => m.notify_url) %>
        <%:Html.HiddenFor(m => m.currency_code) %>
        <%:Html.HiddenFor(m => m.item_number)%>
        <%:Html.HiddenFor(m => m.shipping)%>
        <%:Html.HiddenFor(m => m.amount) %>

    </form>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
