using MTATransit.Shared;
using MTATransit.Shared.Pages;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Messaging;
using Windows.UI.Xaml.Input;
using Windows.System;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MTATransit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, MessageReceived);

            LoadNavView(this, NavView);

            MainFrame.Navigated += MainFrame_Navigated;
            NavView.BackRequested += NavView_BackRequested;
            KeyboardAccelerator GoBack = new KeyboardAccelerator();
            GoBack.Key = VirtualKey.GoBack;
            GoBack.Invoked += BackInvoked;
            KeyboardAccelerator AltLeft = new KeyboardAccelerator();
            AltLeft.Key = VirtualKey.Left;
            AltLeft.Invoked += BackInvoked;
            KeyboardAccelerators.Add(GoBack);
            KeyboardAccelerators.Add(AltLeft);
            // ALT routes here
            AltLeft.Modifiers = VirtualKeyModifiers.Menu;
        }

        public List<PageInfo> Pages = new List<PageInfo>
        {
            new PageInfo()
            {
                PageType = typeof(NavigateHomePage),
                Icon = Symbol.Directions,
                Title = "Navigate",
                Subhead = "to your destination",
                Tooltip = "Navigate to your destination"
            },

            new PageInfo()
            {
                PageType = typeof(DiscoverHomePage),
                Icon = Symbol.Map,
                Title = "Discover",
                Subhead = "hotspots in your area",
                Tooltip = "Discover hotspots in your area",
            },

            new PageInfo()
            {
                PageType = typeof(ExplorePage),
                Icon = Symbol.Street,
                Title = "Explore",
                Subhead = "your transit options",
                Tooltip = "Explore your transit options",
            },

            new PageInfo()
            {
                PageType = typeof(FavoritesPage),
                Icon = Symbol.OutlineStar,
                Title = "Favorites",
                Subhead = "View and manage your favorites",
                Tooltip = "View and manage your favorites",
            },
        };

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                if (args.IsSettingsSelected)
                    MainFrame.Navigate(typeof(SettingsPage));
                return;
            }

            PageInfo pageInfo = Pages[sender.MenuItems.IndexOf(args.SelectedItem)];
            if (pageInfo.PageType.BaseType == typeof(Page))
                MainFrame.Navigate(pageInfo.PageType, null, new Windows.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo());
        }

        private void LoadNavView(Page page, NavigationView NavView)
        {
            foreach (PageInfo info in Pages)
            {
                NavView.MenuItems.Add(info.NavViewItem);

                // If the menu item we're adding goes to this page, then select it
                if (info.PageType == page.GetType())
                    NavView.SelectedItem = info.NavViewItem;
            }
        }

        public void SetLoadingBar(bool loading)
        {
            PageLoadingBar.Opacity = loading ? 1.0: 0.0;
        }
        public void MessageReceived(NotificationMessage message)
        {
            string token = message.Notification;
            switch (token)
            {
                case "loadingStarted":
                    SetLoadingBar(true);
                    break;

                case "loadingFinished":
                    SetLoadingBar(false);
                    break;
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = MainFrame.CanGoBack;

            var pageInfo = Pages.Find((info) => info.PageType == e.SourcePageType);
            // Hide the header if the page is not top-level
            if (pageInfo == null)
            {
                PageHeader.Visibility = Visibility.Collapsed;
            }
            else
            {
                PageHeader.Visibility = Visibility.Visible;
                // Set the header to the correct info
                PageTitleBox.Text = pageInfo.Title;
                PageIconBox.Symbol = pageInfo.Icon;
                PageSubheadBox.Text = pageInfo.Subhead;
            }
        }

        #region Back Button
        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            On_BackRequested();
        }

        // Handles system-level BackRequested events and page-level back button Click events
        private bool On_BackRequested()
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
                return true;
            }
            return false;
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }
        #endregion
    }
}
