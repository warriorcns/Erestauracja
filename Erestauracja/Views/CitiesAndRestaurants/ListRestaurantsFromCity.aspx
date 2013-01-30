<%@ Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.ServiceReference.RestaurantsFromCity>" %>

<%@ Import Namespace="Jmelosegui.Mvc.Controls" %>
<%@ Import Namespace="Telerik.Web.Mvc.UI"%>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <div class="main">
    <script src="../../Scripts/jquery.raty.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('.stars').raty({
                half: true,
                path: "../../Content/images/",
                readOnly: true,
                score: function () { return $(this).attr('data-rating'); }
            });
        });      
    </script>
    
        <div class="LabelNazwaMiasta" runat="server"> <%: Model.CityName %>   </div>
    <div class="main">
        <div class="RestauracjeMapa">
            <div class="PanelListaRestauracji">
            <% foreach (Erestauracja.ServiceReference.RestaurantInCity item in Model.Restaurants) %>
            <% { %>
                    <div class="FindresInfo" onclick="Redirect('<%: Html.Encode(item.ID) %>')">
                        <div> <%: item.DisplayName %> 
                        <% if( (DateTime.Compare(item.CreationDate, DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0)))) > 0 )%>
                        <% { %>
                            NOWOŚĆ
                        <% } %>
                        </div>
                        <div><%: item.Address %> <%: item.Town %> <%: item.PostalCode %></div>
                        <div><%: item.Telephone %></div>
                        <div>Srednia ocena:  <span class="stars" data-rating="<%: item.AverageRating.ToString("F",System.Globalization.CultureInfo.CreateSpecificCulture("en-CA")) %>"></span></div>
                    </div>
                    <hr />
            <% } %>
        </div>
            <div class="PanelMapa">
                <div class="FindmapTowns" id="mapka">
                    <% Html.RenderPartial("RestaurantsMaps", Model.Restaurants as IEnumerable<Erestauracja.ServiceReference.RestaurantInCity>);%>
                    <%--Renderuje mapke oraz dzialaja inne jQery skrypty--%>
                    <% Html.Telerik().ScriptRegistrar().jQuery(false).jQueryValidation(false).OnDocumentReady("$('#mapTowns').dialog();").Render(); %>
                </div>
            </div>
        </div>
    </div>
    
    <script type="text/javascript">        
    function Redirect(RestaurantID) {
        var url = '<%: Url.Action("GetRequest", "Restaurant") %>';
        var data = { id: RestaurantID };
        if (RestaurantID.length != 0) {
            $.post(url, data, function (data) {
                window.location.href = data.redirectToUrl;
            });
       }
    } 
    </script>

</asp:Content>
