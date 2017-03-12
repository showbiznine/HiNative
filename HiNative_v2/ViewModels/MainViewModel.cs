using HiNative.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using HiNative.Services;
using HiNative.API.Models;
using HiNative.Services;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using System.Net.Http;
using Windows.Foundation.Metadata;

namespace HiNative.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private INavigationService _navigationService { get { return ServiceLocator.Current.GetInstance<INavigationService>(); } }
        public HNUserProfile CurrentUser { get; set; }
        public bool InCall { get; set; }
        public int PageNumber { get; set; }
        public int MaxPages { get; set; }
        public HNQuestionsResult QuestionsResult { get; set; }
        public ObservableCollection<object> Topics { get; set; }
        public object SelectedTopic { get; set; }
        public ObservableCollection<HNQuestion> Questions { get; set; }
        public Grid LastSelectedGrid { get; set; }
        public HNFilter Filters { get; set; }
        public HNUnreadCount UnreadCount { get; private set; }
        #endregion

        #region Commands
        public RelayCommand ToggleMenuCommand { get; set; }
        public RelayCommand SelectTopicCommand { get; set; }
        public RelayCommand<ItemClickEventArgs> SelectQuestionCommand { get; set; }
        public RelayCommand<ItemClickEventArgs> NewQuestionCommand { get; set; }
        public RelayCommand FilterChangedCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }
        public RelayCommand GoToNotificationsCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                Filters = new HNFilter { interesting_questions_only = true, questions_not_answered_only = true, questions_with_audios_only = false };
            }
            else
            {
                InitPage();
            }
        }

        public void InitPage()
        {            
            CurrentUser = App.ViewModelLocator.Shell.CurrentUser;
            Filters = new HNFilter { interesting_questions_only = false, questions_not_answered_only = false, questions_with_audios_only = false };
            LoadTopics();
            PageNumber = 1;
            LoadData(false);
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ToggleMenuCommand = new RelayCommand(() => { App.ViewModelLocator.Shell.IsMenuOpen = !App.ViewModelLocator.Shell.IsMenuOpen; });
            SelectQuestionCommand = new RelayCommand<ItemClickEventArgs>(args =>
            {
                var q = args.ClickedItem as HNQuestion;

                var lv = args.OriginalSource as ListView;
                var lvi = lv.ContainerFromItem(q) as ListViewItem;
                LastSelectedGrid = lvi.ContentTemplateRoot as Grid;

                //if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.Animation.ConnectedAnimationService"))
                //{
                //    ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("questionRoot", LastSelectedGrid);

                //}
                App.ViewModelLocator.Question.CurrentQuestion = q;
                App.ViewModelLocator.Question.LoadAnswers((int)q.id);
                _navigationService.NavigateTo(typeof(QuestionPage));
            });
            NewQuestionCommand = new RelayCommand<ItemClickEventArgs>(args =>
            {
                NewQuestionClick(args);
            });
            SelectTopicCommand = new RelayCommand(() => LoadData(false));
            FilterChangedCommand = new RelayCommand(async () =>
            {
                var res = await DataService.PostFilter(Filters);
                if (res.IsSuccessStatusCode)
                {
                    LoadData(false);
                }
                else
                {
                    await new MessageDialog("Failed to set filters", "Error").ShowAsync();
                }
            });
            RefreshCommand = new RelayCommand(() => LoadData(false));
            GoToNotificationsCommand = new RelayCommand(() =>
            {
                App.ViewModelLocator.Notifications.LoadNotifications(false);
                _navigationService.NavigateTo(typeof(NotificationsPage));
            });
        }

        public void NewQuestionClick(ItemClickEventArgs args)
        {
            var q = (args.ClickedItem as Grid).Name;
            App.ViewModelLocator.Compose.QuestionType = q;
            App.ViewModelLocator.Compose.Topics = new ObservableCollection<object>();

            if (!q.Contains("Country"))
            {
                foreach (var item in CurrentUser.user_attributes.native_languages_attributes)
                    App.ViewModelLocator.Compose.Topics.Add(item);
                foreach (var item in CurrentUser.user_attributes.study_languages_attributes)
                    App.ViewModelLocator.Compose.Topics.Add(item);
            }
            else
            {
                foreach (var item in CurrentUser.user_attributes.user_interested_countries_attributes)
                    App.ViewModelLocator.Compose.Topics.Add(item);
            }
            if (App.ViewModelLocator.Compose.Topics.Count > 0)
            {
                App.ViewModelLocator.Compose.SelectedTopic = App.ViewModelLocator.Compose.Topics[0];
            }
            App.ViewModelLocator.Compose.SetupQuestion();
            _navigationService.NavigateTo(typeof(ComposeQuestionPage));
        }

        public void LoadTopics()
        {
            Topics = new ObservableCollection<object>(CurrentUser.user_attributes.native_languages_attributes);
            foreach (var item in CurrentUser.user_attributes.study_languages_attributes)
            {
                Topics.Add(item);
            }
            foreach (var item in CurrentUser.user_attributes.user_interested_countries_attributes)
            {
                Topics.Add(item);
            }
            SelectedTopic = Topics[0];
        }

        public async void LoadData(bool append)
        {
            if (append)
                PageNumber++;
            else
                PageNumber = 1;

            InCall = true;

            try
            {
                int id = 0;
                if (SelectedTopic is HNCountry)
                    id = (int)(SelectedTopic as HNCountry).country_id;
                else if (SelectedTopic is HNLanguage)
                    id = (int)(SelectedTopic as HNLanguage).language_id;

                if (Questions == null || !append)
                {
                    Questions = new ObservableCollection<HNQuestion>();
                }

                MaxPages = await DataService.PopulateQuestions(id, PageNumber, Questions, append, true);
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException)
                    await new MessageDialog("We're having trouble connecting to the HiNative servers").ShowAsync();
                if (append)
                    PageNumber--;
            }
            InCall = false;
        }

        public async void CheckUnreadCount()
        {
            UnreadCount = await DataService.GetUnreadCount();
        }
    }
}
