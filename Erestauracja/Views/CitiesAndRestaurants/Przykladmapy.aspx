<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Przykladmapy
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% 
             Html.Telerik().GoogleMap()
                .Name("map").Latitude(40).Longitude(-3).BindTo<Erestauracja.ServiceReference.Town, Jmelosegui.Mvc.Controls.Overlays.Marker>
                ((System.Collections.Generic.IEnumerable<Erestauracja.ServiceReference.Town>)ViewData["markers"], mappings => mappings.For<Erestauracja.ServiceReference.Town>
                        (
                            binding => binding.ItemDataBound
                            (
                                (marker, obj) =>
                                {
                                    //marker = new Jmelosegui.Mvc.Controls.Overlays.Marker();
                                    //obj = new Erestauracja.ServiceReference.Town();
                                    marker.Latitude = (double)obj.Latitude;
                                    marker.Longitude = (double)obj.Longtitude;
                                }
                            )
                        )
                );%>

            <%--Renderuje mapke oraz dzialaja inne jQery skrypty--%> 
            <% Html.Telerik().ScriptRegistrar().jQuery(false).jQueryValidation(false).OnDocumentReady("$('#mapTowns').dialog();").Render(); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
