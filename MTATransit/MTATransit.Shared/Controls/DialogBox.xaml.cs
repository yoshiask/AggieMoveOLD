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
    public sealed partial class DialogBox : UserControl
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string PrimaryButtonText { get; set; }
        public string SecondaryButtonText { get; set; }
        public Visibility SecondaryButtonVisibility { get; set; }

        public DialogResult Result { get; internal set; }

        public DialogBox(string title, string message, bool isCancellable = false)
        {
            this.InitializeComponent();

            Title = title;
            Message = message;
            PrimaryButtonText = "OK";
            if (isCancellable)
            {
                SecondaryButtonText = "Cancel";
                SecondaryButtonVisibility = Visibility.Visible;
            }
            else
            {
                SecondaryButtonVisibility = Visibility.Collapsed;
            }
        }

        public DialogBox(string title, string message, string buttonText)
        {
            this.InitializeComponent();

            Title = title;
            Message = message;
            PrimaryButtonText = buttonText;
            SecondaryButtonVisibility = Visibility.Collapsed;
        }

        public DialogBox(string title, string message, string primaryButtonText, string secondaryButtonText)
        {
            this.InitializeComponent();

            Title = title;
            Message = message;
            PrimaryButtonText = primaryButtonText;
            SecondaryButtonText = secondaryButtonText;
            SecondaryButtonVisibility = Visibility.Visible;
        }


        public delegate void DialogClosedHandler(DialogResult r);
        public event DialogClosedHandler OnDialogClosed;

        private void PrimaryButton_Click(object sender, RoutedEventArgs args)
        {
            Result = DialogResult.Primary;
            //this.Visibility = Visibility.Collapsed;
            OnDialogClosed?.Invoke(Result);
        }
        private void SecondaryButton_Click(object sender, RoutedEventArgs args)
        {
            Result = DialogResult.Secondary;
            //this.Visibility = Visibility.Collapsed;
            OnDialogClosed?.Invoke(Result);
        }

        public enum DialogResult
        {
            Primary,
            Secondary,
            ClosedByUser
        }
    }
}
