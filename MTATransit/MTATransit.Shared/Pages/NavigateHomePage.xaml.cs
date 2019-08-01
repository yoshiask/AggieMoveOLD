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

            Common.LoadNavView(this, NavView);
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

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SelectItineraryPage), Points);
        }
        #endregion

        #region Points Functions
        private void AddPoint(Models.PointModel model) {
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
                "Add Point",
                new Models.PointModel()
                {
                    Title = "Baldwin Park, CA 91706",
                    Address = "Baldwin Park, CA 91706",
                    ArrivalTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds(),
                    DepartureTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() + 1000,
                },
                true
            );
            pointDialog.OnDialogClosed += (Controls.NewPointDialog.NewPointDialogResult result) =>
            {
                MainGrid.Children.Remove(pointDialog);
                if (result.Result == Controls.NewPointDialog.DialogResult.Primary)
                {
                    Points.Add(result.Model);
                }
            };
            MainGrid.Children.Add(pointDialog);
        }
        private void EditPoint(Models.PointModel oldModel)
        {
            var pointDialog = new Controls.NewPointDialog(
                "Edit Point",
                oldModel,
                true);
            pointDialog.OnDialogClosed += (Controls.NewPointDialog.NewPointDialogResult result) =>
            {
                MainGrid.Children.Remove(pointDialog);
                if (result.Result == Controls.NewPointDialog.DialogResult.Primary)
                {
                    ReplacePoint(oldModel, result.Model);
                }
            };
            MainGrid.Children.Add(pointDialog);
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

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Common.NavView_SelectionChanged(this, sender, args);
        }
    }
}
