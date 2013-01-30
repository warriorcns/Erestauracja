<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.ServiceReference.Restaurant>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Restaurant
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script>
    $(function () {
        $("#accordion").accordion({
            collapsible: true,
            active: false
        });
    });
</script>

<h2>Wybierz restaurację, którą chcesz zarządzać lub kliknij <%: Html.ActionLink("tutaj", "AddRestaurant", "ManagePanel")%>, aby dodać nową. </h2>
</br>
</br>

<div class="demo">
    <div id="accordion">
        <% foreach (Erestauracja.ServiceReference.Restaurant x in (IEnumerable<Erestauracja.ServiceReference.Restaurant>)ViewData["restauracje"]){%>
	    <h3><a href="#"><%: x.Name %>
        <% if(x.IsEnabled == false) %>
        <% { %>
                (Niewidoczna dla klientów)
        <% } %>
        </a></h3>
	    <div>
                <div class="editor-labelE">
                Login:  <%: x.Login%>
                </div>
                <div class="editor-labelE">
                Adres email:  <%: x.Email%>
                </div>
                <div class="editor-labelE">
                Nazwa:  <%: x.Name%>
                </div>
                <div class="editor-labelE">
                Nazwa wyświetlana:  <%: x.DisplayName%>
                </div>
                <div class="editor-labelE">
                Adres:  <%: x.Address%>
                </div>
                <div class="editor-labelE">
                Miasto:  <%: x.Town%>
                </div>
                <div class="editor-labelE">
                Kod pocztowy:  <%: x.PostalCode%>
                </div>
                <div class="editor-labelE">
                Kraj:  <%: x.Country%>
                </div>
                <div class="editor-labelE">
                Numer telefonu:  <%: x.Telephone%>
                </div>
                <div class="editor-labelE">
                Nip:  <%: x.Nip%>
                </div>
                <div class="editor-labelE">
                Regon:  <%: x.Regon%>
                </div>
                <div class="editor-labelE">
                Czas dostawy:  <%: x.DeliveryTime%>
                </div>
                <div class="editor-labelE">
                Cena dostawy:  <%: x.DeliveryPrice%>
                </div>
                <div class="editor-labelE">
                Ilość odwiedzin:  <%: x.InputsCount%>
                </div>
                <div class="editor-labelE">
                Srednia ocena:  <%: x.AverageRating%>
                </div>
                <div class="editor-labelE">
                Ostatnio aktywna:  <%: x.LastActivityDate %>
                </div>
                <%: Html.ActionLink("Edytuj dane", "EditRestaurant", "ManagePanel", new { id = x.ID }, null)%>
                <%: Html.ActionLink("Zmiana hasła", "ChangePassword", "ManagePanel", new { login = x.Login }, null)%>
                <%: Html.ActionLink("Zarządzaj", "MainPage", "ManagePanel", new { id = x.ID }, null)%>
                <%: Html.ActionLink("Usuń", "DeleteRestaurant", "ManagePanel", new { id = x.ID }, null)%>
	    </div>
	<% } %>
    
</div>

</div>


  <!--  <asp:Panel class="Restauracje" ID="Restauracje" runat="server">
        <asp:ListView ID="ListView1" runat="server">
            <LayoutTemplate>
                <div style="border: solid 2px #336699; width: 20%;">
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <div style="border: solid 1px #336699;">
                    <%# Eval("ProductName")%>
                </div>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <div style="border: solid 1px #336699; background-color: #dadada;">
                    <%# Eval("ProductName")%>
                </div>
            </AlternatingItemTemplate>
            <EmptyDataTemplate>
                No records found
            </EmptyDataTemplate>
        </asp:ListView>
    </asp:Panel> -->
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
