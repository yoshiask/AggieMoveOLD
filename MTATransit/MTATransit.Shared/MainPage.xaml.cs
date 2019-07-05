using MTATransit.Shared;
using MTATransit.Shared.API.NextBus;
using MTATransit.Shared.Pages;
using Refit;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MTATransit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string agency;

        public MainPage()
        {
            this.InitializeComponent();
            Load();

            /*var test = new PredictionResponse()
            {
                Copyright = "All data copyright Los Angeles Metro 2019.",
                Predictions = new Predictions()
                {
                    AgencyTitle = "Los Angeles Metro",
                    RouteTag = "910",
                    RouteTitle = "910 Metro Silver Line",
                    Direction = new Predictions.DirectionInfo() {
                        Title = "North to Silver Line-El Monte Sta Via Downtown",
                    },
                    StopTitle = "El Monte Busway / Alameda - Union Statio",
                    StopTag = "70"
                }
            };
            System.Diagnostics.Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(test));*/

            // TODO: Change minimum UWP requirement to b. 16299 so
            //  GIS and MasterDetailsView are available
        }

        public async void Load()
        {
            // Get a list of the agencies this api serves
            var agencies = (await Common.NextBusApi.GetAgencies()).Items;
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

            var agencies = (await api.GetAgencies()).Items;
            var ag = agencies.Find(x => x.Title == title);
            // Now load the available routes
            var routes = (await api.GetRoutes(ag.Tag)).Items;
            foreach (Route rt in routes)
            {
                var info = (await api.GetRouteInfo(ag.Tag, rt.Tag)).Route;
                RoutesBox.Items.Add(new ComboBoxItem()
                {
                    Content = info.Title,
                    Background = Common.BrushFromHex(info.Color),
                    Foreground = Common.BrushFromHex(info.ForegroundColor),
                });
            }
        }

        public async void LoadRouteInfo(string agency, string route)
        {
            var api = Common.NextBusApi;
            var info = await api.GetRouteInfo(agency, route);

            RoutesList.Background = Common.BrushFromHex(info.Route.Color);
            RoutesList.Items.Clear();

            foreach (Stop st in info.Route.Stops)
            {
                RoutesList.Items.Add(new ListBoxItem()
                {
                    Name = st.Tag,
                    Content = st.Title,
                    Foreground = Common.BrushFromHex(info.Route.ForegroundColor),
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
                ag, rt, ((ListBoxItem)RoutesList.SelectedItem).Name
            };
            Frame.Navigate(typeof(RouteDetailsView), pars);
        }
    }
}
