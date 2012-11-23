<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.HomeModels>" ErrorPage="~/Views/Shared/Unauthorized.aspx"%>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="title" ContentPlaceHolderID="TitleContent" runat="server">Home Page</asp:Content>

<asp:Content class="main" ContentPlaceHolderID="MainContent" runat="server">

    <form id="form1" runat="server">
    <br />
    <div class="all">
        <asp:Panel class="lewypanel" runat="server" ScrollBars="Auto" Wrap="true">
            <asp:Label Font-Bold="true" Font-Underline="true" runat="server">   10 najnowszych restauracji</asp:Label>
            <% foreach(Erestauracja.ServiceReference.RestaurantTop rest in (List<Erestauracja.ServiceReference.RestaurantTop>)ViewData["top"]) %>
            <% { %>
                    <div onclick="Redirect('<%: Html.Encode(rest.ID) %>')">
                        <div> <%: rest.DisplayName %></div>
                        <div> <%: rest.Address %> <%: rest.Town %> <%: rest.PostalCode %></div>
                        <div> <%: rest.Telephone %> </div>
                    </div>
                    <hr />
            <% } %>
         
        </asp:Panel>
        <asp:Panel class="PanelInfo" runat="server" ScrollBars="Auto" Wrap="true">
            <asp:Label class="LabelWybierzMiasto" runat="server" Font-Size="Large" 
                Text="Na stronie możesz zamówić posiłki przygotowane przez restauracje wraz z dowozem do domu !">
            </asp:Label>
        </asp:Panel>
        <asp:Panel class="PanelWybor" runat="server" ScrollBars="Auto" Wrap="true">
            <form>
            <asp:Label class="LabelWybierzMiasto" runat="server" Text="Wpisz miasto:"></asp:Label>
            <%--<%=Html.DropDownList("Miasta", ViewData["Miasta"] as SelectList, new { @class = "DropDownListWybierzMiasto" })%>--%>
            <%: Html.TextBoxFor(m => m.TownName, new { id = "target", @class = "ButtonWybierzMiasto" })%>
            <%--<asp:Button ID="ButtonWybierzMiasto" class="ButtonWybierzMiasto" runat="server" Text="Szukaj"
                Font-Size="Small"/>--%>

            <asp:Label class="LabelWybierzRestauracje" runat="server" Text="Wybierz restauracje:"></asp:Label>
            <%: Html.DropDownList("rest", ViewData["rest"] as SelectList, new { id = "Restauracje", @class = "DropDownListWybierzRestauracje" })%>
            <asp:Button ID="ButtonWybierzRestauracje" class="ButtonWybierzRestauracje" runat="server"
                Text="Szczegółowe wyszukiwanie" Font-Size="Small" />
            </form>
        </asp:Panel>
        <asp:Panel class="Panelstatystyki" runat="server" ScrollBars="Auto" Wrap="true">
            <!--<asp:Label ID="Label" class="LabelWybierzMiasto" runat="server" Text="Ilość odwiedzin:"></asp:Label>
            <div class="licznik">
                 GoStats JavaScript Based Code 
                <script type="text/javascript" src="http://gostats.pl/js/counter.js"></script>
                <script type="text/javascript">                    _gos = 'c3.gostats.pl'; _goa = 366701;
                    _got = 2; _goi = 58; _gol = 'licznik blog'; _GoStatsRun();</script>
                <noscript>
                    <a target="_blank" title="licznik blog" href="http://gostats.pl">
                        <img alt="licznik blog" src="http://c3.gostats.pl/bin/count/a_366701/t_2/i_58/counter.png"
                            style="border-width: 0" /></a></noscript>
                <br />
                <!-- End GoStats JavaScript Based Code 
            </div>
            Ilość zalogowanych użytkowników (na podstawie providera):
            <% CustomMembershipProvider onlineCount = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"]; %>
            <% int o = onlineCount.GetNumberOfUsersOnline(); %>
            <%: Html.Label(o.ToString())%>
            -->
            <% Erestauracja.ServiceReference.Statistics staty = (Erestauracja.ServiceReference.Statistics)ViewData["stat"]; %>
            </br>
            <div> Na naszej stwonie znajdziesz <strong> <%: staty.ProductsCount %> </strong> produktów,</div>
            <div> dostępnych w <strong> <%: staty.RestaurantsCount %> </strong> restauracjach.</div>
            </br>
            <div> Dołączyło do nas już <strong> <%: staty.UsersCount %> </strong> osób.</div>
            </br>
            <div> Obecnie na stronie znajduje sie <strong> <%: staty.ActiveUsers %> </strong> aktywnych użytkowników,</div>
            <div> a także<strong> <%: staty.ActiveRestaurants %> </strong> aktywnych restauracji.</div>
        </asp:Panel>
    </div>
    </form>
    
    <script type="text/javascript">
        $('#target').blur(function () {
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
        });
    </script>
    
    <script type="text/javascript">
        function test() {alert('test');}
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

</asp:Content>
