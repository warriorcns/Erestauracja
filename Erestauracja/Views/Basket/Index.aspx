<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.ServiceReference.BasketOut>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
    $(function () {
        $("#accordion").accordion({
            collapsible: true,
            active: false
        });
    });
</script>

<h2>Koszyk - wybierz produkty które chcesz zamówić a następnie kliknij 'Realizuj'</h2>
</br>
<% if (Model != null) %>
<% { %>
    <div id="accordion">
    <% foreach (Erestauracja.ServiceReference.BasketRest rest in Model.Basket) %>
    <% { %>
    <h3><a href="#"><%: rest.DisplayName%> (czyOnline?) Razem: <%: rest.TotalPriceRest %> zł</a></h3>
        <div>
            <div>Kontakt: <%: rest.Telephone %></div>
            <div>Przewidywany czas dostawy: <%: rest.DeliveryTime %></div>
            <div>Koszt dostawy: <%: rest.DeliveryPrice %> zł</div>
            </br>
            <% foreach(Erestauracja.ServiceReference.BasketProduct product in rest.Products) %>
            <% { %>
                <div>
                    <span><%: Html.CheckBox("isSelected", product.IsSelected) %></span>
                    <span>
                         <%: product.ProductName %> 
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
                    <span>Cena: <%: product.Price %>zł x <%: product.Count %> = <%: product.TotalPriceProd %>zł</span>
                    <span><%: Html.ActionLink("usuń z koszyka", "Delete", "Basket", new { id = product.BasketId }, null)%> </span>
                </div>
                </br>
            <% } %>
            <input type=button name="Realizuj"/>
        </div>
    <% } %>
    </div>
<% } %>
</asp:Content>
