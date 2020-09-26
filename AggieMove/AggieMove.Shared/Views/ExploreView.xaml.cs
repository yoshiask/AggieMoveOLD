﻿using AggieMove.Helpers;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TamuBusFeed;
using TamuBusFeed.Models;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AggieMove.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ExploreView : Page
	{
        public ObservableCollection<Route> Routes = new ObservableCollection<Route>();

        public ExploreView()
		{
			this.InitializeComponent();
		}

		private async void Page_Loaded(object sender, RoutedEventArgs e)
		{
            Point currentLoc = await SpatialHelper.GetCurrentLocation();
            LoadMap(currentLoc.Y, currentLoc.X);

            SpatialHelper.Geolocator.PositionChanged += Geolocator_PositionChanged;

            Routes.Clear();
            foreach (Route r in await TamuBusFeedApi.GetRoutes())
			{
                Routes.Add(r);
			}
        }

        private async void Geolocator_PositionChanged(Windows.Devices.Geolocation.Geolocator sender, Windows.Devices.Geolocation.PositionChangedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                MapGraphics.Graphics.Clear();

                var stopPoint = CreateRouteStop(
                    args.Position.Coordinate.Point.Position.Latitude,
                    args.Position.Coordinate.Point.Position.Longitude,
                    System.Drawing.Color.Red
                );
                MapGraphics.Graphics.Add(stopPoint);
            });
        }

        public void LoadMap(double lat, double lon)
        {
            MainMapView.Map = new Map(
                BasemapType.ImageryWithLabels,
                lat,
                lon,
                12
            );

			// Now draw a point where the stop is
			var stopPoint = CreateRouteStop(lat, lon, System.Drawing.Color.Red);
			MapGraphics.Graphics.Add(stopPoint);

			// Display all buildings
			var buildingsAUri = new Uri("https://gis.tamu.edu/arcgis/rest/services/FCOR/TAMU_BaseMap/MapServer/2");
			var buildingsALayer = new FeatureLayer(new ServiceFeatureTable(buildingsAUri));
			MainMapView.Map.OperationalLayers.Add(buildingsALayer);
			var buildingsBUri = new Uri("https://gis.tamu.edu/arcgis/rest/services/FCOR/TAMU_BaseMap/MapServer/3");
			var buildingsBLayer = new FeatureLayer(new ServiceFeatureTable(buildingsBUri));
			MainMapView.Map.OperationalLayers.Add(buildingsBLayer);
		}

        private Graphic CreateRouteStop(double lat, double lon, System.Drawing.Color fill)
        {
            // Now draw a point where the stop is
            var mapPoint = new MapPoint(Convert.ToDouble(lon),
                Convert.ToDouble(lat), SpatialReferences.Wgs84);
            var pointSymbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, fill, 20);
            pointSymbol.Outline = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.White, 5);
            return new Graphic(mapPoint, pointSymbol);
        }
        private Graphic CreateInactiveRouteStop(double lat, double lon)
        {
            return CreateRouteStop(lat, lon, System.Drawing.Color.Black);
        }
    }
}
