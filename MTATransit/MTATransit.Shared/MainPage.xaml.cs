using MTATransit.Shared;
using MTATransit.Shared.API;
using MTATransit.Shared.Pages;
using NextBus.NET.Models;
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
        RouteConfig curRouteConfig;
        List<Agency> Agencies = new List<Agency>();
        List<Route> Routes = new List<Route>();
        List<RouteConfig> RouteConfigs = new List<RouteConfig>();
        List<Stop> Stops = new List<Stop>();

        public MainPage()
        {
            this.InitializeComponent();
            Load();

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

        public async void Load()
        {
            // Get a list of the agencies this api serves
            Agencies = (await Common.NextBusApi.GetAgencies()).ToList();
            foreach (Agency ag in Agencies)
            {
                AgenciesBox.Items.Add(new ComboBoxItem()
                {
                    Content = ag.Title,
                });
            }
        }

        public async void LoadRoutes(Agency ag)
        {
            var api = Common.NextBusApi;
            RoutesBox.Items.Clear();

            // Now load the available routes
            Routes = (await api.GetRoutesForAgency(ag.Tag)).ToList();
            foreach (Route rt in Routes)
            {
                RoutesBox.Items.Add(new ComboBoxItem()
                {
                    Name = rt.Tag,
                    Content = rt.Title,
                });
            }

            RouteConfigs.Clear();
            // Now get the routeConfig for colors
            for (int i = 0; i < RoutesBox.Items.Count; ++i)
            {
                var item = RoutesBox.Items[i] as ComboBoxItem;
                //System.Threading.Thread.Sleep(100);

                var info = await api.GetRouteConfig(ag.Tag, item.Name);
                RouteConfigs.Insert(i, info);
                item.Background = Common.BrushFromHex(info.Color);
                item.Foreground = Common.BrushFromHex(info.OppositeColor);
                item.RequestedTheme = ElementTheme.Light;
            }
        }

        public async void LoadStops(Route route)
        {
            var api = Common.NextBusApi;
            int i = Routes.IndexOf(route);

            RouteConfig info;
            if (i >= RouteConfigs.Count)
                info = await api.GetRouteConfig(curAgency.Tag, route.Tag);
            else
                info = RouteConfigs[i];

            StopsBox.Background = Common.BrushFromHex(info.Color);
            StopsBox.Items.Clear();
            Stops.Clear();

            foreach (Stop st in info.Stops)
            {
                Stops.Add(st);
                StopsBox.Items.Add(new ListViewItem()
                {
                    Name = st.Tag,
                    Content = st.Title,
                    Foreground = Common.BrushFromHex(info.OppositeColor),
                    RequestedTheme = Common.ThemeFromColor(info.OppositeColor),
                });
            }
        }

        private async void AgenciesBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (Agencies.Count > 0)
            {
                curAgency = await NextBusApiHelper.GetAgencyByTitle(((ComboBoxItem)AgenciesBox.SelectedItem).Content.ToString(), Agencies);
                LoadRoutes(curAgency);
                RoutesBox.IsEnabled = true;
            }
        }

        private async void RoutesBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (RoutesBox.Items.Count > 0)
            {
                curRoute = await NextBusApiHelper.GetRouteByTitle(curAgency.Tag, ((ComboBoxItem)RoutesBox.SelectedItem).Content.ToString(), Routes);
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
    }
}
