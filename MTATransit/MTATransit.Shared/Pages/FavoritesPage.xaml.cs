using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MTATransit.Shared.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FavoritesPage : Page
    {
        public ObservableCollection<Models.PointModel> SavedPoints { get; set; } = new ObservableCollection<Models.PointModel>();
        public ObservableCollection<API.OTP.PlanRequestParameters> SavedRoutes { get; set; } = new ObservableCollection<API.OTP.PlanRequestParameters>();

        public FavoritesPage()
        {
            this.InitializeComponent();
            

            Common.RoamingSettings.SetUpRoamingSettings();
            LoadLocations();
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
        private void AddPointButton_Click(object sender, RoutedEventArgs e)
        {
            NewPoint();
        }
        #endregion

        #region SavedPoint Functions
        private void AddPoint(Models.PointModel model)
        {
            SavedPoints.Add(model);
        }
        private void RemovePoint(Models.PointModel model)
        {
            SavedPoints.Remove(model);
            Common.RoamingSettings.DeleteLocation(model.Title);
        }
        private void ReplacePoint(Models.PointModel oldModel, Models.PointModel model)
        {
            int i = SavedPoints.IndexOf(oldModel);
            SavedPoints.Remove(oldModel);
            SavedPoints.Insert(i, model);
            Common.RoamingSettings.ReplaceLocation(oldModel.Title, model.Title, model.Longitude, model.Latitude);
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
                    SavedPoints.Add(result.Model);
                    Common.RoamingSettings.SetLocation(result.Model.Title, result.Model.Longitude, result.Model.Latitude);
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

        private async void LoadLocations()
        {
            SetLoadingBar(true);
            var locations = await Common.RoamingSettings.GetAllLocations();
            foreach (KeyValuePair<string, API.ArcGIS.Location> pair in locations)
            {
                if (SavedPoints.FirstOrDefault((m) => { return m.Title == pair.Key; }) != default(Models.PointModel))
                    continue;

                var loc = pair.Value;
                SavedPoints.Add(new Models.PointModel()
                {
                    Title = pair.Key,
                    Address = loc.Longitude.ToString() + ", " + loc.Latitude.ToString(),
                    Longitude = Convert.ToDecimal(loc.Longitude),
                    Latitude = Convert.ToDecimal(loc.Latitude),
                });
            }
            SetLoadingBar(false);
        }

        private void SetLoadingBar(bool loading)
        {
            string contents = loading ? "loadingStarted" : "loadingFinished";
            var myMessage = new GalaSoft.MvvmLight.Messaging.NotificationMessage(contents);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(myMessage);
        }
    }
}
