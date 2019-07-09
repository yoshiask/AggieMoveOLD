using MTATransit.Shared.API.ArcGIS;
using MTATransit.Shared.API.MTA;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using NextBus.NET;
using Windows.UI.Xaml;

namespace MTATransit.Shared
{
    public static class Common
    {
        // Initialize the services that we're requesting from
        public static NextBusClient NextBusApi = new NextBusClient();
        public static API.NextBus.INextBusApi OldNextBusApi {
            get {
                return RestService.For<Shared.API.NextBus.INextBusApi>("http://webservices.nextbus.com/service/");
            }
        }
        public static IArcGISApi ArcGISApi {
            get {
                return RestService.For<IArcGISApi>("http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/");
            }
        }
        public static IMTAApi MTAApi {
            get {
                return RestService.For<IMTAApi>("http://api.metro.net/");
            }
        }

        /// <summary>
        /// Creates Color from HEX code
        /// </summary>
        /// <param name="hex">HEX code string</param>
        public static Color ColorFromHex(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            return Color.FromArgb(255, r, g, b);
        }

        /// <summary>
        /// Creates Brush from HEX code
        /// </summary>
        /// <param name="hex">HEX code string</param>
        /// <returns></returns>
        public static SolidColorBrush BrushFromHex(string hex)
        {
            return new SolidColorBrush(ColorFromHex(hex));
        }

        /// <summary>
        /// Figures out whether the color is dark or light using luminance
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static ElementTheme ThemeFromColor(string hex)
        {
            var color = ColorFromHex(hex);
            if (color.R * 0.2126 + color.G * 0.7152 + color.B * 0.0722 > 255 / 2)
                return ElementTheme.Dark;
            else
                return ElementTheme.Light;
        }

        public static Dictionary<string, Tuple<Type, NavigationViewItem>> Pages = new Dictionary<string, Tuple<Type, NavigationViewItem>>
        {
            {
                "Explore",
                new Tuple<Type, NavigationViewItem>(
                    typeof(MainPage),
                    new NavigationViewItem()
                    {
                        Icon = new SymbolIcon(Symbol.Street),
                        Content = "Explore",
                        Tag = "Explore your options"
                    }
                )
            },

            {
                "Discover",
                new Tuple<Type, NavigationViewItem>(
                    typeof(object),
                    new NavigationViewItem()
                    {
                        Icon = new SymbolIcon(Symbol.Map),
                        Content = "Discover",
                        Tag = "Discover hotspots in your area"
                    }
                )
            },

            {
                "Navigate",
                new Tuple<Type, NavigationViewItem>(
                    typeof(object),
                    new NavigationViewItem()
                    {
                        Icon = new SymbolIcon(Symbol.Directions),
                        Content = "Navigate",
                        Tag = "Navigate to your destination"
                    }
                )
            },
        };

        public static void NavView_SelectionChanged(Page page, NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Frame frame = Window.Current.Content as Frame;

            if (args.IsSettingsSelected)
            {
                if (page.GetType() != typeof(Pages.SettingsPage))
                    frame.Navigate(typeof(Pages.SettingsPage));
                return;
            }

            var item = (NavigationViewItem)args.SelectedItem;

            Type newPage = Common.Pages[item.Content as string].Item1;

            if (page.GetType() == newPage)
                return;

            if (newPage.BaseType == typeof(Page))
                frame.Navigate(newPage);
        }
    }
}
