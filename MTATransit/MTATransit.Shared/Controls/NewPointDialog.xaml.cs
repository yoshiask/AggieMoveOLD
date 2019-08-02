using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System.Linq;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MTATransit.Shared.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewPointDialog : Page
    {
        #region Window Properties
        public string Title { get; set; }
        public string PrimaryButtonText { get; set; }
        public string SecondaryButtonText { get; set; }
        public Visibility SecondaryButtonVisibility { get; set; }

        Models.PointModel _model = null;
        public Models.PointModel Model {
            get {
                return _model;
            }
            set {
                _model = value;
                if (Model.HasArrivalTime)
                {
                    EnableArrivalBox.IsChecked = true;
                    ArrivalDatePicker.Date = Model.ArrivalDateTime.Value;
                    ArrivalTimePicker.Time = Model.ArrivalDateTime.Value.TimeOfDay;
                }
                if (Model.HasDepartureTime)
                {
                    EnableDepartureBox.IsChecked = true;
                    DepartureDatePicker.Date = Model.DepartureDateTime.Value;
                    DepartureTimePicker.Time = Model.DepartureDateTime.Value.TimeOfDay;
                }
                //AddressBox.Text = Model.Address;
                AddressBox.Text = Model.Title;
                AddressSearchQuery = Model.Address;
            }
        }

        public NewPointDialogResult Result { get; internal set; } = new NewPointDialogResult();

        public delegate void DialogClosedHandler(NewPointDialogResult r);
        public event DialogClosedHandler OnDialogClosed;
        #endregion

        #region Constructors
        public NewPointDialog(string title, bool isCancellable = false)
        {
            InitializeComponent();

            Title = title;
            PrimaryButtonText = "Done";
            if (isCancellable)
                SecondaryButtonText = "Cancel";
            else
                SecondaryButtonVisibility = Visibility.Collapsed;
        }

        public NewPointDialog(string title, string buttonText, bool isCancellable = false)
        {
            InitializeComponent();

            Title = title;
            PrimaryButtonText = buttonText;
            if (isCancellable)
                SecondaryButtonText = "Cancel";
            else
                SecondaryButtonVisibility = Visibility.Collapsed;
        }

        public NewPointDialog(string title, string primaryButtonText, string secondaryButtonText)
        {
            InitializeComponent();

            Title = title;
            PrimaryButtonText = primaryButtonText;
            SecondaryButtonText = secondaryButtonText;
            SecondaryButtonVisibility = Visibility.Visible;
        }


        public NewPointDialog(string title, Models.PointModel model, bool isCancellable = false)
        {
            InitializeComponent();

            Title = title;
            Model = model;
            PrimaryButtonText = "Done";
            if (isCancellable)
                SecondaryButtonText = "Cancel";
            else
                SecondaryButtonVisibility = Visibility.Collapsed;
        }

        public NewPointDialog(string title, Models.PointModel model, string primaryButtonText, string secondaryButtonText)
        {
            InitializeComponent();

            Title = title;
            Model = model;
            PrimaryButtonText = primaryButtonText;
            SecondaryButtonText = secondaryButtonText;
            SecondaryButtonVisibility = Visibility.Visible;
        }
        #endregion

        #region Properties
        private string AddressSearchQuery { get; set; }
        private API.ArcGIS.Suggestion Suggestion { get; set; }
        private List<API.ArcGIS.Suggestion> Suggestions { get; set; } = new List<API.ArcGIS.Suggestion>();

        private readonly string NoResultsString = "No results";
        #endregion

        #region Events
        private async void PrimaryButton_Click(object sender, RoutedEventArgs args)
        {
            CloseProgress.Visibility = Visibility.Visible;
            AddressBox.IsEnabled = false;
            EnableArrivalBox.IsEnabled = false;
            EnableDepartureBox.IsEnabled = false;
            ArrivalDatePicker.IsEnabled = false;
            ArrivalTimePicker.IsEnabled = false;
            DepartureDatePicker.IsEnabled = false;
            DepartureTimePicker.IsEnabled = false;
            PrimaryButton.IsEnabled = false;
            SecondaryButton.IsEnabled = false;

            var model = new Models.PointModel();
            if (EnableArrivalBox.IsChecked.Value)
            {
                if (ArrivalTimePicker.Time == new TimeSpan())
                    ArrivalTimePicker.Time = new TimeSpan(12, 0, 0);
                if (!ArrivalDatePicker.Date.HasValue)
                    ArrivalDatePicker.Date = DateTime.Now;

                var date = ArrivalDatePicker.Date.Value;
                date = date.AddHours(-date.Hour); date = date.AddMinutes(-date.Minute); date = date.AddSeconds(-date.Second);
                var time = ArrivalTimePicker.Time;
                var arr = date.Add(time); ;

                model.ArrivalTime = arr.ToUniversalTime().ToUnixTimeSeconds();
            }
            if (EnableDepartureBox.IsChecked.Value)
            {
                if (DepartureTimePicker.Time == new TimeSpan())
                    DepartureTimePicker.Time = new TimeSpan(12, 0, 0);
                if (!DepartureDatePicker.Date.HasValue)
                    DepartureDatePicker.Date = DateTime.Now;

                var date = DepartureDatePicker.Date.Value;
                date = date.AddHours(-date.Hour); date = date.AddMinutes(-date.Minute); date = date.AddSeconds(-date.Second);
                var time = DepartureTimePicker.Time;
                var arr = date.Add(time); ;

                model.DepartureTime = arr.ToUniversalTime().ToUnixTimeSeconds();
            }
            model.Address = AddressBox.Text;
            API.ArcGIS.AddressCandidate geocode;
            if (Suggestion == null)
            {
                Suggestion = await GetDefaultSuggestion(AddressSearchQuery);
                if (Suggestion == null)
                    Frame.Navigate(
                        typeof(Pages.FatalErrorPage),
                        new Pages.FatalErrorPage.FatalErrorArgs()
                        {
                            Icon = "\uE774",
                            Message = "Failed to get any suggestions based on the user's address query",
                            Exception = new ArgumentNullException("Suggestion", "Failed to get any suggestions based on the user's address query")
                        }
                    );
            }
            geocode = (await Common.ArcGISApi.Geocode(model.Address, Suggestion.MagicKey)).Candidates[0];
            model.Title = geocode.Address;
            model.Latitude = Convert.ToDecimal(geocode.Location.Latitude);
            model.Longitude = Convert.ToDecimal(geocode.Location.Longitude);
            Result.Result = DialogResult.Primary;
            Result.Model = model;
            OnDialogClosed?.Invoke(Result);
        }
        private void SecondaryButton_Click(object sender, RoutedEventArgs args)
        {
            CloseProgress.Visibility = Visibility.Visible;
            AddressBox.IsEnabled = false;
            EnableArrivalBox.IsEnabled = false;
            EnableDepartureBox.IsEnabled = false;
            ArrivalDatePicker.IsEnabled = false;
            ArrivalTimePicker.IsEnabled = false;
            DepartureDatePicker.IsEnabled = false;
            DepartureTimePicker.IsEnabled = false;
            PrimaryButton.IsEnabled = false;
            SecondaryButton.IsEnabled = false;

            Result.Result = DialogResult.Secondary;
            Result.Model = null;
            OnDialogClosed?.Invoke(Result);
        }

        #region AddressBox
        private async void AddressBox_TextChanged(AutoSuggestBox s, AutoSuggestBoxTextChangedEventArgs a)
        {
            if (a.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                try
                {
                    AddressSearchQuery = s.Text;
                    var suggs = await Common.ArcGISApi.GetSuggestions(s.Text);
                    if (suggs != null && suggs.Items != null && suggs.Items.Count > 0)
                    {
                        List<string> SuggestList = new List<string>();
                        Suggestions.Clear();
                        Suggestions = suggs.Items;
                        foreach (API.ArcGIS.Suggestion sugg in suggs.Items)
                        {
                            SuggestList.Add(sugg.Text);
                        }
                        s.ItemsSource = SuggestList;
                    }
                    else
                    {
                        s.ItemsSource = new string[] { NoResultsString };
                    }
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    // TODO: Handle what's likely an HTTP timeout
                    Debug.WriteLine(ex);
                }
            }
        }

        private async void AddressBox_QuerySubmitted(AutoSuggestBox s, AutoSuggestBoxQuerySubmittedEventArgs a)
        {
            if (a.ChosenSuggestion != null)
            {
                // The user selected a suggestion, show them the results
                Suggestion = Suggestions.Find(su => su.Text == (string)a.ChosenSuggestion);
            }
            else if (!string.IsNullOrEmpty(a.QueryText))
            {
                var sugg = await GetDefaultSuggestion(s.Text);
                if (sugg != null)
                    // A search has automatically been done, take the first result as if the user selected it
                    Suggestion = sugg;
                else
                    s.ItemsSource = new string[] { NoResultsString };
            }
        }

        private void AddressBox_SuggestionChosen(AutoSuggestBox s, AutoSuggestBoxSuggestionChosenEventArgs a)
        {
            // Here's where we autocomplete
            if ((string)a.SelectedItem != NoResultsString)
            {
                s.Text = (string)a.SelectedItem;
            }
        }
        #endregion

        #endregion

        private async Task<API.ArcGIS.Suggestion> GetDefaultSuggestion(string query)
        {
            var suggs = await Common.ArcGISApi.GetSuggestions(query);
            if (suggs != null && suggs.Items != null && suggs.Items.Count > 0)
                // A search has automatically been done, take the first result as if the user selected it
                return suggs.Items.FirstOrDefault();
            else
                return null;
        }

        public enum DialogResult
        {
            Primary,
            Secondary,
            ClosedByUser
        }
        public class NewPointDialogResult
        {
            public DialogResult Result { get; set; }
            public Models.PointModel Model { get; set; }
            public Models.PointModel OldModel { get; set; }
        }
        public class EditorControls
        {
            public static TextBox NumberBox(string label, string name = "NumberBox")
            {
                // TODO: Use the real NumberBox from WinUI
                var numBox = new TextBox()
                {
                    Name = name,
                    PlaceholderText = label,
                };
                TextBoxRegex.SetValidationMode(numBox, TextBoxRegex.ValidationMode.Dynamic);
                TextBoxRegex.SetValidationType(numBox, TextBoxRegex.ValidationType.Number);
                return numBox;
            }
        }
    }
}
