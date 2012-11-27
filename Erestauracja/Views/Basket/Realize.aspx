<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.ServiceReference.BasketRest>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Twoje zamówienie - wybierz sopsób zapłaty</h2>
</br>
<% if (Model != null) %>
<% { %>
    <%: Model.DisplayName%> (czyOnline?) Razem: <%: Model.TotalPriceRest%> zł
        <div>
            <div>Kontakt: <%: Model.Telephone%></div>
            <div>Przewidywany czas dostawy: <%: Model.DeliveryTime%></div>
            <div>Koszt dostawy: <%: Model.DeliveryPrice%> zł</div>
            </br>
            <% foreach (Erestauracja.ServiceReference.BasketProduct product in Model.Products) %>
            <% { %>
                <div>
                    <span>
                         <%: product.ProductName%> 
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
                    <span>Cena: <%: product.Price%>zł x <%: product.Count%> = <%: product.TotalPriceProd%>zł</span>
                </div>
                </br>
            <% } %>
            <div>Komentarz do zamówienia
                <%: Html.TextAreaFor(m=>m.Comment, new { id="comment"}) %>
            </div>
            <%: Html.ActionLink("Płatność gotówką przy odbiorze", "Cash", "Basket", new { com = ??, id = (int)ViewData["id"], res = Model.RestaurantId }, null)%>
        </div>
<% } %>

</asp:Content>

