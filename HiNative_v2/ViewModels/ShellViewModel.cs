using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HiNative.Services;
using HiNative.Views;
using HiNativeShared.API.Models;
using HiNativeShared.Services;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

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
        #endregion

        public ShellViewModel()
        {
            if (IsInDesignMode)
            {

            }
            else
            {
                var task = RegisterBackgroundTask("HiNativeBG.BGTask",
                    "BGTask",
                    new TimeTrigger(15, false),
                    null);
                IsMenuOpen = false;
                InitializeCommands();
            }
        }

        #region Background Task
        private async Task<BackgroundTaskRegistration> RegisterBackgroundTask(string taskEntryPoint,
    string name,
    TimeTrigger trigger,
    IBackgroundCondition condition)
        {
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
            BackgroundTaskRegistration task = builder.Register();
            return task;
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
            FrameNavigatedCommand = new RelayCommand(() => IsMenuOpen = false);
        }

        public async Task CheckLoggedIn()
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
    }
}
