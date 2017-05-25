using GalaSoft.MvvmLight;
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
        private INavigationService _navigationService { get { return ServiceLocator.Current.GetInstance<INavigationService>(); } }
        public HNUserProfile CurrentUser { get; set; }
        public bool IsMenuOpen { get; set; }

        private StoreContext context = null;
        #endregion

        #region Commands
        public RelayCommand GoToCurrentUserProfileCommand { get; set; }
        public RelayCommand GoToHomeCommand { get; set; }
        public RelayCommand GoToSettingsCommand { get; set; }
        public RelayCommand FrameNavigatedCommand { get; set; }
        public RelayCommand LeaveFeedbackCommand { get; set; }
        #endregion

        public ShellViewModel()
        {
            if (IsInDesignMode)
            {

            }
            else
            {
                CheckForUpdates();
                var task = RegisterBackgroundTask("CheckNotificationsTask",
                    new TimeTrigger(15, false),
                    null);
                IsMenuOpen = false;
                InitializeCommands();
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
                _navigationService.NavigateTo(typeof(ProfilePage));
            });
            GoToSettingsCommand = new RelayCommand(() => 
            {
                _navigationService.NavigateTo(typeof(SettingsPage));
            });
            GoToHomeCommand = new RelayCommand(() => 
            {
                _navigationService.NavigateTo(typeof(MainPage));
            });
            LeaveFeedbackCommand = new RelayCommand(async () =>
            {
                await LeaveFeedbackAsync();
            });
            FrameNavigatedCommand = new RelayCommand(() => IsMenuOpen = false);
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
                    _navigationService.NavigateTo(typeof(MainPage));
                }
                else
                    _navigationService.NavigateTo(typeof(LoginPage));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await new MessageDialog("We're having trouble connecting to the HiNative servers").ShowAsync();
            }
        }
    }
}
