using MTATransit.Shared.API.RestBus;
using MTATransit.Shared.Controls;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MTATransit.Shared.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetectRoutePage : Page
    {
        public DetectRoutePage()
        {
            this.InitializeComponent();

            AttemptDetectRoute();
        }

        private void SetLoadingBar(bool loading)
        {
            string contents = loading ? "loadingStarted" : "loadingFinished";
            var myMessage = new GalaSoft.MvvmLight.Messaging.NotificationMessage(contents);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(myMessage);
        }

        private async void AttemptDetectRoute()
        {
            SetLoadingBar(true);

            // Try to get the current location
            StatusBlock.Text = "Getting your location...";
            StatusProgBar.Value = 25;
            var loc = await Common.SpatialHelper.GetCurrentLocation();
            if (loc == null)
            {
                var locationDialog = new DialogBox("Error", "Access to your device's location was denied.\nPlease allow access or manually enter\nan address.");
                locationDialog.OnDialogClosed += (DialogBox.DialogResult result) =>
                {
                    MainGrid.Children.Remove(locationDialog);
                };
                MainGrid.Children.Add(locationDialog);
                return;
            }

            // Get all stops near the current location
            StatusBlock.Text = "Searching for nearby stops...";
            StatusProgBar.Value = 50;
            List<LocationPrediction> predictions = await Common.RestBusApi.GetPredictions(loc.Latitude, loc.Longitude);
            Console.WriteLine();
            StatusProgBar.Value = 65;
            IEnumerable<LocationPrediction> sortedPreds = from pred in predictions
                                                          orderby Common.SpatialHelper.GetDistance((double)loc.Latitude, (double)loc.Longitude, pred.Stop.Latitude, pred.Stop.Longitude)
                                                          select pred;

            // Get the vehicles
            StatusBlock.Text = "Checking vehicle list...";
            StatusProgBar.Value = 75;
            List<Vehicle> allVehicles = new List<Vehicle>();
            List<string> checkedRoutes = new List<string>();
            for (int i = 0; i < sortedPreds.Count(); i++)
            {
                var pred = sortedPreds.ToList()[i];
                if (!checkedRoutes.Contains(pred.Route.Id))
                {
                    var vehicles = await Common.RestBusApi.GetRouteVehicles(pred.Agency.Id, pred.Route.Id);
                    foreach (Vehicle v in vehicles)
                    {
                        allVehicles.Add(v);
                    }
                    checkedRoutes.Add(pred.Route.Id);
                }
                StatusProgBar.Value += (i / sortedPreds.Count()) * 20;
            }

            // Sort the vehicles by distance
            StatusBlock.Text = "Comparing distances...";
            StatusProgBar.Value = 100;
            IEnumerable<Vehicle> sortedVehicles = from vcl in allVehicles
                                                  orderby Common.SpatialHelper.GetDistance((double)loc.Latitude, (double)loc.Longitude, vcl.Latitude, vcl.Longitude)
                                                  select vcl;
            sortedVehicles = sortedVehicles.Distinct();

            var dialog = new DialogBox("Detect Bus",
                "Are you currently on a " + sortedVehicles.ToList()[0].RouteId + " vehicle with ID " + sortedVehicles.ToList()[0].Id + "?",
                "Yes", "No"
            );
            dialog.OnDialogClosed += (DialogBox.DialogResult result) =>
            {
                MainGrid.Children.Remove(dialog);
            };
            MainGrid.Children.Add(dialog);

            SetLoadingBar(false);
        }

        #region Commandbar Events
        private void RetryButton_Click(object sender, RoutedEventArgs e)
        {
            AttemptDetectRoute();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(
                typeof(NavigateHomePage), null,
                new Windows.UI.Xaml.Media.Animation.SlideNavigationTransitionInfo()
                {
                    Effect = Windows.UI.Xaml.Media.Animation.SlideNavigationTransitionEffect.FromLeft
                });
        }
        #endregion
    }
}
