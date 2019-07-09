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
    }
}
