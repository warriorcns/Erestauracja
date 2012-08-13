<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="title" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="main" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>
        <%: ViewBag.Message %></h2>
    <form id="form1" runat="server">
    <br />
    <asp:Panel class="lewypanel" runat="server" Height="600" Width="200" ScrollBars="Auto" Wrap="true">
        <asp:DropDownList class="KategorieDropDownList" runat="server" Width="200">
        </asp:DropDownList>
        <asp:BulletedList class="RestauracjeBulletedList" runat="server" BulletStyle="CustomImage" BulletImageUrl="">
            <asp:ListItem>Restauracja 1</asp:ListItem>
            <asp:ListItem>Restauracja 2</asp:ListItem>
            <asp:ListItem>Restauracja 3</asp:ListItem>
            <asp:ListItem>Restauracja 4</asp:ListItem>
            <asp:ListItem>Restauracja 5</asp:ListItem>
        </asp:BulletedList>
    </asp:Panel>
    

    
    </form>
</asp:Content>
