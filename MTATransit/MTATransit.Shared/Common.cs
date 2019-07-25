﻿using MTATransit.Shared.API.ArcGIS;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using NextBus.NET;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using MTATransit.Shared.API.RestBus;
using System.Diagnostics;
using System.Linq;
using Microsoft.Toolkit.Uwp.Helpers;

namespace MTATransit.Shared
{
    public static class Common
    {
        #region API Initializers
        // Initialize the services that we're requesting from
        public static IRestBusApi RestBusApi {
            get {
                return RestService.For<API.RestBus.IRestBusApi>("http://restbus.info/api");
            }
        }
        public static IArcGISApi ArcGISApi {
            get {
                return RestService.For<IArcGISApi>("http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/");
            }
        }
        public static API.MTA.IMTAApi MTAApi {
            get {
                return RestService.For<API.MTA.IMTAApi>("http://api.metro.net/");
            }
        }
        public static API.LAMove.ILAMoveApi LAMoveApi {
            get {
                return RestService.For<API.LAMove.ILAMoveApi>("http://lamove-api.herokuapp.com/v1");
            }
        }
        #endregion

        /// <summary>
        /// Creates Color from HEX code
        /// </summary>
        /// <param name="hex">HEX code string</param>
        public static Windows.UI.Color ColorFromHex(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            return Windows.UI.Color.FromArgb(255, r, g, b);
        }

        /// <summary>
        /// Creates Color from HEX code
        /// </summary>
        /// <param name="hex">HEX code string</param>
        public static System.Drawing.Color DrawingColorFromHex(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            return System.Drawing.Color.FromArgb(255, r, g, b);
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

        public static class SpatialHelper
        {
            public static double GetDistance(double lat1, double lon1, double lat2, double lon2)
            {
                var R = 6371; // Radius of the earth in km
                var dLat = ToRadians(lat2 - lat1);  // deg2rad below
                var dLon = ToRadians(lon2 - lon1);
                var a =
                    Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                var d = R * c; // Distance in km
                return d;
            }

            public static double ToRadians(double deg)
            {
                return deg * (Math.PI / 180);
            }

            public static bool IsWithinRadius(double lat, double lon, double x, double y, double radius)
            {
                return GetDistance(lat, lon, x, y) <= radius;
            }
        }
    }
}
