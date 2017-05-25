using GalaSoft.MvvmLight.Threading;
using HiNative.Services;
using HiNative.ViewModels;
using HiNative.Views;
using Microsoft.HockeyApp;
using Microsoft.Services.Store.Engagement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HiNative
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        public static ViewModelLocator ViewModelLocator;
        public static StoreServicesCustomEventLogger StoreLogger;

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
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            StoreLogger = StoreServicesCustomEventLogger.GetDefault();
            Debug.WriteLine(this.RequestedTheme);

            #region Title/Status bar
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var sb = StatusBar.GetForCurrentView();
                var c = (SolidColorBrush)Current.Resources["TopBarBG"];
                sb.BackgroundColor = c.Color;
                sb.BackgroundOpacity = 100;
            }
            else
            {
                var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;

                // set up our brushes
                var blue = (SolidColorBrush)Current.Resources["TopBarBG"];
                var hoverF = (Color)Current.Resources["SystemBaseMediumHighColor"];
                var hoverB = (Color)Current.Resources["SystemAltLowColor"];

                // override colors!
                titleBar.BackgroundColor = blue.Color;
                titleBar.ForegroundColor = Colors.White;
                titleBar.ButtonBackgroundColor = blue.Color;
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonHoverBackgroundColor = hoverB;
                titleBar.ButtonHoverForegroundColor = hoverF;
                titleBar.ButtonPressedBackgroundColor = blue.Color;
                titleBar.ButtonPressedForegroundColor = Colors.Black;
                titleBar.InactiveBackgroundColor = blue.Color;
                titleBar.InactiveForegroundColor = hoverF;
                titleBar.ButtonInactiveBackgroundColor = blue.Color;
                titleBar.ButtonInactiveForegroundColor = hoverF;
            }
            #endregion

            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = false;

            HockeyClient.Current.Configure("8de5fa42d2b445baaf5498c6cc23638b ");
            await SetupStoreAsync();

            Frame rootFrame = Window.Current.Content as Frame;

            DispatcherHelper.Initialize();
            ViewModelLocator = (ViewModelLocator)Current.Resources["Locator"];

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
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(Shell), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        private async Task SetupStoreAsync()
        {
            StoreServicesEngagementManager engagementManager = StoreServicesEngagementManager.GetDefault();
            await engagementManager.RegisterNotificationChannelAsync();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            if (args is ToastNotificationActivatedEventArgs)
            {
                var toastActivationArgs = args as ToastNotificationActivatedEventArgs;

                StoreServicesEngagementManager engagementManager = StoreServicesEngagementManager.GetDefault();
                string originalArgs = engagementManager.ParseArgumentsAndTrackAppLaunch(
                    toastActivationArgs.Argument);

                // Use the originalArgs variable to access the original arguments
                // that were passed to the app.
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
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

        protected override async void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);

            var taskName = args.TaskInstance.Task.Name;

            switch (taskName)
            {
                case "CheckNotificationsTask":
                    await BackgroundService.CheckNotificationsTask(args.TaskInstance);
                    break;
                default:
                    break;
            }
        }
    }
}
