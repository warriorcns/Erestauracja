<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Erestauracja.Models.MapModel>>" %>

    <%  Html.Telerik().GoogleMap().Width(500).Height(400)
                .Name("mapTowns").Latitude(50).Longitude(18).BindTo<Erestauracja.Models.MapModel, Jmelosegui.Mvc.Controls.Overlays.Marker>
                //( (System.Collections.Generic.IEnumerable<Erestauracja.Controllers.RegionInfo>)ViewData["markers"], mappings => mappings.For<Erestauracja.Controllers.RegionInfo>
                  (Model, m => m.For<Erestauracja.Models.MapModel>
                (
                            binding => binding.ItemDataBound
                            (
                                (marker, obj) =>
                                {
                                    marker.Latitude = (double)obj.Latitude;
                                    marker.Longitude = (double)obj.Longitude;
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

