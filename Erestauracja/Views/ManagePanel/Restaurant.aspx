<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.ServiceReference.Restaurant>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Restaurant
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script>
    $(function () {
        $("#accordion").accordion({
            collapsible: true
        });
    });
</script>

<h2>Wybierz restaurację, którą chcesz zarządzać lub kliknij <%: Html.ActionLink("tutaj", "AddRestaurant", "ManagePanel")%>, aby dodać nową. </h2>
</br>
</br>

<div class="demo">
    <div id="accordion">
        <% foreach (Erestauracja.ServiceReference.Restaurant x in (IEnumerable<Erestauracja.ServiceReference.Restaurant>)ViewData["restauracje"]){%>
	    <h3><a href="#"><%: x.Name %></a></h3>
	    <div>
                <div class="editor-labelE">
                Name:  <%: x.Name%>
                </div>
                <div class="editor-labelE">
                DisplayName:  <%: x.DisplayName%>
                </div>
                <div class="editor-labelE">
                Address:  <%: x.Address%>
                </div>
                <div class="editor-labelE">
                TownID:  <%: x.TownID%>
                </div>
                <div class="editor-labelE">
                Country:  <%: x.Country%>
                </div>
                <div class="editor-labelE">
                Telephone:  <%: x.Telephone%>
                </div>
                <div class="editor-labelE">
                Email:  <%: x.Email%>
                </div>
                <div class="editor-labelE">
                Nip:  <%: x.Nip%>
                </div>
                <div class="editor-labelE">
                Regon:  <%: x.Regon%>
                </div>
                <div class="editor-labelE">
                DeliveryTime:  <%: x.DeliveryTime%>
                </div>
                <%: Html.ActionLink("Edytuj dane", "EditRestaurant", "ManagePanel", x, null)%>
                <%: Html.ActionLink("Zarządzaj", "ManageRestaurant", "ManagePanel", x, null)%>

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
