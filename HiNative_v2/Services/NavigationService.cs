using HiNative.Views;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace HiNative.Services
{
    public class NavigationService : INavigationService
    {
        private Frame _frame { get; set; }

        public Page CurrentPage
        {
            get
            {
                _frame = ((Window.Current.Content as Frame).Content as Shell).FindName("myFrame") as Frame;
                return _frame.Content as Page;
            }
        }

        public string CurrentPageKey
        {
            get
            {
                _frame = ((Window.Current.Content as Frame).Content as Shell).FindName("myFrame") as Frame;
                return (_frame.Content as Page).Name;
            }
        }

        public void GoBack()
        {
            _frame = ((Window.Current.Content as Frame).Content as Shell).FindName("myFrame") as Frame;
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }

        public void NavigateTo(Type page)
        {
            _frame = ((Window.Current.Content as Frame).Content as Shell).FindName("myFrame") as Frame;
            _frame.Navigate(page);
        }

        public void NavigateTo(Type page, object parameter)
        {
            _frame = ((Window.Current.Content as Frame).Content as Shell).FindName("myFrame") as Frame;
            _frame.Navigate(page, parameter);
        }

        public void NavigateTo(Type page, object parameter, NavigationTransitionInfo info)
        {
            _frame = ((Window.Current.Content as Frame).Content as Shell).FindName("myFrame") as Frame;
            _frame.Navigate(page, parameter, info);
        }
    }
}
