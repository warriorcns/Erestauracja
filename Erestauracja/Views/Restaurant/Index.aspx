﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Restaurant/Restaurant.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.MainPageModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<div class="PaneleOpisRestauracji">
<fieldset>    
        <asp:Panel ID="Panel1" class="PanelOpisRestauracji" runat="server" ScrollBars="Auto">
           <p style="position: relative; padding: 10px 10px 10px 10px; font-size: 15px;"> 
                <%= Html.DisplayFor(m => m.Description)%>
           </p> 
        </asp:Panel>
        <asp:Panel ID="Panel2" class="PanelTotoPromocje" runat="server" ScrollBars="auto">
            <asp:Panel ID="Panel3" class="PanelToto" runat="server" ScrollBars="auto">
                <%= Html.DisplayFor(m => m.Foto)%>
            </asp:Panel>
            <asp:Panel ID="Panel4" class="PanelPromocje" runat="server" ScrollBars="auto">
                <%= Html.DisplayFor(m => m.SpecialOffers)%>
            </asp:Panel>
        </asp:Panel>
        </fieldset>
</div>
</asp:Content>