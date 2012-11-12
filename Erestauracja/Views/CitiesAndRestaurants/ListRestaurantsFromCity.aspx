<%@ Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.ServiceReference.RestaurantsFromCity>" %>

<%@ Import Namespace="Jmelosegui.Mvc.Controls" %>
<%@ Import Namespace="Telerik.Web.Mvc.UI"%>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <div class="LabelNazwaMiasta" runat="server"> <%: Model.CityName %>   </div>
    
    <div class="RestauracjeMapa">
        <span class="PanelListaRestauracji">
            <% foreach (Erestauracja.ServiceReference.RestaurantInCity item in Model.Restaurants) %>
            <% { %>
                    <div>
                        <div> <%: item.DisplayName %></div>
                        <% if(item.CreationDate == DateTime.Now ) //powinno być 2 tygodnie wcześniej %>
                        <% { %>
                            <div>NOWOŚC</div>
                        <% } %>
                        <div><%: item.Address %> <%: item.Town %> <%: item.PostalCode %></div>
                        <div><%: item.Telephone %></div>
                        <div>Srednia ocena <%: item.AverageRating %></div>
                    </div>
                    </br>
            <% } %>
        </span>
        <span class="PanelMapa">
        </span>
    </div>
    <!-- /////////////////////// 
    <div class="RestauracjeMapa">
    <asp:Panel class="PanelListaRestauracji" runat="server" ScrollBars="Auto">
    </asp:Panel>
    <asp:Panel class="PanelMapa" runat="server">
    </asp:Panel>
    </div>-->
</asp:Content>
