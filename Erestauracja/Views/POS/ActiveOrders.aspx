<%@ Page Title="" Language="C#" MasterPageFile="~/Views/POS/POS.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
    $("#orders").accordion({
        collapsible: true,
        active: false,
        autoHeight: false
    });
</script>


    <div id="orders">
        <%--glowny naglowek tabeli--%>
        <h3 class="orders-header-main">
            <a href="#" class="orders-header">Telefon</a>
            <a href="#" class="orders-header">Nazwisko</a>
            <a href="#" class="orders-header">Adres</a>
            <a href="#" class="orders-header">Status zamówienia</a>
        </h3>
        
    </div>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
