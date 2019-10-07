using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace MTATransit.Shared.Models
{
    public class Glyphs
    {
        public class TransitIcon
        {
            public string Title { get; }
            /// <summary>
            /// The name of the mode of transportation, as used in OTP
            /// </summary>
            public string OTPMode { get; }
            public string Glyph { get; }

            public BitmapIcon GlyphBitmap {
                get {
                    return new BitmapIcon()
                    {
                        //UriSource = new Uri("")
                    };
                }
            }
        }
    }
}
