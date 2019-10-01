using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using MTATransit.Shared.API.RestBus;
using Esri.ArcGISRuntime.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Esri.ArcGISRuntime.Data;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MTATransit.Shared.Controls
{
    public sealed partial class NavigationPointCard : UserControl
    {
        public Models.PointModel Point {
            get {
                return this.DataContext as Models.PointModel;
            }
        }

        public NavigationPointCard()
        {
            this.InitializeComponent();

            //DataContextChanged += (s, e) => Bindings.Update();
        }

        private void Card_Loading(DependencyObject sender, object args)
        {
            LoadMap(Convert.ToDouble(Point.Latitude), Convert.ToDouble(Point.Longitude));
        }

        public void LoadMap(double lat, double lon)
        {
            MainMapView.Map = new Map(
                BasemapType.ImageryWithLabelsVector,
                lon,
                lat,
                12
            );
            MainMapView.LocationDisplay.IsEnabled = true;
            MainMapView.LocationDisplay.ShowLocation = true;

            // Now draw a point where the stop is
            var stopPoint = CreateRouteStop(Convert.ToDecimal(lat), Convert.ToDecimal(lon), System.Drawing.Color.Red);
            MapGraphics.Graphics.Add(stopPoint);

            // Display all of the Park & Ride Locations
            var parkrideUri = new Uri("https://public.gis.lacounty.gov/public/rest/services/LACounty_Dynamic/LMS_Data_Public/MapServer/187");
            var parkrideLayer = new FeatureLayer(new ServiceFeatureTable(parkrideUri));
            //MainMapView.Map.OperationalLayers.Add(parkrideLayer);
        }

        private Graphic CreateRouteStop(decimal lat, decimal lon, System.Drawing.Color fill)
        {
            // Now draw a point where the stop is
            var stopPoint = new MapPoint(Convert.ToDouble(lat),
                Convert.ToDouble(lon), SpatialReferences.Wgs84);
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
