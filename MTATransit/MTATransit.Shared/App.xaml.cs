using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MTATransit
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
               // this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            bool isInternetAvailable = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            // Check if connected to internet
            if (isInternetAvailable)
            {
                // Initialize the ArcGIS environment
                Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.SetLicense(
                    "runtimelite,1000,rud5976483922,none,GB2PMD17J06HZF3RE159"
                );
            }
            else
            {
                //App.Current.Exit();
            }

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    if (!isInternetAvailable)
                        rootFrame.Navigate(
                            typeof(Shared.Pages.FatalErrorPage),
                            new Shared.Pages.FatalErrorPage.FatalErrorArgs()
                            {
                                //Icon = "\uE774",
                                //SecondaryIcon = "\uEA39",
                                Icon = "\uEB5E",
                                Message = "You are not connected to the internet"
                            });
                    else
                    {
                        var dialog = new Shared.Controls.DialogBox(
                            "Heads up!",
                            @"This app is still in development.
Many bugs are present, and it is likely
that you will find one. If the app crashes
during this demonstration, please reopen
the ""LA Move"" app from the taskbar.
Thank you!"
                        );
                        dialog.OnDialogClosed += (Shared.Controls.DialogBox.DialogResult result) =>
                        {
                            rootFrame.Navigate(typeof(MainPage), e.Arguments);
                        };

                        rootFrame.Navigate(typeof(Shared.Pages.DialogPage), dialog);

                        // When the navigation stack isn't restored navigate to the first page,
                        // configuring the new page by passing required information as a navigation
                        // parameter
                        //rootFrame.Navigate(typeof(Shared.Pages.NavigateHomePage), e.Arguments);
                    }
                    
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            (Window.Current.Content as Frame).Navigate(
                            typeof(Shared.Pages.FatalErrorPage),
                            new Shared.Pages.FatalErrorPage.FatalErrorArgs()
                            {
                                //Icon = "\uE774",
                                //SecondaryIcon = "\uEA39",
                                Icon = "\uEB5E",
                                Message = "Failed to naviagte to " + e.SourcePageType.FullName
                            });

            //throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
