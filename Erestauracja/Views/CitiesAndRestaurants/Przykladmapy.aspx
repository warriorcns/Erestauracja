<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Erestauracja.ServiceReference.Town>>" %>
<%@ Import Namespace="System.Drawing" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Przykladmapy
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% 
    
    
             Html.Telerik().GoogleMap().Width(500).Height(400)
                .Name("map").Latitude(50).Longitude(18).BindTo<Erestauracja.ServiceReference.Town, Jmelosegui.Mvc.Controls.Overlays.Marker>
                //( (System.Collections.Generic.IEnumerable<Erestauracja.Controllers.RegionInfo>)ViewData["markers"], mappings => mappings.For<Erestauracja.Controllers.RegionInfo>
                  (Model, m => m.For<Erestauracja.ServiceReference.Town>
                (
                            binding => binding.ItemDataBound
                            (
                                (marker, obj) =>
                                {
                                    marker.Latitude = (double)obj.Latitude;
                                    marker.Longitude = (double)obj.Longtitude;
                                    marker.Title = obj.TownName;
                                    marker.zIndex = obj.ID;
                                    //marker.Icon = new Jmelosegui.Mvc.Controls.Overlays.MarkerImage("/map/Images/Banderitas/{0}"
                                    //                                , new Size(18, 12)
                                    //                                , new Point(0, 0)
                                    //                                , new Point(0, 12));
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
                ).Render();%>

            <%--Renderuje mapke oraz dzialaja inne jQery skrypty--%> 
            <% Html.Telerik().ScriptRegistrar().jQuery(false).jQueryValidation(false).OnDocumentReady("$('#mapTowns').dialog();").Render(); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
