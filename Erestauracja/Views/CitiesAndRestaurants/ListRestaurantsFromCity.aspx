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
            <div class="mapTowns" id="mapka" style="display: block">
                <% Html.RenderPartial("RestaurantsMaps", Model.Restaurants as IEnumerable<Erestauracja.ServiceReference.RestaurantInCity>);%>
                <%--Renderuje mapke oraz dzialaja inne jQery skrypty--%>
                <% Html.Telerik().ScriptRegistrar().jQuery(false).jQueryValidation(false).OnDocumentReady("$('#mapTowns').dialog();").Render(); %>
            </div>
        </span>
    </div>
    <!-- /////////////////////// 
    <div class="RestauracjeMapa">
    <asp:Panel class="PanelListaRestauracji" runat="server" ScrollBars="Auto">
    </asp:Panel>
    <asp:Panel class="PanelMapa" runat="server">
    </asp:Panel>
    </div>-->
    <script type="text/javascript">        
    function Redirect(RestaurantID) {
        var url = '<%: Url.Action("GetRequest", "Restaurant") %>';
        var data = { id: RestaurantID };
        //alert(RestaurantID);
        if (RestaurantID.length != 0) {
            $.post(url, data, function (data) {
                // TODO: do something with the response from the controller action
                //alert('the value was successfully sent to the server' + str);
                window.location.href = data.redirectToUrl;
            });

        }
        }     </script>
</asp:Content>
