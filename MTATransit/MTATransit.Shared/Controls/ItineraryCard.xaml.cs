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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MTATransit.Shared.Controls
{
    public sealed partial class ItineraryCard : UserControl
    {
        public Models.ItineraryModel Itin {
            get {
                return this.DataContext as Models.ItineraryModel;
            }
        }

        public ItineraryCard()
        {
            this.InitializeComponent();
        }

        private void Card_Loading(FrameworkElement sender, object args)
        {
            // TODO: Find a way to add the arrows using Binding

            for (int i = 0; i < Itin.Legs.Count; i++)
            {
                string text = "";

                var l = Itin.Legs[i];
                text += l.ToLegString();
                if (i != Itin.Legs.Count - 1)
                    text += " > ";

                LegText.Text += text;
            }
        }
    }
}
