using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MTATransit.Shared.API.Yelp;
using System.Diagnostics;
using MTATransit.Shared.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MTATransit.Shared.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DiscoverHomePage : Page
    {
        public ObservableCollection<Business> Points { get; set; } = new ObservableCollection<Business>();

        public DiscoverHomePage()
        {
            this.InitializeComponent();

            LoadBusinesses();
        }

        private async void LoadBusinesses()
        {
            SetLoadingBar(true);
            // Try to get current location, use instead of selected address
            var loc = await Common.SpatialHelper.GetCurrentLocation();
            if (loc == null)
            {
                var dialog = new DialogBox("Error", "Access to your device's location was denied.\nPlease allow access or manually enter\nan address.");
                dialog.OnDialogClosed += (DialogBox.DialogResult result) =>
                {
                    MainGrid.Children.Remove(dialog);
                };
                MainGrid.Children.Add(dialog);
                return;
            }

            var response = await Common.YelpApi.BusinessSearch(loc.Latitude, loc.Longitude, "food");
            foreach (Business b in response.Businesses)
            {
                Points.Add(b);
            }
            SetLoadingBar(false);
        }

        #region Swipe Events
        private void FavoritePoint_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            return;
            //RemovePoint((Models.PointModel)args.SwipeControl.DataContext);
        }

        private void GoPoint_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            Go((Business)args.SwipeControl.DataContext);
        }
        #endregion

        private void Go(Business b)
        {
            // TODO: The user has selected the business they want to go to.
            // Send them to the navigation page, with A being their current
            // location and B being the business.
        }

        private void SetLoadingBar(bool loading)
        {
            string contents = loading ? "loadingStarted" : "loadingFinished";
            var myMessage = new GalaSoft.MvvmLight.Messaging.NotificationMessage(contents);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(myMessage);
        }
    }
}
