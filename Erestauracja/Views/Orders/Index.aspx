<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Erestauracja.ServiceReference.UserOrder>>" %>
<%@ Import Namespace="Erestauracja.ServiceReference" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
    $(function () {
        $("#accordion").accordion({
            collapsible: true,
            active: false
        });
    });
</script>

<%: Html.ValidationSummary(true, "Błąd.")%>
<% if (Model == null) %>
<% { %>
    <h2>Pobieranie aktywnych zamówień nie powiodło się. Przepraszamy za problemy, spróbuj później.</h2>
<% } %>
<% else %>
<% { %>
    <% if (Model.Count == 0) %>
    <% { %>
        <h2>Brak aktywnych zamóweiń.</h2>
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
            <% else if(order.Payment == "payPal") %>
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
