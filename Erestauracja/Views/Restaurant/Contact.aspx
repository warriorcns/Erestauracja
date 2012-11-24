<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Restaurant/Restaurant.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.ContactPageModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">


<fieldset>    
     <asp:Panel ID="Panel5" class="PanelDowoz" runat="server" ScrollBars="Auto">
                <%= Html.DisplayFor(m => m.Contact)%>
    </asp:Panel>

       
        </fieldset>


</asp:Content>