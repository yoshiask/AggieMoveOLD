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
        string agency;
        List<Stop> stops = new List<Stop>();

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
            List<Agency> agencies = (await Common.NextBusApi.GetAgencies()).ToList();
            foreach (Agency ag in agencies)
            {
                AgenciesBox.Items.Add(new ComboBoxItem()
                {
                    Content = ag.Title,
                });
            }
        }

        public async void LoadRoutes(string title)
        {
            var api = Common.NextBusApi;
            RoutesBox.Items.Clear();

            var agencies = (await api.GetAgencies()).ToList();
            var ag = agencies.Find(x => x.Title == title);

            // Now load the available routes
            var routes = (await api.GetRoutesForAgency(ag.Tag));
            foreach (Route rt in routes)
            {
                RoutesBox.Items.Add(new ComboBoxItem()
                {
                    Name = rt.Tag,
                    Content = rt.Title,
                });
            }

            // Now get the routeConfig for colors
            for (int i = 0; i < RoutesBox.Items.Count; ++i)
            {
                var item = RoutesBox.Items[i] as ComboBoxItem;
                var info = await api.GetRouteConfig(ag.Tag, item.Name);
                item.Background = Common.BrushFromHex(info.Color);
                item.Foreground = Common.BrushFromHex(info.OppositeColor);
            }
        }

        public async void LoadRouteInfo(string agency, string route)
        {
            var api = Common.NextBusApi;
            var info = await api.GetRouteConfig(agency, route);

            RoutesList.Background = Common.BrushFromHex(info.Color);
            RoutesList.Items.Clear();
            stops.Clear();

            foreach (Stop st in info.Stops)
            {
                stops.Add(st);
                RoutesList.Items.Add(new ListViewItem()
                {
                    Name = st.Tag,
                    Content = st.Title,
                    Foreground = Common.BrushFromHex(info.OppositeColor),
                    IsHitTestVisible = false,
                });
            }
        }

        private void AgenciesBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            agency = ((ComboBoxItem)AgenciesBox.SelectedItem).Content.ToString();
            LoadRoutes(agency);
            RoutesBox.IsEnabled = true;
        }

        private async void RoutesBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (RoutesBox.Items.Count > 0)
            {
                var ag = await NextBusApiHelper.GetAgencyByTitle(agency);
                var rt = await NextBusApiHelper.GetRouteByTitle(ag.Tag, ((ComboBoxItem)RoutesBox.SelectedItem).Content.ToString());
                LoadRouteInfo(ag.Tag, rt.Tag);
                RoutesList.ScrollIntoView(RoutesList.Items[0]);
            }
        }

        private async void RoutesList_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            var ag = await NextBusApiHelper.GetAgencyByTitle(agency);
            var rt = await NextBusApiHelper.GetRouteByTitle(ag.Tag, ((ComboBoxItem)RoutesBox.SelectedItem).Content.ToString());

            object[] pars =
            {
                ag, rt, stops[RoutesList.SelectedIndex]
            };
            Frame.Navigate(typeof(RouteDetailsView), pars);
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
