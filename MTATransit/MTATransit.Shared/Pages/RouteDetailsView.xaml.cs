using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using System.Linq;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Animation;
using NextBus.NET;
using NextBus.NET.Models;
using Esri.ArcGISRuntime.Portal;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.UI.Controls;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MTATransit.Shared.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RouteDetailsView : Page
    {
        MapView MainMapView = new MapView();

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
            var route = ((object[])e.Parameter)[1] as Route;
            var stop = ((object[])e.Parameter)[2] as Stop;

            LoadStopInfo(agency, route, stop);
            LoadMap(stop.Lat, stop.Lon);
            base.OnNavigatedTo(e);
        }

        public async void LoadStopInfo(Agency agency, Route route, Stop stop)
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

        public void LoadMap(decimal lat, decimal lon)
        {
            MainMapView.Map = new Map(
                BasemapType.ImageryWithLabelsVector,
                Convert.ToDouble(lat),
                Convert.ToDouble(lon),
                19
            );
            MainMapView.LocationDisplay.IsEnabled = true;
            MainMapView.LocationDisplay.ShowLocation = true;

            // Create a new graphics layer
            var MapGraphics = new GraphicsOverlay();

            // Now draw a point where the stop is
            var stopPoint = new MapPoint(Convert.ToDouble(lat), Convert.ToDouble(lon));
            var pointSymbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, System.Drawing.Color.Black, 20);
            pointSymbol.Outline = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.White, 5);
            var stopGraphic = new Graphic(stopPoint, pointSymbol);
            MapGraphics.Graphics.Add(stopGraphic);

            // Display all of the Park & Ride Locations
            var parkrideUri = new Uri("https://public.gis.lacounty.gov/public/rest/services/LACounty_Dynamic/LMS_Data_Public/MapServer/187");
            var parkrideLayer = new FeatureLayer(new ServiceFeatureTable(parkrideUri));
            MainMapView.Map.OperationalLayers.Add(parkrideLayer);

            // Now display the map
            MainMapView.GraphicsOverlays.Add(MapGraphics);
            MainGrid.Children.Add(MainMapView);
            Windows.UI.Xaml.Controls.Grid.SetRow(MainMapView, 2);
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Common.NavView_SelectionChanged(this, sender, args);
        }
    }
}
