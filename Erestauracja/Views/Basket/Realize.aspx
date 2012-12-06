<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.ServiceReference.BasketRest>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Twoje zamówienie - wybierz sopsób zapłaty</h2>
<br />
<% if (Model != null) %>
<% { %>
    <%: Model.DisplayName%> (czyOnline?) Razem: <%: Model.TotalPriceRest%> zł
        <div>
            <div>Kontakt: <%: Model.Telephone%></div>
            <div>Przewidywany czas dostawy: <%: Model.DeliveryTime%></div>
            <div>Koszt dostawy: <%: Model.DeliveryPrice%> zł</div>
            <br />
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
                <br />
            <% } %>
            <div>Komentarz do zamówienia
                <%: Html.TextAreaFor(m=>m.Comment, new { id="comment"}) %>
            </div>
            <%: Html.ActionLink("Płatność gotówką przy odbiorze", "Cash", "Basket", new { com = "koment", id = (int)ViewData["id"], res = Model.RestaurantId }, new{@id = "acash"})%>
            <%--<%: Html.ActionLink("a         a", "PostToPaypal", "PayPal", new { com = "koment", id = (int)ViewData["id"], res = Model.RestaurantId }, new { @id = "acash", @class = "paypalbutton" })%>--%>
            <br />
            <div>
                <input type="image" id="paypalButton" onclick="posttopaypal('<%: ViewData["id"] %>','<%: Model.RestaurantId %>')" src="https://www.sandbox.paypal.com/en_US/i/btn/btn_buynowCC_LG.gif" border="0" name="submit" alt="PayPal - The safer, easier way to pay online!">
            </div>
            

        </div>
<% } %>

    <script type="text/javascript">
            $("#acash").click(function () {
                var comm = $("#comment").val();
                this.href = this.href.replace("koment", comm);
            });
    </script>

    
    <script type="text/javascript">
        function posttopaypal(id, resid) {

            //
            var comm = $("#comment").val();
            

            var url = '<%: Url.Action("PostToPaypal", "PayPal") %>';
            var data = { comm: comm, id: id, resid: resid };

            $.post(url, data, function (data) {
                // window.location.href = data.redirectToUrl;

            });
            
        }
    </script>

</asp:Content>

