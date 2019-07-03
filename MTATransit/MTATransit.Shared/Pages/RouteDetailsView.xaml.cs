using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MTATransit.Shared.API.NextBus;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MTATransit.Shared.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RouteDetailsView : Page
    {
        public RouteDetailsView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadRouteInfo(((string[])e.Parameter)[0], ((string[])e.Parameter)[1]);
            base.OnNavigatedTo(e);
        }

        public async void LoadRouteInfo(string agency, string route)
        {
            var api = Common.NextBusApi;
            var info = await api.GetRouteInfo(agency, route);

            MainGrid.Background = Common.BrushFromHex(info.Route.Color);

            foreach (Stop st in info.Route.Stops)
            {
                StopsBox.Items.Add(new ListViewItem()
                {
                    Content = st.Title,
                    Foreground = Common.BrushFromHex(info.Route.ForegroundColor),
                });
            }
        }
    }
}
