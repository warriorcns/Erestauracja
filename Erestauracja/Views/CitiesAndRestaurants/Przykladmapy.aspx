﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Przykladmapy
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">



<%:
        Html.Telerik().GoogleMap()
            .Name("map")
            .Width(300)
            .Height(300)
                .Latitude(54.093429)
                .Longitude(18.777669)
            .Markers(m => m.Add().Title("Hello World!"))
    %>

    


    <% Html.Telerik().ScriptRegistrar().Render();%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
