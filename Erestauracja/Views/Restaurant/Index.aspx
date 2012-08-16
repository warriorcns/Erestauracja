<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content class="main" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
    
        <nav class="menucontainer">
        <ul class="menu">
            <li>
                <%: Html.ActionLink("Opis", "Index", "Restaurant")%></li>
            <li>
                <%: Html.ActionLink("Menu", "Menu", "Restaurant")%></li>
            <li>
                <%: Html.ActionLink("Dowóz", "Delivery", "Restaurant")%></li>
            <li>
                <%: Html.ActionLink("Imprezy okolicznościowe", "Parties", "Restaurant")%></li>
            <li>
                <%: Html.ActionLink("Galeria", "Gallery", "Restaurant")%></li>
            <li>
                <%: Html.ActionLink("Kontakt", "Contact", "Restaurant")%></li>
        </ul>
    </nav>
    
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
