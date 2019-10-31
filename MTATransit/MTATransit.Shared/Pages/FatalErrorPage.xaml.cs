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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MTATransit.Shared.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FatalErrorPage : Page
    {
        public string Message { get; set; }
        public string Icon { get; set; }
        public string SecondaryIcon { get; set; }
        public Exception Exception { get; set; }

        public FatalErrorPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var args = e.Parameter as FatalErrorArgs;
            if (args != null)
            {
                Icon = args.Icon;
                SecondaryIcon = args.SecondaryIcon;
                Message = args.Message;
            }
            else
            {
                Icon = "\uE730";
                SecondaryIcon = "";
                Message = "A fatal error occurred";
            }

            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            App.Current.Exit();
            //base.OnNavigatingFrom(e);
        }

        public class FatalErrorArgs
        {
            public string Message { get; set; }
            public string Icon { get; set; }
            public string SecondaryIcon { get; set; }
            public Exception Exception { get; set; }
        }
    }    
}
