using MTATransit.Shared;
using MTATransit.Shared.API;
using MTATransit.Shared.Pages;
using MTATransit.Shared.API.RestBus;
using Refit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MTATransit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Agency curAgency;
        Route curRoute;
        List<Agency> Agencies = new List<Agency>();
        List<Route> Routes = new List<Route>();
        List<Stop> Stops = new List<Stop>();

        public MainPage()
        {
            this.InitializeComponent();
            LoadAgencies();

            Common.LoadNavView(this, NavView);
        }

        public async void LoadAgencies()
        {
            try
            {
                /*var vid = await Shared.API.LAMove.LAMoveHelper.GetVIDFromBus();
                var vehicles = await Common.RestBusApi.GetAgencyVehicles("lametro");
                var vehicle = vehicles.Find(v => v.Id == vid);
                if (vehicle != null)
                {
                    var dialog = new Shared.Controls.DialogBox("LA Move", $"You are currently on route {vehicle.RouteId}, vehicle #{vid}");
                    dialog.OnDialogClosed += (Shared.Controls.DialogBox.DialogResult result) =>
                    {
                        MainGrid.Children.Remove(dialog);
                    };
                    MainGrid.Children.Add(dialog);
                }*/
            }
            catch
            {
                // TODO: Prompt the user for vehicle id.
                // Display the pictogram showing where the ID can be found inside the bus.
            }

            // Get a list of the agencies this api serves
            try
            {
                SetLoadingBar(true);
                Agencies = await Common.RestBusApi.GetAgencies();
                if (Agencies == null)
                    return;

                foreach (Agency ag in Agencies)
                {
                    AgenciesBox.Items.Add(new ComboBoxItem()
                    {
                        Content = ag.Title,
                    });
                }
                SetLoadingBar(false);
            }
            catch
            {
                // Was unable to load agencies
            }
        }

        public async void LoadRoutes(Agency ag)
        {
            SetLoadingBar(true);
            var api = Common.RestBusApi;
            RoutesBox.Items.Clear();
            Routes.Clear();

            // Now load the available routes
            Routes = await api.GetAgencyRoutes(ag.Id);
            if (Routes == null)
                return;

            try
            {
                foreach (Route rt in Routes)
                {
                    RoutesBox.Items.Add(new ComboBoxItem()
                    {
                        Name = rt.Id,
                        Content = rt.Title,
                    });
                }

                // Now get the routeConfig for colors
                for (int i = 0; i < RoutesBox.Items.Count; ++i)
                {
                    var item = RoutesBox.Items[i] as ComboBoxItem;

                    var info = await api.GetRoute(ag.Id, item.Name);
                    if (info != null)
                    {
                        int ind = Routes.FindIndex(r => r.Id == info.Id);
                        if (ind < 0)
                            ind = i;
                        Routes[ind] = info;

                        item.Background = Common.BrushFromHex(info.Color);
                        item.Foreground = Common.BrushFromHex(info.TextColor);
                        item.RequestedTheme = ElementTheme.Light;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            SetLoadingBar(false);
        }

        public async void LoadStops(Route route)
        {
            SetLoadingBar(true);
            var api = Common.RestBusApi;
            int i = Routes.IndexOf(route);

            if (curRoute.Color == null)
                curRoute = await api.GetRoute(curAgency.Id, route.Id);
            else
                curRoute = Routes[i];

            StopsBox.Background = Common.BrushFromHex(curRoute.Color);
            StopsBox.Items.Clear();
            Stops.Clear();

            foreach (Stop st in curRoute.Stops)
            {
                Stops.Add(st);
                StopsBox.Items.Add(new ListViewItem()
                {
                    Name = st.Id,
                    Content = st.Title,
                    Foreground = Common.BrushFromHex(curRoute.TextColor),
                    RequestedTheme = Common.ThemeFromColor(curRoute.TextColor),
                });
            }
            SetLoadingBar(false);
        }

        private async void AgenciesBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (Agencies.Count > 0)
            {
                curAgency = await NextBusApiHelper.GetAgencyByTitle(((ComboBoxItem)AgenciesBox.SelectedItem).Content.ToString(), Agencies);
                StopsBox.Items.Clear();
                LoadRoutes(curAgency);
                RoutesBox.IsEnabled = true;
            }
        }

        private async void RoutesBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (RoutesBox.Items.Count > 0)
            {
                curRoute = await NextBusApiHelper.GetRouteByTitle(curAgency.Id, ((ComboBoxItem)RoutesBox.SelectedItem).Content.ToString(), Routes);
                LoadStops(curRoute);
                StopsBox.ScrollIntoView(StopsBox.Items[0]);
            }
        }

        private void StopsBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            var ag = curAgency;
            var rt = curRoute;

            object[] pars =
            {
                ag, rt, Stops[StopsBox.SelectedIndex]
            };
            Frame.Navigate(typeof(RouteDetailsView), pars);
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Common.NavView_SelectionChanged(this, sender, args);
        }

        private void SetLoadingBar(bool loading)
        {
            PageLoadingBar.Visibility = loading ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
        }
    }
}
