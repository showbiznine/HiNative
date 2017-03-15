using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HiNative.Services;
using HiNative.Views;
using HiNative.API.Models;
using HiNative.Services;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace HiNative.ViewModels
{
    public class ProfileSearchViewModel : ViewModelBase
    {
        #region Fields
        private INavigationService _navigationService { get { return ServiceLocator.Current.GetInstance<INavigationService>(); } }
        public HNUserProfile CurrentUser { get; set; }
        public bool InCall { get; set; }
        public int PageNumber { get; set; }
        public HNQuestionsResult QuestionsResult { get; set; }
        public ObservableCollection<HNQuestion> Questions { get; set; }
        public Grid LastSelectedGrid { get; set; }
        public HNUnreadCount UnreadCount { get; private set; }
        private ProfileSearchType _currentType;

        public enum ProfileSearchType
        {
            notype,
            questions,
            answers,
            quick_responses,
            likes,
            featured_answers
        }
        #endregion

        #region Commands
        public RelayCommand ToggleMenuCommand { get; set; }
        public RelayCommand SelectTopicCommand { get; set; }
        public RelayCommand<ItemClickEventArgs> SelectQuestionCommand { get; set; }
        public RelayCommand<ItemClickEventArgs> NewQuestionCommand { get; set; }
        public RelayCommand FilterChangedCommand { get; set; }

        #endregion

        public ProfileSearchViewModel()
        {
            if (IsInDesignMode)
            {
            }
            else
            {
                InitPage();
            }
        }

        public void InitPage()
        {
            PageNumber = 1;
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
        }

        public async Task LoadData(bool append, ProfileSearchType type)
        {
            if (type == ProfileSearchType.notype)
                type = _currentType;

            _currentType = type;
            if (append)
                PageNumber++;
            else
                PageNumber = 1;

            InCall = true;
            try
            {
                if (Questions == null || !append)
                {
                    Questions = new ObservableCollection<HNQuestion>();
                }
                QuestionsResult = await DataService.ProfileSearch(PageNumber, CurrentUser.user_attributes.id, type.ToString());
                foreach (var q in QuestionsResult.questions)
                {
                    Questions.Add(q);
                }
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException)
                    await new MessageDialog("We're having trouble connecting to the HiNative servers").ShowAsync();
            }
            InCall = false;
        }

        public async Task CheckUnreadCount()
        {
            UnreadCount = await DataService.GetUnreadCount();
        }
    }
}
