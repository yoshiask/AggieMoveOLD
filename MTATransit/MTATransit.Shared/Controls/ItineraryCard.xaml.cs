using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using MTATransit.Shared.API.OTPMTA;
using MTATransit.Shared.API.RestBus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation.Collections;
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

        public ItineraryCard()
        {
            this.InitializeComponent();
            this.Loaded += ItineraryCard_Loaded;
        }

        private void ItineraryCard_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMap();

            // TODO: Find a way to add the arrows using Binding
            for (int i = 0; i < Itin.Legs.Count; i++)
            {
                string text = "";

                var l = Itin.Legs[i];
                text += l.ToLegString();
                if (i != Itin.Legs.Count - 1)
                    text += " > ";

                LegText.Text += text;
            }
        }

        public async void LoadMap()
        {
            MainMapView.Map = new Map(
                BasemapType.ImageryWithLabels, 0, 0, 1
            );
            MainMapView.LocationDisplay.IsEnabled = true;
            MainMapView.LocationDisplay.ShowLocation = true;
            //MainMapView.IsHitTestVisible = false;

            List<MapPoint> Points = new List<MapPoint>();
            foreach (Leg leg in Itin.Legs)
            {
                var geometry = GooglePolylineConverter.Decode(leg.Geometry.Points);
                foreach (API.ArcGIS.Location location in geometry)
                {
                    Points.Add(new MapPoint(location.Longitude, location.Latitude));
                }
            }

            //  use a polyline builder to create the new polyline from a collection of points
            Polyline Path = new PolylineBuilder(Points, SpatialReferences.Wgs84).ToGeometry();
            System.Diagnostics.Debug.WriteLine(Path.Extent);
            // create a simple line symbol to display the polyline
            var lineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.FromArgb(255, 11, 95, 96), 4.0);

            MapGraphics.Graphics.Add(new Graphic(Path, lineSymbol));
            await MainMapView.SetViewpointGeometryAsync(Path, 20);

            // Have to add points after the path, otherwise the points will show underneath the line
            foreach (Leg leg in Itin.Legs)
            {
                MapGraphics.Graphics.Add(CreateRouteStop(
                    Convert.ToDecimal(Points[0].Y), Convert.ToDecimal(Points[0].X),
                    System.Drawing.Color.DarkRed
                ));
                MapGraphics.Graphics.Add(CreateRouteStop(
                    Convert.ToDecimal(Points[Points.Count - 1].Y), Convert.ToDecimal(Points[Points.Count - 1].X),
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
