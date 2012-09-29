<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.DeliveryPageModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<div class="PanelDowoz">
<fieldset>
    <legend>Dowóz - kliknij <%: Html.ActionLink("tutaj", "EditDeliveryPage", "ManagePanel", new { id = Model.RestaurantID }, null) %> aby edytować</legend>
        
     <asp:Panel ID="Panel5" class="PanelDowoz" runat="server" ScrollBars="Auto">
                <%= Html.DisplayFor(m => m.Delivery)%>
    </asp:Panel>

       
        </fieldset>
</div>

</asp:Content>
