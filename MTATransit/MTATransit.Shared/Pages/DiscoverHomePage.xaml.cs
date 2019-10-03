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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MTATransit.Shared.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DiscoverHomePage : Page
    {
        public ObservableCollection<Models.PointModel> Points { get; set; } = new ObservableCollection<Models.PointModel>();

        public DiscoverHomePage()
        {
            this.InitializeComponent();

            Common.LoadNavView(this, NavView);
        }

        #region Swipe Events
        private void FavoritePoint_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            return;
            RemovePoint((Models.PointModel)args.SwipeControl.DataContext);
        }

        private void GoPoint_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            return;
            EditPoint((Models.PointModel)args.SwipeControl.DataContext);
        }
        #endregion

        #region Card Events
        private void GoPointButton_Click(object sender, RoutedEventArgs e)
        {
            return;
            var model = GetModelFromControls(sender as Button);
            if (model != null)
                EditPoint(model);
        }

        private void FavoritePointButton_Click(object sender, RoutedEventArgs e)
        {
            return;
            var model = GetModelFromControls(sender as Button);
            if (model != null)
                RemovePoint(model);
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
            pointDialog.OnDialogClosed += (Controls.NewPointDialog.NewPointDialogResult result) =>
            {
                MainGrid.Children.Remove(pointDialog);
                if (result.Result == Controls.NewPointDialog.DialogResult.Primary)
                {
                    Points.Add(result.Model);
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

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Common.NavView_SelectionChanged(this, sender, args);
        }
    }
}
