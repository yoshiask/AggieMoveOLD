using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using System.Linq;
using NextBus.NET.Models;
/*using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.UI.Controls;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;*/
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MTATransit.Shared.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RouteDetailsView : Page
    {
        public RouteDetailsView()
        {
            this.InitializeComponent();

            foreach (Tuple<Type, NavigationViewItem> info in Common.Pages.Values)
            {
                var menuItem = new NavigationViewItem
                {
                    Icon = info.Item2.Icon,
                    Content = info.Item2.Content,
                    Tag = info.Item2.Tag
                };

                NavView.MenuItems.Add(menuItem);

                // If the menu item we're adding goes to this page, then select it
                if (info.Item1 == GetType())
                    NavView.SelectedItem = menuItem;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var agency = ((object[])e.Parameter)[0] as Agency;
            var route = ((object[])e.Parameter)[1] as RouteConfig;
            var stop = ((object[])e.Parameter)[2] as Stop;

            LoadStopInfo(agency, route, stop);
            //LoadMap(stop, route);
            base.OnNavigatedTo(e);
        }

        public async void LoadStopInfo(Agency agency, RouteConfig route, Stop stop)
        {
            var api = Common.NextBusApi;
            var predictions = (await api.GetRoutePredictionsByStopTag(agency.Tag, stop.Tag, route.Tag)).ToList();
            var routeInfo = await api.GetRouteConfig(agency.Tag, route.Tag);

            MainGrid.Background = Common.BrushFromHex(routeInfo.Color);
            PageHeader.Foreground = Common.BrushFromHex(routeInfo.OppositeColor);
            var itemTheme = Common.ThemeFromColor(routeInfo.OppositeColor);

            var pred = predictions[0];
            PageHeader.Text = pred.StopTitle;
            foreach (RouteDirection dir in pred.Directions)
            {
                foreach (Prediction pr in dir.Predictions)
                {
                    int secs = pr.Seconds;
                    string display = Math.Round(Convert.ToDouble(pr.Seconds) / 60, 0).ToString();

                    PredictionBox.Items.Add(new ListViewItem()
                    {
                        Content = display,
                        Foreground = Common.BrushFromHex(routeInfo.OppositeColor),
                        RequestedTheme = itemTheme,
                    });
                }
            }
        }

        /*public void LoadMap(Stop stop, RouteConfig route)
        {
            MainMapView.Map = new Map(
                BasemapType.ImageryWithLabelsVector,
                Convert.ToDouble(stop.Lat),
                Convert.ToDouble(stop.Lon),
                19
            );
            MainMapView.LocationDisplay.IsEnabled = true;
            MainMapView.LocationDisplay.ShowLocation = true;

            // Now draw a point where the stop is
            //var stopPoint = CreateRouteStop(stop.Lat, stop.Lon, System.Drawing.Color.Red);
            //MapGraphics.Graphics.Add(stopPoint);

            // Display the selected route & stop
            //DrawRoutePath(route, stop, MapGraphics, true);

            // Display all of the Park & Ride Locations
            var parkrideUri = new Uri("https://public.gis.lacounty.gov/public/rest/services/LACounty_Dynamic/LMS_Data_Public/MapServer/187");
            var parkrideLayer = new FeatureLayer(new ServiceFeatureTable(parkrideUri));
            //MainMapView.Map.OperationalLayers.Add(parkrideLayer);
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

        private void DrawRoutePath(RouteConfig rtc, Stop selectedStop, GraphicsOverlay overlay, bool showStops)
        {
            //Create polyline geometry
            var polylinePoints = new PointCollection(SpatialReferences.Wgs84);
            foreach (Path path in rtc.Paths)
            {
                foreach (Point stop in path.Points)
                {
                    polylinePoints.Add(new MapPoint(Convert.ToDouble(stop.Lon), Convert.ToDouble(stop.Lat)));
                }
            }
                
            var polyline = new Polyline(polylinePoints);

            //Create symbol for polyline
            var polylineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, Common.DrawingColorFromHex(rtc.Color), 3);

            //Create a polyline graphic with geometry and symbol
            overlay.Graphics.Add(new Graphic(polyline, polylineSymbol));

            if (showStops)
                // Draw a point for each stop
                foreach (Stop stop in rtc.Stops)
                {
                    Graphic graphic;
                    if (selectedStop.Lat == stop.Lat && selectedStop.Lon == stop.Lon)
                        graphic = CreateRouteStop(stop.Lat, stop.Lon, System.Drawing.Color.Red);
                    else
                        graphic = CreateInactiveRouteStop(stop.Lat, stop.Lon);
                    overlay.Graphics.Add(graphic);
                }
        }

        private async void MapView_Tapped(object sender, GeoViewInputEventArgs e)
        {
            //MapPoint tappedPoint = (MapPoint)GeometryEngine.Project(e.Location, SpatialReferences.Wgs84);

            var resultGraphics = await MainMapView.IdentifyGraphicsOverlayAsync(MapGraphics, e.Position, 10, false);
        }*/

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Common.NavView_SelectionChanged(this, sender, args);
        }
    }
}
