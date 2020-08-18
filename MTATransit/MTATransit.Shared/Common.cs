using MTATransit.Shared.API.ArcGIS;
using Refit;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using MTATransit.Shared.API.RestBus;
using System.Diagnostics;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI.Xaml.Data;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Newtonsoft.Json;
using FontFamily = Windows.UI.Xaml.Media.FontFamily;

namespace MTATransit.Shared
{
	public static class Common
	{
		#region API Initializers
		// Initialize the services that we're requesting from
		public static IRestBusApi RestBusApi
		{
			get
			{
				return RestService.For<IRestBusApi>("http://restbus.info/api");
			}
		}
		public static IArcGISApi ArcGISApi
		{
			get
			{
				return RestService.For<IArcGISApi>("http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/");
			}
		}
		public static API.MTA.IMTAApi MTAApi
		{
			get
			{
				return RestService.For<API.MTA.IMTAApi>("http://api.metro.net/");
			}
		}
		public static API.LAMove.ILAMoveApi LAMoveApi
		{
			get
			{
				return RestService.For<API.LAMove.ILAMoveApi>("http://lamove-api.herokuapp.com/v1");
			}
		}
		public static API.OTP.IOTPApi OTPApi
		{
			get
			{
				return RestService.For<API.OTP.IOTPApi>("http://localhost:8080/otp");
			}
		}
		public static API.Yelp.IYelpApi YelpApi
		{
			get
			{
				return RestService.For<API.Yelp.IYelpApi>("https://api.yelp.com/v3");
			}
		}
		#endregion

		public static FontFamily DINFont = new FontFamily("/Assets/Fonts/DINRegular#DIN");
		public static FontFamily LAMoveIconFont = new FontFamily("/Assets/Fonts/LA-Move-Icons#LA-Move-Icons");

		/// <summary>
		/// Creates Color from HEX code
		/// </summary>
		/// <param name="hex">HEX code string</param>
		public static Windows.UI.Color ColorFromHex(string hex)
		{
			hex = hex.Replace("#", string.Empty);
			byte r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
			byte g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
			byte b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
			return Windows.UI.Color.FromArgb(255, r, g, b);
		}

		/// <summary>
		/// Creates Color from HEX code
		/// </summary>
		/// <param name="hex">HEX code string</param>
		public static System.Drawing.Color DrawingColorFromHex(string hex)
		{
			hex = hex.Replace("#", string.Empty);
			byte r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
			byte g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
			byte b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
			return System.Drawing.Color.FromArgb(255, r, g, b);
		}

		/// <summary>
		/// Creates Brush from HEX code
		/// </summary>
		/// <param name="hex">HEX code string</param>
		/// <returns></returns>
		public static SolidColorBrush BrushFromHex(string hex)
		{
			return new SolidColorBrush(ColorFromHex(hex));
		}

		/// <summary>
		/// Figures out whether the color is dark or light using luminance
		/// </summary>
		/// <param name="hex"></param>
		/// <returns></returns>
		public static ElementTheme ThemeFromColor(string hex)
		{
			var color = ColorFromHex(hex);
			if (color.R * 0.2126 + color.G * 0.7152 + color.B * 0.0722 > 255 / 2)
				return ElementTheme.Dark;
			else
				return ElementTheme.Light;
		}

		public static Windows.UI.Color ConvertColor(System.Drawing.Color draw)
		{
			return Windows.UI.Color.FromArgb(draw.A, draw.R, draw.G, draw.B);
		}
		public static System.Drawing.Color ConvertColor(Windows.UI.Color ui)
		{
			return System.Drawing.Color.FromArgb(ui.A, ui.R, ui.G, ui.B);
		}

		public static class SpatialHelper
		{
			public static double GetDistance(double lat1, double lon1, double lat2, double lon2)
			{
				var R = 6371; // Radius of the earth in km
				var dLat = ToRadians(lat2 - lat1);  // deg2rad below
				var dLon = ToRadians(lon2 - lon1);
				var a =
					Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
					Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
					Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

				var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
				var d = R * c; // Distance in km
				return d;
			}

			public static double ToRadians(double deg)
			{
				return deg * (Math.PI / 180);
			}

			public static bool IsWithinRadius(double lat, double lon, double x, double y, double radius)
			{
				return GetDistance(lat, lon, x, y) <= radius;
			}

			private static Models.PointModel CurrentLocationCache { get; set; }
			private static DateTime? CurrentLocationLastUpdated { get; set; }
			public static Models.PointModel GetCachedLocation()
			{
				return CurrentLocationCache;
			}
			public async static Task<Models.PointModel> GetCurrentLocation(bool acceptCache = true, uint accuracy = 1)
			{
				if (CurrentLocationCache == null || !CurrentLocationLastUpdated.HasValue
					|| DateTime.Now.Subtract(CurrentLocationLastUpdated.Value).TotalMinutes > 5
					|| !acceptCache)
				{
					// Cache needs to be updated
					var accessStatus = await Geolocator.RequestAccessAsync();
					if (accessStatus == GeolocationAccessStatus.Allowed)
					{
						CurrentLocationCache = new Models.PointModel();
						Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = accuracy };
						Geoposition pos = await geolocator.GetGeopositionAsync();
						CurrentLocationCache.Title = "Your Location";
						CurrentLocationCache.IsCurrentLocation = true;
						CurrentLocationCache.Latitude = Convert.ToDecimal(pos.Coordinate.Point.Position.Latitude);
						CurrentLocationCache.Longitude = Convert.ToDecimal(pos.Coordinate.Point.Position.Longitude);
						CurrentLocationCache.Geolocator = geolocator;
						CurrentLocationCache.Address = $"{CurrentLocationCache.Longitude}, {CurrentLocationCache.Latitude}";

						Debug.WriteLine($"Current location: {CurrentLocationCache.Address}");
					}
				}

				return CurrentLocationCache;
			}
		}

		public static class NumberHelper
		{
			public static string ToShortTimeString(long seconds)
			{
				if (seconds < 60)
					return "Now";

				int minutes = Convert.ToInt32(seconds / 60);
				int hours = Convert.ToInt32(minutes / 60);
				minutes -= (hours * 60);

				if (hours <= 0)
					return $"{minutes.ToString()} mins";
				else
					return $"{hours.ToString()} hrs, {minutes.ToString()} mins";
			}

			/// <summary>
			/// Takes Unix time (seconds) and returns something like 2:00 AM
			/// </summary>
			/// <param name="seconds"></param>
			/// <returns></returns>
			public static string ToShortDayTimeString(double unixTimeStamp)
			{
				return UnixTimeStampToDateTime(unixTimeStamp).ToString("hh:mm tt");
			}
			/// <summary>
			/// Takes Unix time (seconds) and returns something like 2:00 AM
			/// </summary>
			/// <param name="seconds"></param>
			/// <returns></returns>
			public static string ToShortDayTimeString(long unixTimeStamp)
			{
				return UnixTimeStampToDateTime(unixTimeStamp).ToString("hh:mm tt");
			}

			public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
			{
				// Unix timestamp is seconds past epoch
				DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
				dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
				return dtDateTime;
			}
			public static DateTime UnixTimeStampToDateTime(long unixTimeStamp, bool isLocal = false)
			{
				// Unix timestamp is seconds past epoch
				DateTime dtDateTime = new DateTime(
					1970, 1, 1, 0, 0, 0, 0,
					isLocal ? DateTimeKind.Local : DateTimeKind.Utc
				);
				dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
				return dtDateTime;
			}

			public static double MetersToMiles(double meters)
			{
				double kilometers = meters / 1000;
				double miles = kilometers * 0.621371;
				return miles;
			}
			public static double MilesToMeters(double miles)
			{
				double kilometers = miles / 0.621371;
				double meters = kilometers * 1000;
				return meters;
			}

			/// <summary>
			/// Returns a string that represents U.S. dollars. Formatted like ~$2.31
			/// </summary>
			public static string ToShortCurrencyString(decimal amount, bool isApprox = false)
			{
				string output = "";
				if (isApprox)
					output += "~";
				output += String.Format("{0:C}", amount);
				return output;
			}
		}

		public static class RoamingSettings
		{
			#region Files
			private static readonly StorageFolder roamingFolder = ApplicationData.Current.RoamingFolder;
			private static readonly ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;

			private const string SavedLocationsPath = "savedLocations.json";
			private const string SavedRoutesPath = "savedRoutes.json";
			#endregion

			public static async void SetUpRoamingSettings()
			{
				if (!await roamingFolder.FileExistsAsync(SavedLocationsPath))
				{
					StorageFile locationsFile = await roamingFolder.CreateFileAsync(SavedLocationsPath,
					CreationCollisionOption.OpenIfExists);
					await FileIO.WriteTextAsync(locationsFile,
						JsonConvert.SerializeObject(new Dictionary<string, Location>())
					);
				}

				if (!await roamingFolder.FileExistsAsync(SavedRoutesPath))
				{

					StorageFile routesFile = await roamingFolder.CreateFileAsync(SavedRoutesPath,
					CreationCollisionOption.OpenIfExists);
					await FileIO.WriteTextAsync(routesFile,
						JsonConvert.SerializeObject(new Dictionary<string, API.OTP.PlanRequestParameters>())
					);
				}
			}

			public static async void SetLocation(string name, decimal lon, decimal lat)
			{
				// Get the file and deserialize it
				StorageFile locationsFile = await roamingFolder.GetFileAsync(SavedLocationsPath);
				var savedLocations = JsonConvert.DeserializeObject<Dictionary<string, Location>>(await FileIO.ReadTextAsync(locationsFile));

				var location = new Location()
				{
					Latitude = Convert.ToDouble(lat),
					Longitude = Convert.ToDouble(lon)
				};
				if (savedLocations.ContainsKey(name))
					savedLocations[name] = location;
				else
					savedLocations.Add(name, location);

				// Serialized the edited file
				await FileIO.WriteTextAsync(locationsFile,
					JsonConvert.SerializeObject(savedLocations)
				);
			}
			public static async void SetRoute(string name, API.OTP.PlanRequestParameters planRequest)
			{
				// Get the file and deserialize it
				StorageFile routesFile = await roamingFolder.GetFileAsync(SavedRoutesPath);
				var savedRoutes = JsonConvert.DeserializeObject<Dictionary<string, API.OTP.PlanRequestParameters>>(await FileIO.ReadTextAsync(routesFile));

				if (savedRoutes.ContainsKey(name))
					savedRoutes[name] = planRequest;
				else
					savedRoutes.Add(name, planRequest);

				// Serialized the edited file
				await FileIO.WriteTextAsync(routesFile,
					JsonConvert.SerializeObject(savedRoutes)
				);
			}

			public static async Task<Location> GetLocation(string name)
			{
				var savedLocations = await GetAllLocations();

				if (savedLocations.ContainsKey(name))
					return savedLocations[name];
				else
					return null;
			}
			public static async Task<Models.PointModel> GetLocationAsPoint(string name)
			{
				var loc = await GetLocation(name);
				return new Models.PointModel()
				{
					Title = name,
					Latitude = Convert.ToDecimal(loc.Latitude),
					Longitude = Convert.ToDecimal(loc.Longitude)
				};
			}
			public static async Task<API.OTP.PlanRequestParameters> GetRoute(string name)
			{
				var savedRoutes = await GetAllRoutes();

				if (savedRoutes.ContainsKey(name))
					return savedRoutes[name];
				else
					return null;
			}

			public static async void DeleteLocation(string name)
			{
				// Get the file and deserialize it
				StorageFile locationsFile = await roamingFolder.GetFileAsync(SavedLocationsPath);
				var savedLocations = JsonConvert.DeserializeObject<Dictionary<string, Location>>(await FileIO.ReadTextAsync(locationsFile));
				savedLocations.Remove(name);

				// Serialized the edited file
				await FileIO.WriteTextAsync(locationsFile,
					JsonConvert.SerializeObject(savedLocations)
				);
			}
			public static async void DeleteRoute(string name)
			{
				// Get the file and deserialize it
				StorageFile routesFile = await roamingFolder.GetFileAsync(SavedRoutesPath);
				var savedRoutes = JsonConvert.DeserializeObject<Dictionary<string, API.OTP.PlanRequestParameters>>(await FileIO.ReadTextAsync(routesFile));
				savedRoutes.Remove("name");

				// Serialized the edited file
				await FileIO.WriteTextAsync(routesFile,
					JsonConvert.SerializeObject(savedRoutes)
				);
			}

			public static async void ReplaceLocation(string oldName, string newName, decimal lon, decimal lat)
			{
				// Get the file and deserialize it
				StorageFile locationsFile = await roamingFolder.GetFileAsync(SavedLocationsPath);
				var savedLocations = JsonConvert.DeserializeObject<Dictionary<string, Location>>(await FileIO.ReadTextAsync(locationsFile));

				savedLocations.Remove(oldName);

				var location = new Location()
				{
					Latitude = Convert.ToDouble(lat),
					Longitude = Convert.ToDouble(lon)
				};
				if (savedLocations.ContainsKey(newName))
					savedLocations[newName] = location;
				else
					savedLocations.Add(newName, location);

				// Serialized the edited file
				await FileIO.WriteTextAsync(locationsFile,
					JsonConvert.SerializeObject(savedLocations)
				);
			}
			public static async void ReplaceRoute(string oldName, string newName, API.OTP.PlanRequestParameters planRequest)
			{
				// Get the file and deserialize it
				StorageFile routesFile = await roamingFolder.GetFileAsync(SavedRoutesPath);
				var savedRoutes = JsonConvert.DeserializeObject<Dictionary<string, API.OTP.PlanRequestParameters>>(await FileIO.ReadTextAsync(routesFile));

				savedRoutes.Remove(oldName);

				if (savedRoutes.ContainsKey(newName))
					savedRoutes[newName] = planRequest;
				else
					savedRoutes.Add(newName, planRequest);

				// Serialized the edited file
				await FileIO.WriteTextAsync(routesFile,
					JsonConvert.SerializeObject(savedRoutes)
				);
			}

			public static async Task<Dictionary<string, Location>> GetAllLocations()
			{
				// Get the file and deserialize it
				StorageFile locationsFile = await roamingFolder.GetFileAsync(SavedLocationsPath);
				return JsonConvert.DeserializeObject<Dictionary<string, Location>>(await FileIO.ReadTextAsync(locationsFile));
			}
			public static async Task<Dictionary<string, API.OTP.PlanRequestParameters>> GetAllRoutes()
			{
				// Get the file and deserialize it
				StorageFile routesFile = await roamingFolder.GetFileAsync(SavedRoutesPath);
				return JsonConvert.DeserializeObject<Dictionary<string, API.OTP.PlanRequestParameters>>(await FileIO.ReadTextAsync(routesFile));
			}
		}

		public static void WatchForAlerts()
		{
			var stream = Tweetinvi.Stream.CreateFilteredStream();
			stream.AddTrack("tweetinvi");
			stream.MatchingTweetReceived += (sender, args) =>
			{
				Console.WriteLine("A tweet containing 'tweetinvi' has been found; the tweet is '" + args.Tweet + "'");
			};
			stream.StartStreamMatchingAllConditions();
		}
	}

	public class XamlConverters
	{
		public class VisibleWhenZeroConverter : IValueConverter
		{
			public object Convert(object v, Type t, object p, string l) =>
				Equals(0d, (double)v) ? Visibility.Visible : Visibility.Collapsed;

			public object ConvertBack(object v, Type t, object p, string l) => null;
		}
	}
}
