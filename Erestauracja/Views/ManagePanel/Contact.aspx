<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.ContactPageModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">


<fieldset>
    <legend>Kontakt - kliknij <%: Html.ActionLink("tutaj", "EditContactPage", "ManagePanel", new { id = Model.RestaurantID }, null) %> aby edytować</legend>
        
     <asp:Panel ID="Panel5" class="PanelDowoz" runat="server" ScrollBars="Auto">
                <%= Html.DisplayFor(m => m.Contact)%>
    </asp:Panel>

       
        </fieldset>


</asp:Content>
