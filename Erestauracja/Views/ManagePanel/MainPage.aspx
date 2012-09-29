<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.MainPageModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<div class="PaneleOpisRestauracji">
<fieldset>
    <legend>Strona główna - kliknij <%:Html.ActionLink("tutaj", "EditMainPage", "ManagePanel", new { id = Model.RestaurantID }, null)%> aby edytować</legend>
        
        <asp:Panel ID="Panel1" class="PanelOpisRestauracji" runat="server" ScrollBars="Auto">
           <p style="position: relative; padding: 10px 10px 10px 10px; font-size: 15px;"> 
                <%= Html.DisplayFor(m => m.Description)%>
           </p> 
        </asp:Panel>
        <asp:Panel ID="Panel2" class="PanelTotoPromocje" runat="server" ScrollBars="Auto">
            <asp:Panel ID="Panel3" class="PanelToto" runat="server" ScrollBars="Auto">
                <%= Html.DisplayFor(m => m.Foto)%>
            </asp:Panel>
            <asp:Panel ID="Panel4" class="PanelPromocje" runat="server" ScrollBars="Auto">
                <%= Html.DisplayFor(m => m.SpecialOffers)%>
            </asp:Panel>
        </asp:Panel>
        </fieldset>
</div>
</asp:Content>

