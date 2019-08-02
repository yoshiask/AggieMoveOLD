using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using MTATransit.Shared.API.RestBus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
            //LoadMap(Convert.ToDouble(Itin.Legs[1].Latitude), Convert.ToDouble(Point.Longitude));

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

        /*public void LoadMap(double lat, double lon)
        {
            MainMapView.Map = new Map(
                BasemapType.ImageryWithLabelsVector,
                lon,
                lat,
                12
            );
            MainMapView.LocationDisplay.IsEnabled = true;
            MainMapView.LocationDisplay.ShowLocation = true;

            // Now draw a point where the stop is
            var stopPoint = CreateRouteStop(Convert.ToDecimal(lat), Convert.ToDecimal(lon), System.Drawing.Color.Red);
            MapGraphics.Graphics.Add(stopPoint);

            // Display all of the Park & Ride Locations
            var parkrideUri = new Uri("https://public.gis.lacounty.gov/public/rest/services/LACounty_Dynamic/LMS_Data_Public/MapServer/187");
            var parkrideLayer = new FeatureLayer(new ServiceFeatureTable(parkrideUri));
            //MainMapView.Map.OperationalLayers.Add(parkrideLayer);
        }

        private Graphic CreateRouteStop(decimal lat, decimal lon, System.Drawing.Color fill)
        {
            // Now draw a point where the stop is
            var stopPoint = new MapPoint(Convert.ToDouble(lat),
                Convert.ToDouble(lon), SpatialReferences.Wgs84);
            var pointSymbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, fill, 20);
            pointSymbol.Outline = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.White, 5);
            return new Graphic(stopPoint, pointSymbol);
        }
        private Graphic CreateInactiveRouteStop(decimal lat, decimal lon)
        {
            return CreateRouteStop(lat, lon, System.Drawing.Color.Black);
        }

        private async void MapView_Tapped(object sender, GeoViewInputEventArgs e)
        {
            //MapPoint tappedPoint = (MapPoint)GeometryEngine.Project(e.Location, SpatialReferences.Wgs84);

            var resultGraphics = await MainMapView.IdentifyGraphicsOverlayAsync(MapGraphics, e.Position, 10, false);
        }*/
    }
}
