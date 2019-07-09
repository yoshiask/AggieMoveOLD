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
            var route = ((object[])e.Parameter)[1] as Route;
            var stop = ((object[])e.Parameter)[2] as Stop;

            LoadStopInfo(agency, route, stop);
            LoadMap(stop.Lat, stop.Lon);
            base.OnNavigatedTo(e);
        }

        public async void LoadStopInfo(Agency agency, Route route, Stop stop)
        {
            var api = Common.NextBusApi;
            var predictions = (await api.GetRoutePredictionsByStopId(agency.Tag, stop.StopId.ToString())).ToList(); //await api.GetStopPredictions(agency.Tag, route.Tag, stop.StopId);
            var routeInfo = await api.GetRouteConfig(agency.Tag, route.Tag);

            MainGrid.Background = Common.BrushFromHex(routeInfo.Color);
            PageHeader.Foreground = Common.BrushFromHex(routeInfo.OppositeColor);

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

            var parkrideUri = new Uri("https://public.gis.lacounty.gov/public/rest/services/LACounty_Dynamic/LMS_Data_Public/MapServer/187");
            var parkrideLayer = new FeatureLayer(new ServiceFeatureTable(parkrideUri));
            MainMapView.Map.OperationalLayers.Add(parkrideLayer);
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = (NavigationViewItem)args.SelectedItem;
            var name = item.Content as string;

            if (name == null || name == "Settings")
                return;

            Type newPage = Common.Pages[item.Content as string].Item1;

            if (GetType() == newPage)
                return;

            if (newPage.DeclaringType == typeof(Page))
                Frame.Navigate(newPage);
        }
    }
}
