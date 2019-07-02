using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace MTATransit.Shared
{
    public static class Common
    {
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
