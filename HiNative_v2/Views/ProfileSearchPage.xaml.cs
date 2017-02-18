using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.Extensions;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HiNative.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfileSearchPage : Page
    {
        private ScrollViewer _listScroller;
        private bool _scrolling;
        private double _startPosition;

        public ProfileSearchPage()
        {
            this.InitializeComponent();
        }

        private void listView_Loaded(object sender, RoutedEventArgs e)
        {
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _listScroller = listView.GetFirstDescendantOfType<ScrollViewer>();
            _listScroller.ViewChanged += ListScroller_ViewChanged;
            _listScroller.ViewChanging += _listScroller_ViewChanging;
        }

        private void _listScroller_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (!_scrolling)
            {
                _startPosition = _listScroller.VerticalOffset;
                _scrolling = true;
            }
        }

        private void ListScroller_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            // When 30 px from the end of the list, load more items
            ScrollViewer view = (ScrollViewer)sender;
            double progress = view.ScrollableHeight - view.VerticalOffset;
            if (progress < 60 & !App.ViewModelLocator.ProfileSearch.InCall && App.ViewModelLocator.ProfileSearch.PageNumber < App.ViewModelLocator.ProfileSearch.QuestionsResult.pagination.total_pages)
            {
                App.ViewModelLocator.ProfileSearch.LoadData(true, ViewModels.ProfileSearchViewModel.ProfileSearchType.notype);
            }
        }
    }
}
