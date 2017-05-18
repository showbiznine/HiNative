﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HiNative.Services;
using HiNative.Views;
using HiNative.API.Models;
using HiNative.Services;
using Microsoft.Advertising.WinRT.UI;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Services.Store.Engagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Services.Store;
using Windows.Storage;
using Windows.UI.Popups;

namespace HiNative.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        #region Fields
        public INavigationService navigationService { get { return ServiceLocator.Current.GetInstance<INavigationService>(); } }
        public HNUserProfile CurrentUser { get; set; }

        InterstitialAd myInterstitialAd = null;
        string myAppId = "c7ead89e-1e8d-4728-a855-d805ceccd3c7";
        string myAdUnitId = "336474";
        public bool AdReady = false;

        private StoreContext context = null;
        #endregion

        #region Commands
        public RelayCommand GoToCurrentUserProfileCommand { get; set; }
        public RelayCommand GoToHomeCommand { get; set; }
        public RelayCommand GoToSettingsCommand { get; set; }
        public RelayCommand FrameNavigatedCommand { get; set; }
        public RelayCommand LeaveFeedbackCommand { get; set; }
        #endregion

        public EventHandler InterstitialAdCompleted { get; set; }
        public EventHandler InterstitialAdCanceled { get; set; }
        public EventHandler InterstitialAdError { get; set; }

        public ShellViewModel()
        {
            if (IsInDesignMode)
            {

            }
            else
            {
                //CheckForUpdates();
                var task = RegisterBackgroundTask("CheckNotificationsTask",
                    new TimeTrigger(15, false),
                    null);
                InitializeCommands();
                SetupAds();
            }
        }

        private async Task CheckForUpdates()
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }

            // Get the updates that are available.
            IReadOnlyList<StorePackageUpdate> updates =
                await context.GetAppAndOptionalStorePackageUpdatesAsync();

            if (updates.Count > 0)
            {
                // Alert the user that updates are available and ask for their consent
                // to start the updates.
                MessageDialog dialog = new MessageDialog(
                    "Download and install updates now? This may cause the application to exit.", "There's an update available!");
                dialog.Commands.Add(new UICommand("Yes"));
                dialog.Commands.Add(new UICommand("No"));
                IUICommand command = await dialog.ShowAsync();

                if (command.Label.Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                {
                    // Download and install the updates.
                    IAsyncOperationWithProgress<StorePackageUpdateResult, StorePackageUpdateStatus> downloadOperation =
                        context.RequestDownloadAndInstallStorePackageUpdatesAsync(updates);

                    // The Progress async method is called one time for each step in the download
                    // and installation process for each package in this request.
                    //downloadOperation.Progress = async (asyncInfo, progress) =>
                    //{
                    //    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                    //    () =>
                    //    {
                    //        downloadProgressBar.Value = progress.PackageDownloadProgress;
                    //    });
                    //};

                    StorePackageUpdateResult result = await downloadOperation.AsTask();
                }
            }
        }

        #region Ads
        private void SetupAds()
        {
            myInterstitialAd = new InterstitialAd();
            myInterstitialAd.AdReady += MyInterstitialAd_AdReady;
            myInterstitialAd.ErrorOccurred += MyInterstitialAd_ErrorOccurred;
            myInterstitialAd.Completed += MyInterstitialAd_Completed;
            myInterstitialAd.Cancelled += MyInterstitialAd_Cancelled;

            RequestAd();
        }

        public void RequestAd()
        {
            myInterstitialAd.RequestAd(AdType.Video, myAppId, myAdUnitId);
        }

        public bool ShowAd()
        {
            try
            {
                myInterstitialAd.Show();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void MyInterstitialAd_Completed(object sender, object e)
        {
            LoggerService.LogEvent("Interstitial_ad_watched");
            AdReady = false;
        }

        private void MyInterstitialAd_Cancelled(object sender, object e)
        {
            LoggerService.LogEvent("Interstitial_ad_canceled");
        }

        private void MyInterstitialAd_AdReady(object sender, object e)
        {
            AdReady = true;
        }

        private void MyInterstitialAd_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            LoggerService.LogEvent("Interstitial_ad_error");
        } 
        #endregion

        #region Background Task
        private async Task<BackgroundTaskRegistration> RegisterBackgroundTask(string name,
            TimeTrigger trigger,
            IBackgroundCondition condition)
        {
            var access = await BackgroundExecutionManager.RequestAccessAsync();
            //Check for existing registrations
            foreach (var _task in BackgroundTaskRegistration.AllTasks)
            {
                if (_task.Value.Name == name)
                {
                    //The task is already registered

                    //_task.Value.Unregister(true);
                    return (BackgroundTaskRegistration)(_task.Value);
                }
            }

            //Register the task

            var builder = new BackgroundTaskBuilder();
            await BackgroundExecutionManager.RequestAccessAsync();
            builder.Name = name;
            builder.SetTrigger(trigger);

            if (condition != null)
            {
                builder.AddCondition(condition);
            }
            try
            {
                BackgroundTaskRegistration task = builder.Register();
                return task;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        } 
        #endregion

        private void InitializeCommands()
        {
            GoToCurrentUserProfileCommand = new RelayCommand(async () =>
            {
                await App.ViewModelLocator.Profile.LoadUser(CurrentUser.user_attributes.id);
                navigationService.NavigateTo(typeof(ProfilePage));
            });
            GoToSettingsCommand = new RelayCommand(() => 
            {
                navigationService.NavigateTo(typeof(SettingsPage));
            });
            GoToHomeCommand = new RelayCommand(() => 
            {
                navigationService.NavigateTo(typeof(MainPage));
            });
            LeaveFeedbackCommand = new RelayCommand(async () =>
            {
                await LeaveFeedbackAsync();
            });
        }

        private async Task LeaveFeedbackAsync()
        {
            if (StoreServicesFeedbackLauncher.IsSupported())
            {
                var launcher = StoreServicesFeedbackLauncher.GetDefault();
                await launcher.LaunchAsync();
            }
            else
            {
                var msg = new MessageDialog("Why don't you send us an email instead",
                    "It looks like the feedback hub is having some trouble launching");
                msg.Commands.Add(new UICommand(
                    "Yes",
                    new UICommandInvokedHandler(SendFeedbackEmail)));
                msg.Commands.Add(new UICommand("No"));
            }
        }

        private async void SendFeedbackEmail(IUICommand command)
        {
            var success = await Windows.System.Launcher.LaunchUriAsync(new Uri("mailto:hinative_uwp@outlook.com"));
        }

        public async Task CheckLoggedIn()
        {
            try
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                var id = localSettings.Values["User_ID"];
                if (id != null)
                {
                    CurrentUser = await DataService.LoadProfile(Convert.ToInt32(id));
                    navigationService.NavigateTo(typeof(MainPage));
                }
                else
                    navigationService.NavigateTo(typeof(LoginPage));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await new MessageDialog("We're having trouble connecting to the HiNative servers").ShowAsync();
            }
        }
    }
}
