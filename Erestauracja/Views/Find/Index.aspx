<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Erestauracja.ServiceReference.RestaurantInCity>>" %>
<%@ Import Namespace="Erestauracja.ServiceReference" %>
<%@ Import Namespace="Jmelosegui.Mvc.Controls" %>
<%@ Import Namespace="Telerik.Web.Mvc.UI"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Wyszukiwanie zaawansowane
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


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
    <% if ( (ViewData["town"]) == null && (ViewData["res"]) == null ) 
       { %>
        <script type="text/javascript">
            $(function () {
                $('.PanelMapa').css("display", "none");
                $('.PanelListaRestauracji').css("display", "none");
            });
        </script>
    <% }
       else
       {%>
       <script type="text/javascript">
           $(function () {
               $('.PanelMapa').css("display", "block");
               $('.mapTowns').css("display", "block");
               $('.PanelListaRestauracji').css("display", "block");
           });
       </script>
       <% } %>
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
            <span class="PanelListaRestauracji" id="resList">
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
                            <div>Srednia ocena: <span class="stars" data-rating="<%: item.AverageRating.ToString("F",System.Globalization.CultureInfo.CreateSpecificCulture("en-CA")) %>"></span></div>
                        </div>
                        <hr />
                <% } %>
            </span>
            <span class="PanelMapa">
                <div class="FindmapTowns" id="mapka">
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
