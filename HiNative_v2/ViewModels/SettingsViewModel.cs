using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HiNative.Services;
using HiNative.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HiNative.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Fields
        private INavigationService _navigationService { get { return ServiceLocator.Current.GetInstance<INavigationService>(); } }
        #endregion

        #region Commands
        public RelayCommand LogOutCommand { get; set; }
        #endregion

        public SettingsViewModel()
        {
            if (IsInDesignMode)
            {

            }
            else
            {
                InitializeCommands();
            }
        }

        private void InitializeCommands()
        {
            LogOutCommand = new RelayCommand(() =>
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["User_ID"] = null;
                _navigationService.NavigateTo(typeof(LoginPage));
            });
        }
    }
}
