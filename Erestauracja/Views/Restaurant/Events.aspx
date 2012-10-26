<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Restaurant/Restaurant.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.EventsPageModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

<fieldset>
    <legend>Imprezy okolicznościowe - kliknij <%: Html.ActionLink("tutaj", "EditEventsPage", "ManagePanel", new { id = Model.RestaurantID }, null)%> aby edytować</legend>
        
     <asp:Panel ID="Panel5" class="PanelDowoz" runat="server" ScrollBars="Auto">
                <%= Html.DisplayFor(m => m.Events)%>
    </asp:Panel>

       
        </fieldset>

</asp:Content>
