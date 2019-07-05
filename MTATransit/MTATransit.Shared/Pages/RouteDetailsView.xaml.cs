using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MTATransit.Shared.API.NextBus;
using System;
using System.Collections.Generic;

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
            var agency = ((object[])e.Parameter)[0] as Agency;
            var route = ((object[])e.Parameter)[1] as Route;
            var stop = ((object[])e.Parameter)[2] as string;

            LoadStopInfo(agency, route, stop);
            base.OnNavigatedTo(e);
        }

        public async void LoadStopInfo(Agency agency, Route route, string stopId)
        {
            var api = Common.NextBusApi;
            var response = await api.GetStopPredictions(agency.Tag, route.Tag, stopId);
            var routeInfo = await api.GetRouteInfo(agency.Tag, route.Tag);

            MainGrid.Background = Common.BrushFromHex(routeInfo.Route.Color);
            PageHeader.Foreground = Common.BrushFromHex(routeInfo.Route.ForegroundColor);

            Prediction pred;

            // Do these checks because the API always returns
            // 200 OK (even if there's an error). It also sometimes returns only one
            // item without a list
            try
            {
                Prediction test = Newtonsoft.Json.JsonConvert.DeserializeObject<Prediction>(response.Predictions.ToString());
                pred = test;
            }
            catch
            {
                try
                {
                    List<Prediction> predList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Prediction>>(response.Predictions.ToString());
                    pred = predList[0];
                }
                catch
                {
                    throw new Exception("Request was not successful");
                }
            }

            PageHeader.Text = pred.StopTitle;
            foreach (PredictionInfo pr in pred.Direction.Prediction)
            {
                int secs = pr.Seconds;
                string display = Math.Round(Convert.ToDouble(pr.Seconds) / 60, 0).ToString();

                PredictionBox.Items.Add(new ListViewItem()
                {
                    Content = display,
                    Foreground = Common.BrushFromHex(routeInfo.Route.ForegroundColor),
                });
            }
        }
    }
}
