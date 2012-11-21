<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.ServiceReference.BasketOut>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h2>Koszyk - wybierz produkty które chcesz zamówić a następnie kliknij 'Realizuj'</h2>
</br>
<% if (Model != null) %>
<% { %>
    <% foreach (Erestauracja.ServiceReference.BasketRest rest in Model.Basket) %>
    <% { %>
        <div>
            <%: rest.DisplayName %>
        </div>
    <% } %>
<% } %>
</asp:Content>
