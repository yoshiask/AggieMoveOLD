﻿using System;
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
    public sealed partial class NavigateHomePage : Page
    {
        public ObservableCollection<Models.PointModel> Points { get; set; } = new ObservableCollection<Models.PointModel>();

        public NavigateHomePage()
        {
            this.InitializeComponent();

            Common.LoadNavView(this, NavView);

            var pointDialog = new Controls.NewPointDialog(
                "Add Point",
                new Models.PointModel()
                {
                    Title = "A: El Monte Station",
                    Address = "El Monte, CA 91731",
                    ArrivalTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds(),
                    DepartureTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() + 1000,
                },
                true);
            pointDialog.OnDialogClosed += (Controls.NewPointDialog.NewPointDialogResult result) =>
            {
                MainGrid.Children.Remove(pointDialog);
                Points.Add(result.Model);
            };
            MainGrid.Children.Add(pointDialog);
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Common.NavView_SelectionChanged(this, sender, args);
        }
    }
}
