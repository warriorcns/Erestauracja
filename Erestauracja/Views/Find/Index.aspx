<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Erestauracja.ServiceReference.RestaurantInCity>>" %>
<%@ Import Namespace="Erestauracja.ServiceReference" %>
<%@ Import Namespace="Jmelosegui.Mvc.Controls" %>
<%@ Import Namespace="Telerik.Web.Mvc.UI"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
    $(function () {
        $("#searchButton").button()
            .click(function (event) {
                event.preventDefault();
                var town = $("#townTB").val();
                var res = $("#resTB").val();

                var url = '<%: Url.Action("Search", "Find") %>';
                var data = { town: town, res: res };

                if (data.length != 0) {
                    $.post(url, data, function (data) {
                        window.location.href = data.redirectToUrl;
                    });
                }
            });
    });
</script>

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
    } 
    </script>

    <%: Html.ValidationSummary(true, "Błąd.")%>

    <div>
    <span>Podaj nazwe miasta </span>
    <span><%: Html.TextBox("town", (string)ViewData["town"], new { @id = "townTB" })%></span>
    <span> lub nazwe restauracji </span>
    <span><%: Html.TextBox("res", (string)ViewData["res"], new { @id = "resTB" })%></span>
    <span><input type="button" id="searchButton" value="Szukaj" onclick="search()"/></span>
    </div>

    <div style="width: 100%; clear: both; height: 100%; overflow: hidden;">
    <% if (Model == null) %>
    <% { %>
        <h2>Szukanie restauracji nie powiodło się. Przepraszamy za problemy, spróbuj później.</h2>
    <% } %>
    <% else %>
    <% { %>
        <% if (Model.Count == 0 && (bool)ViewData["first"]==false) %>
        <% { %>
            <h2>Brak restauracji spełniającej podane kryteria.</h2>
        <% } %>
        <% else %>
        <% { %>
        <div class="RestauracjeMapa">
            <span class="PanelListaRestauracji">
                <% foreach (Erestauracja.ServiceReference.RestaurantInCity item in Model) %>
                <% { %>
                        <div onclick="Redirect('<%: Html.Encode(item.ID) %>')">
                            <div> <%: item.DisplayName %> 
                            <% if( (DateTime.Compare(item.CreationDate, DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0)))) > 0 )%>
                            <% { %>
                                NOWOŚĆ
                            <% } %>
                            </div>
                            <div><%: item.Address %> <%: item.Town %> <%: item.PostalCode %></div>
                            <div><%: item.Telephone %></div>
                            <div>Srednia ocena <%: item.AverageRating %></div>
                        </div>
                        <hr />
                <% } %>
            </span>
            <span class="PanelMapa">
                <div class="mapTowns" id="mapka" style="display: block">
                    <% Html.RenderPartial("RestaurantsMaps", Model as IEnumerable<Erestauracja.ServiceReference.RestaurantInCity>);%>
                    <%--Renderuje mapke oraz dzialaja inne jQery skrypty--%>
                    <% Html.Telerik().ScriptRegistrar().jQuery(false).jQueryValidation(false).OnDocumentReady("$('#mapTowns').dialog();").Render(); %>
                </div>
            </span>
        </div>
        <% } %>
    <% } %>
    </div>
</asp:Content>
