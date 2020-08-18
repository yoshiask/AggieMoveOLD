using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using Esri.ArcGISRuntime.Mapping;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
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
            
            if (Point.IsCurrentLocation)
            {
                Point.Geolocator.PositionChanged += Geolocator_PositionChanged;
            }
            else
            {
                try
                {
                    Point.Geolocator.PositionChanged -= Geolocator_PositionChanged;
                }
                catch (ArgumentException)
                {
                    // Do nothing. This exception just means that the method
                    // was never attached because it wasn't previously set to
                    // use the current location.
                }
            }
        }

        private async void Geolocator_PositionChanged(Windows.Devices.Geolocation.Geolocator sender, Windows.Devices.Geolocation.PositionChangedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                MapGraphics.Graphics.Clear();

                var stopPoint = CreateRouteStop(
                    Convert.ToDecimal(args.Position.Coordinate.Point.Position.Latitude),
                    Convert.ToDecimal(args.Position.Coordinate.Point.Position.Longitude),
                    System.Drawing.Color.Red
                );
                MapGraphics.Graphics.Add(stopPoint);
            });            
        }

        public Graphic LoadMap(double lat, double lon)
        {
            MainMapView.Map = new Map(
                BasemapType.ImageryWithLabels,
                lat,
                lon,
                12
            );

            // Now draw a point where the stop is
            var stopPoint = CreateRouteStop(Convert.ToDecimal(lat), Convert.ToDecimal(lon), System.Drawing.Color.Red);
            MapGraphics.Graphics.Add(stopPoint);

            // Display buildings
            var buildingsSmUri = new Uri("https://gis.tamu.edu/arcgis/rest/services/FCOR/TAMU_BaseMap/MapServer/2");
            var buildingsSmLayer = new FeatureLayer(new ServiceFeatureTable(buildingsSmUri));
            //MainMapView.Map.OperationalLayers.Add(buildingsSmLayer);
            var buildingsMdUri = new Uri("https://gis.tamu.edu/arcgis/rest/services/FCOR/TAMU_BaseMap/MapServer/3");
            var buildingsMdLayer = new FeatureLayer(new ServiceFeatureTable(buildingsMdUri));
            //MainMapView.Map.OperationalLayers.Add(buildingsMdLayer);

            return stopPoint;
        }

        private Graphic CreateRouteStop(decimal lat, decimal lon, System.Drawing.Color fill)
        {
            // Now draw a point where the stop is
            var mapPoint = new MapPoint(Convert.ToDouble(lon),
                Convert.ToDouble(lat), SpatialReferences.Wgs84);
            var pointSymbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, fill, 20);
            pointSymbol.Outline = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.White, 5);
            return new Graphic(mapPoint, pointSymbol);
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

        public delegate void EditRequestedHandler(Models.PointModel p);
        public event EditRequestedHandler EditRequested;
        private void EditButton_Click(object sender, RoutedEventArgs args)
        {
            EditRequested?.Invoke(Point);
        }

        public delegate void DeleteRequestedHandler(Models.PointModel p);
        public event DeleteRequestedHandler DeleteRequested;
        private void DeleteButton_Click(object sender, RoutedEventArgs args)
        {
            DeleteRequested?.Invoke(Point);
        }

        private void ShowFlyout(object sender, RoutedEventArgs args)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
    }
}
