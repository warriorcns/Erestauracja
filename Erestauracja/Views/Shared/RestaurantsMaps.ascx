<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Erestauracja.ServiceReference.RestaurantInCity>>" %>

<%@ Import Namespace="Erestauracja.ServiceReference" %>
<%@ Import Namespace="Erestauracja.Models" %>
<link href="../../Content/Site.css" rel="stylesheet" type="text/css" />

<div>
    <%  Html.Telerik().GoogleMap().Name("map")
            .Width(450).Height(450)
                .Latitude(52.281602).Longitude(19.15686).BindTo<Erestauracja.ServiceReference.RestaurantInCity, Jmelosegui.Mvc.Controls.Overlays.Marker>
                //( (System.Collections.Generic.IEnumerable<Erestauracja.Controllers.RegionInfo>)ViewData["markers"], mappings => mappings.For<Erestauracja.Controllers.RegionInfo>
                  (Model, m => m.For<Erestauracja.ServiceReference.RestaurantInCity>
                (
                            binding => binding.ItemDataBound
                            (
                                (marker, obj) =>
                                {
                                    marker.Latitude = (double)obj.Latitude;
                                    marker.Longitude = (double)obj.Longtitude;
                                    marker.Title = obj.DisplayName;
                                    //marker.zIndex = obj.ID;
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

            

</div>

