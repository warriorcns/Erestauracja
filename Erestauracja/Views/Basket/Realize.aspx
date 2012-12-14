<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.ServiceReference.BasketRest>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% if (Model != null) %>
<% { %>
    <h2>Twoje zamówienie - wybierz sopsób zapłaty</h2>
    <br />
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
                <%: Html.TextAreaFor(m => m.Comment, new { id = "comment" })%>
            </div>
            <br />
            <div> Wybierz typ płatności: </div>
            
            <%: Html.ActionLink("Płatność gotówką przy odbiorze", "Cash", "Basket", new { com = "koment", id = (int)ViewData["id"], res = Model.RestaurantId }, new { @id = "acash" })%>
            <br />
            <br />
            <%: Html.ActionLink("Zaplac paypalem", "PostToPaypal", "PayPal", new { com = "koment", id = (int)ViewData["id"], resid = Model.RestaurantId }, new { @id = "paypal", @class = "paypalbutton" })%>
            <br />
        </div>
<% } %>
<% else %>
<% { %>
    <h2>Uwaga! - <%: ViewData["error"] %></h2>
    <br />
    <%: Html.ActionLink("Powrót do koszyka", "Index", "Basket")%>       
<% } %>

<script type="text/javascript">
    $("#acash , #paypal").click(function () {
        var comm = $("#comment").val();
        this.href = this.href.replace("koment", comm);
    });
    </script>
<%--    <script type="text/javascript">
            $("#acash").click(function () {
                var comm = $("#comment").val();
                this.href = this.href.replace("koment", comm);
            });
    </script>
    <script type="text/javascript">
        $("#paypal").click(function () {
            var comm = $("#comment").val();
            this.href = this.href.replace("komentP", comm);
        });
    </script>--%>

    
    <script type="text/javascript">
        function posttopaypal(id, resid) {

            //
            var comm = $("#comment").val();


            var url = '<%: Url.Action("GetRequest", "PayPal") %>';
            var data = { comm: comm, id: id, resid: resid };

            $.post(url, data, function (data) {
                //window.location.href = data.
                //window.location.href = data;
                //$(document).load(data);
                //alert(data.redirect);
                //window.location.href = data.redirectToUrl;
            });
            
        }
    </script>

</asp:Content>

