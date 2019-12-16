using MTATransit.Shared.API;
using MTATransit.Shared.API.RestBus;
using MTATransit.Shared.API.ArcGIS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using MTATransit.Shared.Controls;
using Windows.Networking.Connectivity;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MTATransit.Shared.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExplorePage : Page
    {
        public ExplorePage()
        {
            this.InitializeComponent();

            LoadAgencies();
        }

        private void SetLoadingBar(bool loading)
        {
            string contents = loading ? "loadingStarted" : "loadingFinished";
            var myMessage = new GalaSoft.MvvmLight.Messaging.NotificationMessage(contents);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(myMessage);
        }

        #region By Agency
        Agency curAgency;
        Route curRoute;
        List<Agency> Agencies = new List<Agency>();
        List<Route> Routes = new List<Route>();
        List<Stop> Stops = new List<Stop>();

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
                var dir = curRoute.Directions.Find(d => d.Stops.Contains(st.Id));
                Stops.Add(st);
                StopsBox.Items.Add(new ListViewItem()
                {
                    Name = st.Id,
                    Content = $"{st.Title} ({dir.Title})",
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
            Frame.Navigate(typeof(RouteDetailsView), pars,
                new SlideNavigationTransitionInfo()
                {
                    Effect = SlideNavigationTransitionEffect.FromRight
                }
            );
        }
        #endregion

        #region By Location
        private string AddressSearchQuery { get; set; }
        private Suggestion Suggestion { get; set; }
        private List<Suggestion> Suggestions { get; set; } = new List<Suggestion>();

        private ObservableCollection<LocationPrediction> LocationPredictions { get; set; }
            = new ObservableCollection<LocationPrediction>();
        private Shared.Models.PointModel Point { get; set; }

        private readonly string NoResultsString = "No results";


        private async void AddressBox_TextChanged(AutoSuggestBox s, AutoSuggestBoxTextChangedEventArgs a)
        {
            if (a.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                try
                {
                    AddressSearchQuery = s.Text;
                    var suggs = await Common.ArcGISApi.GetSuggestions(s.Text);
                    if (suggs != null && suggs.Items != null && suggs.Items.Count > 0)
                    {
                        List<string> SuggestList = new List<string>();
                        Suggestions.Clear();
                        Suggestions = suggs.Items;
                        foreach (Suggestion sugg in suggs.Items)
                        {
                            SuggestList.Add(sugg.Text);
                        }
                        s.ItemsSource = SuggestList;
                    }
                    else
                    {
                        s.ItemsSource = new string[] { NoResultsString };
                    }
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    // TODO: Handle what's likely an HTTP timeout
                    //Debug.WriteLine(ex);
                }
            }
        }

        private async void AddressBox_QuerySubmitted(AutoSuggestBox s, AutoSuggestBoxQuerySubmittedEventArgs a)
        {
            if (a.ChosenSuggestion != null)
            {
                // The user selected a suggestion, show them the results
                Suggestion = Suggestions.Find(su => su.Text == (string)a.ChosenSuggestion);
            }
            else if (!string.IsNullOrEmpty(a.QueryText))
            {
                var sugg = await GetDefaultSuggestion(s.Text);
                if (sugg != null)
                    // A search has automatically been done, take the first result as if the user selected it
                    Suggestion = sugg;
                else
                    s.ItemsSource = new string[] { NoResultsString };
            }
        }

        private void AddressBox_SuggestionChosen(AutoSuggestBox s, AutoSuggestBoxSuggestionChosenEventArgs a)
        {
            // Here's where we autocomplete
            if ((string)a.SelectedItem != NoResultsString)
            {
                s.Text = (string)a.SelectedItem;
            }
        }

        private async System.Threading.Tasks.Task<Suggestion> GetDefaultSuggestion(string query)
        {
            var suggs = await Common.ArcGISApi.GetSuggestions(query);
            if (suggs != null && suggs.Items != null && suggs.Items.Count > 0)
                // A search has automatically been done, take the first result as if the user selected it
                return suggs.Items.FirstOrDefault();
            else
                return null;
        }


        private async void LocationGoButton_Click(object sender, RoutedEventArgs e)
        {
            SetLoadingBar(true);
            Point = null;
            if (CurrentLocationButton.IsChecked.Value)
            {
                // Try to get current location, use instead of selected address
                Point = await Common.SpatialHelper.GetCurrentLocation();
                if (Point == null)
                {
                    var dialog = new DialogBox("Error", "Access to your device's location was denied.\nPlease allow access or manually enter\nan address.");
                    dialog.OnDialogClosed += (DialogBox.DialogResult result) =>
                    {
                        MainGrid.Children.Remove(dialog);
                    };
                    MainGrid.Children.Add(dialog);
                    return;
                }
            }
            else
            {
                Point = new Shared.Models.PointModel();
                Point.Address = AddressBox.Text;
                AddressCandidate geocode;
                if (Suggestion == null)
                {
                    Suggestion = await GetDefaultSuggestion(AddressSearchQuery);
                    if (Suggestion == null)
                        Frame.Navigate(
                            typeof(FatalErrorPage),
                            new FatalErrorPage.FatalErrorArgs()
                            {
                                Icon = "\uE774",
                                Message = "Failed to get any suggestions based on the user's address query",
                                Exception = new ArgumentNullException("Suggestion", "Failed to get any suggestions based on the user's address query")
                            }
                        );
                }

                geocode = (await Common.ArcGISApi.Geocode(Point.Address, Suggestion.MagicKey)).Candidates[0];
                Point.Title = geocode.Address;
                Point.Latitude = Convert.ToDecimal(geocode.Location.Latitude);
                Point.Longitude = Convert.ToDecimal(geocode.Location.Longitude);
            }

            foreach (LocationPrediction pre in await Common.RestBusApi.GetPredictions(Point.Latitude, Point.Longitude))
            {
                Debug.WriteLine(pre.Stop.Title);
                LocationPredictions.Add(pre);

                var connectionCost = NetworkInformation.GetInternetConnectionProfile().GetConnectionCost();
                if (connectionCost.NetworkCostType == NetworkCostType.Unknown
                        || connectionCost.NetworkCostType == NetworkCostType.Unrestricted)
                {
                    continue;
                    // Connection cost is unknown/unrestricted, load as much data as you want

                    // Now get the routeConfig for colors
                    for (int i = 0; i < RoutesBox.Items.Count; ++i)
                    {
                        var item = RoutesBox.Items[i] as ComboBoxItem;

                        var info = await Common.RestBusApi.GetRoute(null, item.Name);
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
                else
                {
                    // Metered Network, don't load the extra stuff
                }
            }
            SetLoadingBar(false);
        }

        private async void LocationStopsBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            SetLoadingBar(true);
            var selectedPrediction = LocationPredictions[LocationStopsBox.SelectedIndex];

            Agency ag = await Common.RestBusApi.GetAgency(selectedPrediction.Agency.Id);
            Route rt = await Common.RestBusApi.GetRoute(ag.Id, selectedPrediction.Route.Id);
            Stop st = rt.Stops.Find((s) => s.Id == selectedPrediction.Stop.Id);

            SetLoadingBar(false);
            Frame.Navigate(
                typeof(RouteDetailsView), new object[] { ag, rt, st },
                new SlideNavigationTransitionInfo()
                {
                    Effect = SlideNavigationTransitionEffect.FromRight
                }
            );
        }
        #endregion
    }
}
