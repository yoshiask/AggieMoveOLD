using MTATransit.Shared;
using MTATransit.Shared.API.NextBus;
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
        // Initialize the service that we're requesting from
        INextBusApi api = RestService.For<INextBusApi>("http://webservices.nextbus.com/service/");


        public MainPage()
        {
            this.InitializeComponent();
            Load();

            // TODO: Change minimum UWP requirement to b. 16299 so
            //  GIS and MasterDetailsView are available
        }

        public async void Load()
        {
            
            // Get a list of the agencies this api serves
            var agencies = (await api.GetAgencies()).Items;
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
            RoutesBox.Items.Clear();

            var agencies = (await api.GetAgencies()).Items;
            var ag = agencies.Find(x => x.Title == title);
            // Now load the available routes
            var routes = (await api.GetRoutes(ag.Tag)).Items;
            foreach (Route rt in routes)
            {
                var info = (await api.GetRouteInfo(ag.Tag, rt.Tag)).Route;
                RoutesBox.Items.Add(new ListViewItem()
                {
                    Content = info.Title,
                    Background = Common.BrushFromHex(info.Color),
                    Foreground = Common.BrushFromHex(info.ForegroundColor),
                });
            }
        }

        private void AgenciesBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            LoadRoutes(((ComboBoxItem)AgenciesBox.SelectedItem).Content.ToString());
        }
    }
}
