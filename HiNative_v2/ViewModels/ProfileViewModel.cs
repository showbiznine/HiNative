using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HiNative.Services;
using HiNative.Views;
using HiNative.API.Models;
using HiNative.Services;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using static HiNative.ViewModels.ProfileSearchViewModel;

namespace HiNative.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        #region Fields
        private INavigationService _navigationService { get { return ServiceLocator.Current.GetInstance<INavigationService>(); } }
        public HNUserProfile User { get; set; }
        public string UserName { get; set; }
        public BitmapImage ProfilePicture { get; set; }
        public bool InCall { get; set; }
        public bool CanEdit { get; set; }
        #endregion

        #region Commands
        public RelayCommand SearchUserQuestions { get; set; }
        public RelayCommand SearchUserAnswers { get; set; }
        public RelayCommand SearchUserFeaturedAnswers { get; set; }
        public RelayCommand SearchUserLiked { get; set; }
        public RelayCommand SearchUserQuickResponses { get; set; }
        #endregion

        public ProfileViewModel()
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
            SearchUserAnswers = new RelayCommand(() => SearchProfile(ProfileSearchType.answers));
            SearchUserQuestions = new RelayCommand(() => SearchProfile(ProfileSearchType.questions));
            SearchUserLiked = new RelayCommand(() => SearchProfile(ProfileSearchType.likes));
            SearchUserQuickResponses = new RelayCommand(() => SearchProfile(ProfileSearchType.quick_responses));
            SearchUserFeaturedAnswers = new RelayCommand(() => SearchProfile(ProfileSearchType.featured_answers));
        }

        public async void LoadUser(int uID)
        {
            CanEdit = false;
            User = new HNUserProfile();
            User = await DataService.LoadProfile(uID);
            if (UserName != User.user_attributes.name)
                UserName = User.user_attributes.name;
            CanEdit = User.user_attributes.id == App.ViewModelLocator.Shell.CurrentUser.user_attributes.id;
        }

        private void SearchProfile(ProfileSearchType type)
        {
            App.ViewModelLocator.ProfileSearch.CurrentUser = User;
            App.ViewModelLocator.ProfileSearch.LoadData(false, type);
            _navigationService.NavigateTo(typeof(ProfileSearchPage));
        }
    }
}
