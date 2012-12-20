<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.HomeModels>" ErrorPage="~/Views/Shared/Unauthorized.aspx"%>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="title" ContentPlaceHolderID="TitleContent" runat="server">Strona główna</asp:Content>

<asp:Content class="main" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function callMethod() {
            //IsOnline
            $('.ResID').each(function () {
                var Resid = $(this).val();
                var url = '<%: Url.Action("IsOnline", "Home") %>';
                var data = { id: Resid };
                $.post(url, data, function (data) {
                    $(".resIsOnline" + Resid).text("(" + data + ")");
                    if (data === "Online") {
                        $(".resIsOnline" + Resid).css("color", "#66CC00");
                    }
                    else {
                        $(".resIsOnline" + Resid).css("color", "#FF0000");
                    }
                });
            });
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            callMethod();
            setInterval("callMethod()", 30000); //600000ms = 10min
        });
    </script>

    <form id="form1" runat="server">
    <br />
    <div class="all">
        <asp:Panel class="lewypanel" runat="server" ScrollBars="Auto" Wrap="true">
            <asp:Label Font-Bold="true" runat="server" style="position: relative; padding: 5% 0 0 10%; font-size:larger;">   10 najnowszych restauracji</asp:Label>
            <hr />
            <% foreach(Erestauracja.ServiceReference.RestaurantTop rest in (List<Erestauracja.ServiceReference.RestaurantTop>)ViewData["top"]) %>
            <% { %>
                    <input class="ResID" name="id" type="hidden" value="<%: rest.ID %>" />
                    <div class="resInfo" onclick="Redirect('<%: Html.Encode(rest.ID) %>')">
                        <div>
                            <span> <%: rest.DisplayName %></span> <span class="resIsOnline<%: rest.ID %>"></span>
                        </div>
                        <div> <%: rest.Address %> <%: rest.Town %> <%: rest.PostalCode %></div>
                        <div> <%: rest.Telephone %> </div>
                    </div>
                    <hr />
            <% } %>
         
        </asp:Panel>
        <asp:Panel class="PanelInfo" runat="server" ScrollBars="Auto" Wrap="true">
            <asp:Label class="LabelWybierzMiasto" runat="server" Font-Size="Large" 
                Text="System E-Restauracja umożliwia wyszukiwanie oraz przeglądanie oferty restauracji z całego kraju.
Zalogowani klienci mają możliwość dodawania produktów oferowanych przez restauracje do koszyka,
a następnie wybierając płatność, za pomocą serwisu PayPal, kartą kredytową, kartą debetową lub płatność przy odbiorze, złożyć zamówienie.">
            </asp:Label>
        </asp:Panel>
        <asp:Panel class="PanelWybor" runat="server" ScrollBars="Auto" Wrap="true">
            <div>
            

                <asp:Label class="LabelWybierzMiasto" runat="server" Text="Wpisz miasto:"></asp:Label>
                <%--<%=Html.DropDownList("Miasta", ViewData["Miasta"] as SelectList, new { @class = "DropDownListWybierzMiasto" })%>--%>
                <%: Html.TextBoxFor(m => m.TownName, new { id = "target", @class = "ButtonWybierzMiasto searchTxb" })%>
                <asp:Button ID="searchRes" class="ButtonWybierzMiasto" runat="server" Text="Szukaj"
                    Font-Size="Small"/>

                <asp:Label class="LabelWybierzRestauracje" id="chooseRes" runat="server" Text="Wybierz restauracje:"></asp:Label>
                <%: Html.DropDownList("rest", ViewData["rest"] as SelectList, new { id = "Restauracje", @class = "DropDownListWybierzRestauracje" })%>
            
                <%--<asp:Button ID="advancedSearch" class="ButtonWybierzRestauracje" runat="server"
                    Text="Szczegółowe wyszukiwanie" Font-Size="Small" />--%>
                    <input type="button" value="Szczegółowe wyszukiwanie" id="advancedSearch" class="ButtonWybierzRestauracje"/>
                                
            </div>
            
        </asp:Panel>
        <asp:Panel class="Panelstatystyki" runat="server" ScrollBars="Auto" Wrap="true">
            <% Erestauracja.ServiceReference.Statistics staty = (Erestauracja.ServiceReference.Statistics)ViewData["stat"]; %>
            
            <div class="stats">
                <div> Na naszej stwonie znajdziesz <strong> <%: staty.ProductsCount %> </strong> produktów,</div>
                <div> dostępnych w <strong> <%: staty.RestaurantsCount %> </strong> restauracjach.</div>
                <br/>
                <div> Dołączyło do nas już <strong> <%: staty.UsersCount %> </strong> osób.</div>
                <br/>
                <div> Obecnie na stronie znajduje sie <strong> <%: staty.ActiveUsers %> </strong> aktywnych użytkowników,</div>
                <div> a także<strong> <%: staty.ActiveRestaurants %> </strong> aktywnych restauracji.</div>
            </div>
        </asp:Panel>
    </div>
    </form>
    
    <script type="text/javascript">
        $('#MainContent_searchRes').click(function () {

            var town = $("#target").val();
            var url = '<%: Url.Action("SearchRestaurants", "Home") %>';
            var data = { value: town };
            $.post(url, data, function (data) {
                $('#Restauracje').empty();
                if (data.length !== 0) {
                    $('#Restauracje').append($("<option selected=\"selected\"/>").val('0').text('Wybierz z listy..'));
                    $('#Restauracje').append($("<option selected=\"selected\"/>").val('0|' + town).text('Wszystkie'));
                    $.each(data, function () {
                        $("#Restauracje").append($("<option selected=\"selected\"/>").val(this.Value).text(this.Text));
                    });
                    $("#Restauracje").val('0');
                }
                else {
                    $('#Restauracje').append($("<option selected=\"selected\"/>").val('0').text('Brak restauracji dla tego miasta..'));
                    $("#Restauracje").val('0');
                }
            });
            document.getElementById("Restauracje").style.display = "block";
            document.getElementById("MainContent_chooseRes").style.display = "inline";

            return false;
        });
    </script>
    
    <script type="text/javascript">
        //przekierowanie po wyborze restauracji do strony restauracji z pobraniem id restauracji
        $("#Restauracje").change(function () {
            var str = "";
            $("select option:selected").each(function () {
                str = $(this).val();
            });
            //redirect to res page
            //alert('test->' + str.length + '<-test');
            //przekierowac do innej metody jezeli user wybierze opcje 'wszystkie'

            var url = '<%: Url.Action("GetRequest", "Restaurant") %>';
            var data = { id: str };

            if (str.length != 0) {
                $.post(url, data, function (data) {
                   // TODO: do something with the response from the controller action
                    //alert('the value was successfully sent to the server' + str);
                    window.location.href = data.redirectToUrl;
                });
            }
        }).trigger('change');
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

    <script type="text/javascript">
        $('#advancedSearch').click(function () {
            var town = "";
            var res = "";
            var first = true;
            var url = '<%: Url.Action("getReq", "Find") %>';
            var data = { town: town, res: res, first: first };
            $.post(url, data, function (data) {
                window.location.href = data.redirectToUrl;
            });
        });
    </script>


</asp:Content>
