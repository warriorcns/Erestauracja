<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="title" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content class="main" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>
        <%: ViewBag.Message %></h2>
    <form id="form1" runat="server">
    <br />
    <div class="all">
    <asp:Panel class="lewypanel" runat="server" ScrollBars="Auto" Wrap="true">
        <asp:DropDownList id="DropDownList1" name="DropDownList1" class="DropDownListKategorie" runat="server" >
        </asp:DropDownList>
        <asp:BulletedList class="BulletedListRestauracje" runat="server" BulletStyle="CustomImage" BulletImageUrl="">
            <asp:ListItem>Restauracja 1</asp:ListItem>
            <asp:ListItem>Restauracja 2</asp:ListItem>
            <asp:ListItem>Restauracja 3</asp:ListItem>
            <asp:ListItem>Restauracja 4</asp:ListItem>
            <asp:ListItem>Restauracja 5</asp:ListItem>
        </asp:BulletedList>
    </asp:Panel>

    <asp:Panel class="PanelInfo" runat="server" ScrollBars="Auto" Wrap="true">
        <p>Info czym strona sie zajmuje.</p>
    </asp:Panel>

        <asp:Panel class="PanelWybor" runat="server" ScrollBars="Auto" Wrap="true">
            <asp:Label class="LabelWybierzMiasto" runat="server" Text="Wybierz miasto:"></asp:Label>
            
            <%=Html.DropDownList("Miasta", ViewData["Miasta"] as SelectList, new { @class = "DropDownListWybierzMiasto" })%>
            
            <asp:Button ID="ButtonWybierzMiasto" class="ButtonWybierzMiasto" runat="server" Text="Szukaj" Font-Size="Small"/>

            
            
            
            <asp:Label class="LabelWybierzRestauracje" runat="server" Text="Wybierz restauracje:"></asp:Label>
            <%=Html.DropDownList("Restauracje", ViewData["Restauracje"] as SelectList, new { @class = "DropDownListWybierzRestauracje" })%>
            <asp:Button ID="ButtonWybierzRestauracje" class="ButtonWybierzRestauracje" runat="server" Text="Szczegółowe wyszukiwanie" Font-Size="Small"/>
            


        </asp:Panel>
    </div>

    
    </form>
</asp:Content>
