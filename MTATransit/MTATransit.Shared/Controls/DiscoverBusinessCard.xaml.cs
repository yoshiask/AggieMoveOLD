using MTATransit.Shared.API.Yelp;
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
    public sealed partial class DiscoverBusinessCard : UserControl
    {
        public Business Business {
            get {
                return this.DataContext as Business;
            }
        }

        public DiscoverBusinessCard()
        {
            this.InitializeComponent();
        }

        public delegate void DialogClosedHandler(Business b);
        public event DialogClosedHandler GoButtonClicked;

        private void GoButton_Click(object sender, RoutedEventArgs args)
        {
            GoButtonClicked?.Invoke(Business);
        }
    }
}
