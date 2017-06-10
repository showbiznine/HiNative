using HiNative.ViewModels;
using HiNative.API.Models;
using Microsoft.Advertising.Shared.WinRT;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.Controls.Extensions;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HiNative.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Compositor _compositor;
        private ScrollViewer _listScroller;
        private bool _scrolling;
        private double _startPosition;
        private MainViewModel VM;

        public MainPage()
        {
            this.InitializeComponent();
            VM = this.DataContext as MainViewModel;
            //SetupAnimations();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.ViewModelLocator.Main.CheckUnreadCount();
            #region ConnectedAnimations
            var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("questionRoot");
            if (anim != null)
            {
                anim.TryStart(App.ViewModelLocator.Main.LastSelectedGrid);
            }
            #endregion
        }

        #region Composition
        private void SetupAnimations()
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            ElementCompositionPreview.SetIsTranslationEnabled(lblPageName, true);

            #region Top Bar
            var topBarHide = _compositor.CreateScalarKeyFrameAnimation();
            topBarHide.Duration = TimeSpan.FromMilliseconds(300);
            topBarHide.InsertKeyFrame(1, -100f);
            topBarHide.Target = "Translate.Y";
            ElementCompositionPreview.SetImplicitHideAnimation(lblPageName, topBarHide);


            var topBarShow = _compositor.CreateScalarKeyFrameAnimation();
            topBarShow.Duration = TimeSpan.FromMilliseconds(300);
            topBarShow.InsertKeyFrame(0, -100f);
            topBarShow.InsertKeyFrame(1, 0f);
            topBarShow.Target = "Translate.Y";
            ElementCompositionPreview.SetImplicitShowAnimation(lblPageName, topBarShow);

            #endregion

            #region New Question Button

            #endregion
        }
        #endregion

        private void listView_Loaded(object sender, RoutedEventArgs e)
        {
            //var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            //_listScroller = listView.GetFirstDescendantOfType<ScrollViewer>();
            //_listScroller.ViewChanged += ListScroller_ViewChanged;
            //_listScroller.ViewChanging += _listScroller_ViewChanging;
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
            if (progress < 60 & !App.ViewModelLocator.Main.InCall &&
                App.ViewModelLocator.Main.PageNumber < App.ViewModelLocator.Main.MaxPages)
            {
                App.ViewModelLocator.Main.LoadData(true);
            }
        }

        private void adMainPage_AdRefreshed(object sender, RoutedEventArgs e)
        {
            //if (adMainPage.HasAd)
            //    adMainPage.Visibility = Visibility.Visible;
            //else
            //    adMainPage.Visibility = Visibility.Collapsed;
        }

        private void adMainPage_ErrorOccurred(object sender, Microsoft.Advertising.WinRT.UI.AdErrorEventArgs e)
        {
            //adMainPage.Visibility = Visibility.Collapsed;
            //adMainPage.Refresh();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //SetupAnimations();
        }
    }
}
