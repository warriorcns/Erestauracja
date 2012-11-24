<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Restaurant/Restaurant.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.EventsPageModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

<fieldset>      
     <asp:Panel ID="Panel5" class="PanelDowoz" runat="server" ScrollBars="Auto">
                <%= Html.DisplayFor(m => m.Events)%>
    </asp:Panel>

       
        </fieldset>

</asp:Content>
