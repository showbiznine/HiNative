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
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Popups;

namespace HiNative.ViewModels
{
    public class NotificationsViewModel : ViewModelBase
    {
        #region Fields
        private INavigationService _navigationService { get { return ServiceLocator.Current.GetInstance<INavigationService>(); } }
        public bool InCall { get; set; }
        public int PageNumber { get; set; }
        public HNActivities ActivityRoot { get; set; }
        public Ellipse LastClickedProfilePic { get; set; }
        #endregion

        #region Commands
        public RelayCommand<ItemClickEventArgs> SelectActivityCommand { get; set; }
        #endregion

        public NotificationsViewModel()
        {
            if (IsInDesignMode)
            {

            }
            else
            {
                InitializeCommands();
                InitPage();
            }
        }

        private void InitPage()
        {
            PageNumber = 1;
            LoadNotifications(false);
        }

        private void InitializeCommands()
        {
            SelectActivityCommand = new RelayCommand<ItemClickEventArgs>(async args =>
            {
                var i = args.ClickedItem as HNActivity;
                await DataService.CheckNotification(i.id);
                App.ViewModelLocator.Question.LoadQuestion(i.payload.id);
                _navigationService.NavigateTo(typeof(QuestionPage));
            });
        }

        public async Task LoadNotifications(bool append)
        {
            InCall = true;
            if (append)
                PageNumber++;
            else
                PageNumber = 1;
            try
            {
                if (!append)
                    ActivityRoot = await DataService.GetNotifications(PageNumber);
                else
                {
                    var r = await DataService.GetNotifications(PageNumber);
                    foreach (var item in r.activities)
                    {
                        ActivityRoot.activities.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                await new MessageDialog("We're having trouble connecting to the HiNative servers").ShowAsync();
                LoggerService.LogEvent("Get_notifications_failed");
            }
            InCall = false;
        }
    }
}
