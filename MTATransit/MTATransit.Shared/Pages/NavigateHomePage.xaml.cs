using MTATransit.Shared.API.RestBus;
using MTATransit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class NavigateHomePage : Page
    {
        public ObservableCollection<Models.PointModel> Points { get; set; } = new ObservableCollection<Models.PointModel>();

        public NavigateHomePage()
        {
            this.InitializeComponent();

            Common.RoamingSettings.SetUpRoamingSettings();
        }

        private void SetLoadingBar(bool loading)
        {
            string contents = loading ? "loadingStarted" : "loadingFinished";
            var myMessage = new GalaSoft.MvvmLight.Messaging.NotificationMessage(contents);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(myMessage);
        }

        #region Swipe Events
        private void DeletePoint_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            RemovePoint((Models.PointModel)args.SwipeControl.DataContext);
        }

        private void EditPoint_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            EditPoint((Models.PointModel)args.SwipeControl.DataContext);
        }
        #endregion

        #region Card Events
        private void EditPointButton_Click(object sender, RoutedEventArgs e)
        {
            var model = GetModelFromControls(sender as Button);
            if (model != null)
                EditPoint(model);
        }

        private void DeletePointButton_Click(object sender, RoutedEventArgs e)
        {
            var model = GetModelFromControls(sender as Button);
            if (model != null)
                RemovePoint(model);
        }
        #endregion

        #region CommandBar Events
        private async void AddPointButton_Click(object sender, RoutedEventArgs e)
        {
            if ((await Common.RoamingSettings.GetLocation("Home")) == null)
                Common.RoamingSettings.SetLocation("Home",
                    (await Common.SpatialHelper.GetCurrentLocation()).Longitude,
                    (await Common.SpatialHelper.GetCurrentLocation()).Latitude
                );
            NewPoint();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(
                typeof(SelectItineraryPage), Points.ToList(),
                new Windows.UI.Xaml.Media.Animation.SlideNavigationTransitionInfo()
                {
                    Effect = Windows.UI.Xaml.Media.Animation.SlideNavigationTransitionEffect.FromRight
                });
        }

        private void DetectRouteButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(
                typeof(DetectRoutePage), Points.ToList(),
                new Windows.UI.Xaml.Media.Animation.SlideNavigationTransitionInfo()
                {
                    Effect = Windows.UI.Xaml.Media.Animation.SlideNavigationTransitionEffect.FromRight
                });
        }
        #endregion

        #region Points Functions
        private void AddPoint(Models.PointModel model)
        {
            Points.Add(model);
        }
        private void RemovePoint(Models.PointModel model)
        {
            Points.Remove(model);
        }
        private void ReplacePoint(Models.PointModel oldModel, Models.PointModel model)
        {
            int i = Points.IndexOf(oldModel);
            Points.Remove(oldModel);
            Points.Insert(i, model);
        }

        private void NewPoint()
        {
            var pointDialog = new Controls.NewPointDialog(
                "New Point",
                new Models.PointModel()
                {
                    Title = "",
                    Address = "",
                    ArrivalTime = 0,
                    DepartureTime = 0
                },
                true
            );
            ControlBar.Visibility = Visibility.Collapsed;
            pointDialog.OnDialogClosed += (Controls.NewPointDialog.NewPointDialogResult result) =>
            {
                MainGrid.Children.Remove(pointDialog);
                ControlBar.Visibility = Visibility.Visible;
                if (result.Result == Controls.NewPointDialog.DialogResult.Primary)
                {
                    AddPoint(result.Model);
                }
            };
            MainGrid.Children.Add(pointDialog);
            Grid.SetRowSpan(pointDialog, 2);
        }
        private void EditPoint(Models.PointModel oldModel)
        {
            var pointDialog = new Controls.NewPointDialog(
                "Edit Point",
                oldModel,
                true
            );
            ControlBar.Visibility = Visibility.Collapsed;
            pointDialog.OnDialogClosed += (Controls.NewPointDialog.NewPointDialogResult result) =>
            {
                MainGrid.Children.Remove(pointDialog);
                ControlBar.Visibility = Visibility.Visible;
                if (result.Result == Controls.NewPointDialog.DialogResult.Primary)
                {
                    ReplacePoint(oldModel, result.Model);
                }
            };
            MainGrid.Children.Add(pointDialog);
            Grid.SetRowSpan(pointDialog, 2);
        }

        private Models.PointModel GetModelFromControls(Button button)
        {
            try
            {
                var controlStack = VisualTreeHelper.GetParent(button) as UIElement;
                var cardGrid = VisualTreeHelper.GetParent(controlStack) as Grid;
                var pointCard = cardGrid.Children[0] as Controls.NavigationPointCard;
                return (Models.PointModel)pointCard.DataContext;
            }
            catch (NullReferenceException)
            {
                // The Point Card wasn't found
                return null;
            }
        }
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is List<Models.PointModel>)
            {
                var points = e.Parameter as List<Models.PointModel>;
                if (points != null)
                {
                    Points = new ObservableCollection<Models.PointModel>(points);
                    DataContext = new ObservableCollection<Models.PointModel>(points);
                }
                    
            }
            base.OnNavigatedTo(e);
        }
    }
}
