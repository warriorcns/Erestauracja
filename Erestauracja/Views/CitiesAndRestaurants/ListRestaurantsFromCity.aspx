<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="Jmelosegui.Mvc.Controls" %>
<%@ Import Namespace="Telerik.Web.Mvc.UI"%>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.Telerik().ScriptRegistrar().Render();%>
    <asp:Label class="LabelNazwaMiasta" runat="server" 
        Text="Nazwa wybranego miasta" CssClass="LabelNazwaMiasta"></asp:Label>

    <div class="RestauracjeMapa">
    <asp:Panel class="PanelListaRestauracji" runat="server" ScrollBars="Auto">

    </asp:Panel>
    <asp:Panel class="PanelMapa" runat="server">
    <%--Dac tutaj mape z google maps wraz z znacznikami--%>
    
    
    <%: Html.Telerik().GoogleMap().Name("map")%>
    </asp:Panel>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
