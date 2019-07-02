using MTATransit.Shared;
using MTATransit.Shared.API.MTA;
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
        public MainPage()
        {
            this.InitializeComponent();
            Load();
        }

        public async void Load()
        {
            var metroApi = RestService.For<IMTAApi>("https://api.metro.net/");
            var agencies = await metroApi.GetAgencies();

            foreach (Agency ag in agencies)
            {
                AgenciesBox.Text += ag.DisplayName;
            }

            // Now load the bus routes
            var routes = (await metroApi.GetRoutes(agencies[0].Id)).Items;
            foreach (Route rt in routes)
            {
                var info = await metroApi.GetRouteInfo(agencies[0].Id, rt.Id);
                RoutesBox.Items.Add(new ListViewItem()
                {
                    Content = info.DisplayName,
                    Background = Common.BrushFromHex(info.Background),
                    Foreground = Common.BrushFromHex(info.Foreground),
                });
            }
        }
    }
}
