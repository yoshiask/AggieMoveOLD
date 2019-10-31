using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using MTATransit.Shared.API.OTP;
using MTATransit.Shared.API.RestBus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MTATransit.Shared.Controls
{
    public sealed partial class ItineraryCard : UserControl
    {
        public Models.ItineraryModel Itin {
            get {
                return this.DataContext as Models.ItineraryModel;
            }
        }
        public Border icon = Models.Glyphs.TransitIcon.DefaultTransitIconsOTP["WALK"].GetIcon(true);

        public ItineraryCard()
        {
            this.InitializeComponent();
            this.Loaded += ItineraryCard_Loaded;
        }

        private void ItineraryCard_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMap();
            LegsStack.Children.Clear();

            for (int i = 0; i < Itin.Legs.Count; i++)
            {
                var legStack = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Padding = new Thickness(0, 0, 5, 0)
                };

                var l = Itin.Legs[i];
                legStack.Children.Add(
                    Models.Glyphs.TransitIcon.DefaultTransitIconsOTP[l.Mode].GetIcon(true)
                );
                legStack.Children.Add(new TextBlock()
                {
                    Text = l.ToShortDisplayString(),
                    FontFamily = Common.DINFont,
                    FontSize = 18,
                    Padding = new Thickness(5, 2, 0, 2)
                });

                if (i != Itin.Legs.Count - 1)
                    legStack.Children.Add(new TextBlock()
                    {
                        Text = ">",
                        FontFamily = Common.DINFont,
                        FontWeight = Windows.UI.Text.FontWeights.Bold,
                        FontSize = 18,
                        Padding = new Thickness(5, 2, 0, 2)
                    });
                LegsStack.Children.Add(legStack);
            }
        }

        public async void LoadMap()
        {
            MainMapView.Map = new Map(
                BasemapType.ImageryWithLabels, 0, 0, 1
            );
            //MainMapView.IsHitTestVisible = false;

            List<MapPoint> Points = new List<MapPoint>();
            foreach (Leg leg in Itin.Legs)
            {
                var geometry = GooglePolylineConverter.Decode(leg.Geometry.Points);
                List<MapPoint> legPoints = new List<MapPoint>();
                foreach (API.ArcGIS.Location location in geometry)
                {
                    MapPoint point = new MapPoint(location.Longitude, location.Latitude);
                    legPoints.Add(point);
                    Points.Add(point);
                }

                //  use a polyline builder to create the new polyline from a collection of points
                var legPath = new PolylineBuilder(legPoints, SpatialReferences.Wgs84).ToGeometry();
                // create a simple line symbol to display the polyline
                var legLineSymbol = new SimpleLineSymbol(
                    SimpleLineSymbolStyle.Solid,
                    Common.ConvertColor(Common.ColorFromHex(Models.Glyphs.TransitIcon.DefaultTransitIconsOTP[leg.Mode].DefaultBackColor)),
                    4.0
                );
                MapGraphics.Graphics.Add(new Graphic(legPath, legLineSymbol));
            }

            //  use a polyline builder to create the new polyline from a collection of points
            Polyline Path = new PolylineBuilder(Points, SpatialReferences.Wgs84).ToGeometry();
            await MainMapView.SetViewpointGeometryAsync(Path, 20);

            // Have to add points after adding the path, 
            // otherwise the points will show underneath the line
            foreach (Leg leg in Itin.Legs)
            {
                MapGraphics.Graphics.Add(CreateRouteStop(
                    Convert.ToDecimal(Points.First().Y), Convert.ToDecimal(Points.First().X),
                    System.Drawing.Color.DarkRed
                ));
                MapGraphics.Graphics.Add(CreateRouteStop(
                    Convert.ToDecimal(Points.Last().Y), Convert.ToDecimal(Points.Last().X),
                    System.Drawing.Color.DarkRed
                ));
            }
        }

        private Graphic CreateRouteStop(decimal lat, decimal lon, System.Drawing.Color fill)
        {
            // Now draw a point where the stop is
            var stopPoint = new MapPoint(Convert.ToDouble(lon),
                Convert.ToDouble(lat), SpatialReferences.Wgs84);
            var pointSymbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, fill, 20);
            pointSymbol.Outline = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.White, 5);
            return new Graphic(stopPoint, pointSymbol);
        }
        private Graphic CreateInactiveRouteStop(decimal lat, decimal lon)
        {
            return CreateRouteStop(lat, lon, System.Drawing.Color.Black);
        }

        private async void MapView_Tapped(object sender, GeoViewInputEventArgs e)
        {
            //MapPoint tappedPoint = (MapPoint)GeometryEngine.Project(e.Location, SpatialReferences.Wgs84);

            var resultGraphics = await MainMapView.IdentifyGraphicsOverlayAsync(MapGraphics, e.Position, 10, false);
        }
    }
}
