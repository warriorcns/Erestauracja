<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Restaurant/Restaurant.master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content class="main" ContentPlaceHolderID="Main" runat="server">
    <div class="PaneleOpisRestauracji">
        <asp:Panel class="PanelOpisRestauracji" runat="server" ScrollBars="Auto">
            <p style="position: relative; padding: 10px 10px 10px 10px; font-size: 15px;">
                Nasza restauracja zajmuje się przygotowaniem posiłków kuchni polskiej. Dania można
                zamawiać telefonicznie, jak i zjeść na miejscu w bardzo klimatycznym lokalu.</p>
        </asp:Panel>
        <asp:Panel class="PanelTotoPromocje" runat="server" ScrollBars="Auto">
            <asp:Panel class="PanelToto" runat="server" ScrollBars="Auto">
            </asp:Panel>
            <asp:Panel class="PanelPromocje" runat="server" ScrollBars="Auto">
            </asp:Panel>
        </asp:Panel>
    </div>
</asp:Content>
