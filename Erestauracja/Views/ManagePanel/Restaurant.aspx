<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Restaurant
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Wybierz restaurację, którą chcesz zarządzać lub kliknij <%: Html.ActionLink("tutaj", "AddRestaurant", "ManagePanel")%>, aby dodać nową. </h2>

    <asp:Panel class="Restauracje" ID="Restauracje" runat="server">
        <asp:ListView ID="ListView1" runat="server">
            <LayoutTemplate>
                <div style="border: solid 2px #336699; width: 20%;">
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <div style="border: solid 1px #336699;">
                    <%# Eval("ProductName")%>
                </div>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <div style="border: solid 1px #336699; background-color: #dadada;">
                    <%# Eval("ProductName")%>
                </div>
            </AlternatingItemTemplate>
            <EmptyDataTemplate>
                No records found
            </EmptyDataTemplate>
        </asp:ListView>
    </asp:Panel>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
