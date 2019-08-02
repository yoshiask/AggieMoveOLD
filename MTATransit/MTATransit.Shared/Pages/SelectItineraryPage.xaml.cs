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
using MTATransit.Shared.API.OTPMTA;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MTATransit.Shared.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SelectItineraryPage : Page
    {
        public ObservableCollection<Models.ItineraryModel> Itineraries { get; set; } = new ObservableCollection<Models.ItineraryModel>();
        public List<Models.PointModel> Points { get; set; }

        public SelectItineraryPage()
        {
            this.InitializeComponent();

            Common.LoadNavView(this, NavView);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Points = e.Parameter as List<Models.PointModel>;
            LoadItineraries(Points); ;

            base.OnNavigatedTo(e);
        }

        private async void LoadItineraries(List<Models.PointModel> points)
        {
            if (points.Count < 2)
            {
                // TODO: The user did not select more than one point.
                // There is not trip specified, so notify the user and
                // navigate back to the Navigate home page.
                Frame.Navigate(typeof(NavigateHomePage));
                return;
            }

            // TODO: This is only temporary, as the app should definitely support
            // more than just two points.

            string startCoord = points[0].Longitude.ToString() + "," + points[0].Latitude.ToString();
            string endCoord = points[1].Longitude.ToString() + "," + points[1].Latitude.ToString();

            var request = new PlanRequestParameters()
            {
                FromPlace = startCoord,
                ToPlace = endCoord,
                ArriveDate = points[1].ArrivalDateTime.Value.ToString("MM-dd-yyy"),
                ArriveTime = points[1].ArrivalTime
            };
            var plan = (await Common.OTPMTAApi.CalculatePlan(request)).Plan;

            if (plan == null)
                return;

            foreach (Itinerary it in plan.Itineraries)
            {
                Itineraries.Add(new Models.ItineraryModel(it));
            }

            #region old junk
            /*foreach (Itinerary it in plan.Itineraries)
            {
                int i = plan.Itineraries.IndexOf(it);
                var dialog = new Controls.DialogBox("Itinerary " + i.ToString(), it.ToString());
                dialog.OnDialogClosed += (Controls.DialogBox.DialogResult result) =>
                {
                    switch (result)
                    {
                        case Controls.DialogBox.DialogResult.Primary:

                            break;

                        case Controls.DialogBox.DialogResult.Secondary:

                            break;
                    }
                    MainGrid.Children.Remove(dialog);
                };
                MainGrid.Children.Add(dialog);
                Grid.SetRowSpan(dialog, 3);
            }*/
            #endregion
        }

        private void BackButton_Click(object sender, RoutedEventArgs args)
        {
            Frame.Navigate(typeof(NavigateHomePage), Points);
        }

        private void NextButton_Click(object sender, RoutedEventArgs args)
        {
            var dialog = new Controls.DialogBox("\"Go\" Button", "This button would start navigation.\nOf course, the app will support\nthis, just like Google Maps.\nAt the moment, this feature is\nincomplete.");
            dialog.OnDialogClosed += (Controls.DialogBox.DialogResult result) =>
            {
                MainGrid.Children.Remove(dialog);
            };
            MainGrid.Children.Add(dialog);
            Grid.SetRowSpan(dialog, 2);
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Common.NavView_SelectionChanged(this, sender, args);
        }

        public class ItineraryPageNavigationArgs
        {
            public IList<Models.PointModel> Points;
        }
    }
}
