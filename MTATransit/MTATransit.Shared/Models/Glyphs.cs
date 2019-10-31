using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MTATransit.Shared.Models
{
    public class Glyphs
    {
        public class TransitIcon
        {
            /// <summary>
            /// The human name of the icon
            /// </summary>
            public string Title { get; internal set; }
            /// <summary>
            /// The name of the mode of transportation, as used in OTP
            /// </summary>
            public string OTPMode { get; internal set; }
            /// <summary>
            /// The character as it is in the LA Move Icon font
            /// </summary>
            public string FontCharacter { get; internal set; }
            /// <summary>
            /// The character as it is in the LA Move Icon font, heavy
            /// </summary>
            public string FontCharacterHeavy { get; internal set; }
            public string DefaultBackColor { get; internal set; }
            public string DefaultForeColor { get; internal set; }

            public static Dictionary<string, TransitIcon> DefaultTransitIconsOTP = new Dictionary<string, TransitIcon>()
            {
                {
                    "BUS",
                    new TransitIcon()
                    {
                        Title = "Bus",
                        OTPMode = "BUS",
                        FontCharacterHeavy = "\ue908",
                        DefaultBackColor = "#FC7112",
                        DefaultForeColor = "#FFFFFF"
                    }
                },

                {
                    "RAIL",
                    new TransitIcon()
                    {
                        Title = "Heavy Rail",
                        OTPMode = "RAIL",
                        FontCharacterHeavy = "\uE906",
                        DefaultBackColor = "#0B5F60",
                        DefaultForeColor = "#FFFFFF"
                    }
                },

                {
                    "TRAM",
                    new TransitIcon()
                    {
                        Title = "Light Rail",
                        OTPMode = "TRAM",
                        FontCharacterHeavy = "\uE907",
                        DefaultBackColor = "#FFB300",
                        DefaultForeColor = "#000000"
                    }
                },

                {
                    "BIKE",
                    new TransitIcon()
                    {
                        Title = "Bike",
                        OTPMode = "BIKE",
                        FontCharacterHeavy = "\uE905",
                        DefaultBackColor = "#7AC34E",
                        DefaultForeColor = "#FFFFFF"
                    }
                },

                {
                    "WALK",
                    new TransitIcon()
                    {
                        Title = "Walk",
                        OTPMode = "WALK",
                        FontCharacterHeavy = "\uE904",
                        DefaultBackColor = "#334995",
                        DefaultForeColor = "#FFFFFF"
                    }
                },

                {
                    "CAR",
                    new TransitIcon()
                    {
                        Title = "Car",
                        OTPMode = "CAR",
                        FontCharacterHeavy = "\uE901",
                        DefaultBackColor = "#795548",
                        DefaultForeColor = "#FFFFFF"
                    }
                },
            };

            public Border GetIcon(bool isHeavy)
            {
                return GetIcon(Common.ColorFromHex(DefaultForeColor), Common.ColorFromHex(DefaultBackColor), isHeavy);
            }
            public Border GetIcon(Color fore, bool isHeavy)
            {
                return GetIcon(fore, Colors.Transparent, isHeavy);
            }
            public Border GetIcon(SolidColorBrush fore, bool isHeavy)
            {
                return GetIcon(fore, new SolidColorBrush(Colors.Transparent), isHeavy);
            }
            public Border GetIcon(Color fore, Color back, bool isHeavy)
            {
                return GetIcon(new SolidColorBrush(fore), new SolidColorBrush(back), isHeavy);
            }
            public Border GetIcon(SolidColorBrush fore, SolidColorBrush back, bool isHeavy)
            {
                return new Border()
                {
                    Child = new FontIcon()
                    {
                        Glyph = isHeavy ? FontCharacterHeavy : FontCharacter,
                        FontFamily = Common.LAMoveIconFont,
                        FontSize = 24,
                        Height = 27 + 1 / 3,
                        Width = 27 + 1 / 3,
                        Foreground = fore
                    },
                    Background = back
                };
            }
        }
    }
}
