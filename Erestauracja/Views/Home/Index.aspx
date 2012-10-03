<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.HomeModels>" ErrorPage="~/Views/Shared/Unauthorized.aspx"%>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="title" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>
<asp:Content class="main" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%: ViewBag.Message %></h2>
        
    <form id="form1" runat="server">
    <br />
    <div class="all">
        <asp:Panel class="lewypanel" runat="server" ScrollBars="Auto" Wrap="true">
            <asp:DropDownList ID="DropDownList1" name="DropDownList1" class="DropDownListKategorie"
                runat="server">
            </asp:DropDownList>
            <asp:BulletedList class="BulletedListRestauracje" runat="server" BulletStyle="CustomImage"
                BulletImageUrl="">
                <asp:ListItem>Restauracja 1</asp:ListItem>
                <asp:ListItem>Restauracja 2</asp:ListItem>
                <asp:ListItem>Restauracja 3</asp:ListItem>
                <asp:ListItem>Restauracja 4</asp:ListItem>
                <asp:ListItem>Restauracja 5</asp:ListItem>
            </asp:BulletedList>
        </asp:Panel>
        <asp:Panel class="PanelInfo" runat="server" ScrollBars="Auto" Wrap="true">
            <asp:Label class="LabelWybierzMiasto" runat="server" Font-Size="Large" Text="Na stronie możesz zamówić posiłki przygotowane przez restauracje wraz z dowozem do domu !"></asp:Label>
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
        <asp:Panel class="Panelstatystyki" runat="server" ScrollBars="Auto"
            Wrap="true">
            <asp:Label ID="Label" class="LabelWybierzMiasto" runat="server" Text="Ilość odwiedzin:"></asp:Label>
            <div class="licznik">
                <!-- GoStats JavaScript Based Code -->
                <script type="text/javascript" src="http://gostats.pl/js/counter.js"></script>
                <script type="text/javascript">                    _gos = 'c3.gostats.pl'; _goa = 366701;
                    _got = 2; _goi = 58; _gol = 'licznik blog'; _GoStatsRun();</script>
                <noscript>
                    <a target="_blank" title="licznik blog" href="http://gostats.pl">
                        <img alt="licznik blog" src="http://c3.gostats.pl/bin/count/a_366701/t_2/i_58/counter.png"
                            style="border-width: 0" /></a></noscript>
                <br />
                <!-- End GoStats JavaScript Based Code -->
            </div>

            Ilość zalogowanych użytkowników (na podstawie providera):
            <% CustomMembershipProvider onlineCount = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"]; %>
            <% int o = onlineCount.GetNumberOfUsersOnline(); %>
            <%: Html.Label(o.ToString())%>
            
        </asp:Panel>
    </div>
    </form>
    
    <script type="text/javascript">
        //document.getElementById("TxtBox").onblur() = document.getElementById("ButtonWybierzMiasto").onclick();
        $('#target').blur(function () {
            var town = $("#target").val();
            var url = '<%: Url.Action("SearchRestaurants", "Home") %>';
            var data = { value: town };
            $.post(url, data, function (data) {
                // TODO: do something with the response from the controller action
                //alert('the value was successfully sent to the server');
                //wypelniam dynamicznie DDL danymi zwroconymi przez metode - lista restauracji
                //czysci liste
                $('#Restauracje').empty();
                $('#Restauracje').append($("<option selected=\"selected\"/>").val('0').text('Wszystkie'));
                $.each(data, function () {
                    
                    
                    $("#Restauracje").append($("<option selected=\"selected\"/>").val(this.Value).text(this.Text));
                    
                });
                $("#Restauracje").val('0');
            });

            //pokazuje DDL z restauracjami
            document.getElementById("Restauracje").style.display = "block";
        });
    </script>
    <%--<%: Url.Action("Restaurant", "CitiesAndRestaurants") %>--%>
    <script type="text/javascript">
        function test() {alert('test');}
    </script>
    <script type="text/javascript">
        //przekierowanie po wyborze restauracji do strony restauracji z pobraniem id restauracji

        $("#Restauracje").change(function () {
            var str = "";
            $("select option:selected").each(function () {
                str += $(this).val() + " ";
            });
            //$("div").text(str);
            //redirect to res page
            alert('test:' + str);

            var url = '<%: Url.Action("Restaurant", "CitiesAndRestaurants") %>';
            var data = { value: str };
            if ( !$(str).val() ) {
                $.post(url, data, function (data) {
                    // TODO: do something with the response from the controller action
                    alert('the value was successfully sent to the server' + str);
                });
            }
        }).trigger('change');

    </script>
</asp:Content>
