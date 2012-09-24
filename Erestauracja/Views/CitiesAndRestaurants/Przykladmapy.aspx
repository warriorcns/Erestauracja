<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Erestauracja.Controllers.RegionInfo>>" %>
<%@ Import Namespace="System.Drawing" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Przykladmapy
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% 
    
    
             Html.Telerik().GoogleMap()
                .Name("map").Latitude(40).Longitude(-3).BindTo<Erestauracja.Controllers.RegionInfo, Jmelosegui.Mvc.Controls.Overlays.Marker>
                ( (System.Collections.Generic.IEnumerable<Erestauracja.Controllers.RegionInfo>)ViewData["markers"], mappings => mappings.For<Erestauracja.Controllers.RegionInfo>
                        (
                            binding => binding.ItemDataBound
                            (
                                (marker, obj) =>
                                {
                                    marker.Latitude = (double)obj.Latitude;
                                    marker.Longitude = (double)obj.Longitude;
                                    marker.Title = obj.Title;
                                    marker.zIndex = obj.zIndex;
                                    marker.Icon = new Jmelosegui.Mvc.Controls.Overlays.MarkerImage("/map/Images/Banderitas/{0}"
                                                                    , new Size(18, 12)
                                                                    , new Point(0, 0)
                                                                    , new Point(0, 12));
                                    marker.Window = new Jmelosegui.Mvc.Controls.Overlays.InfoWindow(marker)
                                    {
                                        Template =
                                        {
                                            Content = () => Writer.Write(obj.InfoWindowContent)
                                        }
                                    };
                                }
                            )
                        )
                );%>

            <%--Renderuje mapke oraz dzialaja inne jQery skrypty--%> 
            <% Html.Telerik().ScriptRegistrar().jQuery(false).jQueryValidation(false).OnDocumentReady("$('#mapTowns').dialog();").Render(); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
