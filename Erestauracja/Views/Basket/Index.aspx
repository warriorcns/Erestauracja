<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.ServiceReference.BasketOut>" %>
<%@ Import Namespace="Erestauracja.ServiceReference" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Runtime.Serialization" %>
<%@ Import Namespace="System.Runtime.Serialization.Formatters.Binary" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
    $(function () {
        $("#accordion").accordion({
            collapsible: true,
            active: false
        });
    });
</script>

<script runat="server" type="text/C#">
String Send(BasketRest data)
{
    Stream myStream = new MemoryStream();
    try
    {
        IFormatter formatter = new BinaryFormatter();
        formatter.Serialize(myStream, data);
        myStream.Seek(0, SeekOrigin.Begin);
        byte[] buffer = new byte[myStream.Length];
        myStream.Read(buffer, 0, (int)myStream.Length);

        return Convert.ToBase64String(buffer);
    }
    finally
    {
        myStream.Close();
    }
}
</script>


<% using (Html.BeginForm()) %>
<% { %>
<h2>Koszyk - wybierz produkty które chcesz zamówić a następnie kliknij 'Realizuj'</h2>
<h2>Kliknij <%: Html.ActionLink("tutaj", "ClearBasket", "Basket")%>, aby usunąć wszystkie pozycje z koszyka</h2>
</br>
<% if (Model != null) %>
<% { %>
    <div id="accordion">
    <% foreach (BasketRest rest in Model.Basket) %>
    <% { %>
    <h3><a href="#"><%: rest.DisplayName%> (czyOnline?) Razem: <%: rest.TotalPriceRest%> zł</a></h3>
        <div>
            <div>Kontakt: <%: rest.Telephone%></div>
            <div>Przewidywany czas dostawy: <%: rest.DeliveryTime%></div>
            <div>Koszt dostawy: <%: rest.DeliveryPrice%> zł</div>
            </br>
            <% foreach (BasketProduct product in rest.Products) %>
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
                    <span><%: Html.ActionLink("usuń z koszyka", "Delete", "Basket", new { id = product.BasketId }, null)%> </span>
                </div>
                </br>
            <% } %>
            <%: Html.ActionLink("Realizuj", "Realize", "Basket", new { data = Send(rest) }, null)%>
        </div>
    <% } %>
    </div>
<% } %>

<% } %>
</asp:Content>
