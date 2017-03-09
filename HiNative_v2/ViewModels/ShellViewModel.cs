using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HiNative.Services;
using HiNative.Views;
using HiNativeShared.API.Models;
using HiNativeShared.Services;
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
                var task = RegisterBackgroundTask("CheckNotificationsTask",
                    new TimeTrigger(15, false),
                    null);
                IsMenuOpen = false;
                InitializeCommands();
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
            builder.TaskEntryPoint = taskEntryPoint;
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
            GoToCurrentUserProfileCommand = new RelayCommand(() =>
            {
                App.ViewModelLocator.Profile.LoadUser(CurrentUser.user_attributes.id);
                _navigationService.NavigateTo(typeof(ProfilePage));
            });
            GoToSettingsCommand = new RelayCommand(() => _navigationService.NavigateTo(typeof(SettingsPage)));
            GoToHomeCommand = new RelayCommand(() => 
            {
                _navigationService.NavigateTo(typeof(MainPage));
            });
            LeaveFeedbackCommand = new RelayCommand(() =>
            {
                LeaveFeedbackAsync();
            });
            FrameNavigatedCommand = new RelayCommand(() => IsMenuOpen = false);
        }

        private async void LeaveFeedbackAsync()
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
            }
        }
    }
}
