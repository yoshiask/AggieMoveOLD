using MTATransit.Shared.API.NextBus;
using MTATransit.Shared.API.ArcGIS;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace MTATransit.Shared
{
    public static class Common
    {
        // Initialize the service that we're requesting from
        public static INextBusApi NextBusApi = RestService.For<INextBusApi>("http://webservices.nextbus.com/service/");
        public static IArcGISApi ArcGISApi = RestService.For<IArcGISApi>("http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/");
        public static string ArcGISUrl = "http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/";

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
    }
}
