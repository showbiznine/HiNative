using HiNativeShared.API.Models;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HiNative.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Compositor _compositor;
        private Visual _newQuestionList;
        private Visual _newQuestionButton;
        private CompositeTransform _newQBtnTransform;
        bool _menuOpen = false;
        private ScalarKeyFrameAnimation _openAnimation;
        private ScalarKeyFrameAnimation _closeAnimation;
        private ScalarKeyFrameAnimation _closeInstantAnimation;
        private ScrollViewer _listScroller;
        private bool _scrolling;
        private double _startPosition;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.ViewModelLocator.Main.CheckUnreadCount();
            #region ConnectedAnimations
            if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.Animation.ConnectedAnimationService"))
            {
                var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("questionRoot");
                if (anim != null)
                {
                    anim.TryStart(App.ViewModelLocator.Main.LastSelectedGrid);
                }
            }
            #endregion
        }

        private void lstNewQuestion_Loaded(object sender, RoutedEventArgs e)
        {
            SetupAnimations();
            CloseMenuInstantly();
        }

        #region Composition
        private void SetupAnimations()
        {
            //var root = ElementCompositionPreview.GetElementVisual(stkAskQuestion);
            //root.Offset = new Vector3(0f, (float)lstNewQuestion.ActualHeight, 0f);

            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _newQuestionList = ElementCompositionPreview.GetElementVisual(lstNewQuestion);
            _newQuestionButton = ElementCompositionPreview.GetElementVisual(grdNewQButton);

            #region Synced offset

            //Keep the offsets of the pan area and PaneRoot on sync
            var offsetExpressionAnimation = _compositor.CreateExpressionAnimation();
            offsetExpressionAnimation.Expression = "button.Offset.Y";
            offsetExpressionAnimation.SetReferenceParameter("button", _newQuestionButton);
            _newQuestionList.StartAnimation("Offset.Y", offsetExpressionAnimation);

            #endregion

            #region Open Animation
            _openAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _openAnimation.Duration = TimeSpan.FromMilliseconds(300);
            _openAnimation.InsertKeyFrame(1.0f, (float)adMainPage.ActualHeight);
            #endregion

            #region Close Animation
            _closeAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _closeAnimation.Duration = TimeSpan.FromMilliseconds(300);
            _closeAnimation.InsertKeyFrame(1.0f, (float)(lstNewQuestion.ActualHeight + adMainPage.ActualHeight));
            #endregion
        }

        private void grdNewQButton_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            _newQBtnTransform = grdNewQButton.GetCompositeTransform();
        }

        private void grdNewQButton_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var y = e.Delta.Translation.Y;

            var offset = _newQuestionButton.Offset.Y;

            // keep the pan within the boundary
            if (offset < 0) return;
            if (offset > lstNewQuestion.ActualHeight)
            {
                _newQuestionButton.Offset = new Vector3(0, (float)(lstNewQuestion.ActualHeight), 0);
                return;
            }

            _newQuestionButton.Offset = new Vector3(0, offset + (float)y, 0);
            // while we are panning the PanArea on X axis, let's sync the PaneRoot's position X too
        }

        private void grdNewQButton_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var y = e.Velocities.Linear.Y;

            var initialTranslation = lstNewQuestion.ActualHeight;

            // ignore a little bit velocity (+/-0.1)
            if (y <= -0.1)
            {
                OpenMenu();
            }
            else if (y > -0.1 && y < 0.1)
            {
                if (Math.Abs(_newQBtnTransform.TranslateY) < Math.Abs(lstNewQuestion.ActualHeight) / 2)
                {
                    OpenMenu();
                }
                else
                {
                    CloseMenu();
                }
            }
            else
            {
                CloseMenu();
            }
        }

        private void OpenMenu()
        {
            _newQuestionButton.StartAnimation("Offset.Y", _openAnimation);
            _menuOpen = true;
        }

        private void CloseMenu()
        {
            _newQuestionButton.StartAnimation("Offset.Y", _closeAnimation);
            _menuOpen = false;
        }

        private void CloseMenuInstantly()
        {
            _newQuestionButton.Offset = new System.Numerics.Vector3(0, (float)(lstNewQuestion.ActualHeight + adMainPage.ActualHeight), 0);
            _menuOpen = false;
        }
        #endregion

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SetupAnimations();
        }

        private void grdNewQButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_menuOpen)
                CloseMenu();
            else
                OpenMenu();
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
            if (progress < 60 & !App.ViewModelLocator.Main.InCall &&
                App.ViewModelLocator.Main.PageNumber < App.ViewModelLocator.Main.MaxPages)
            {
                App.ViewModelLocator.Main.LoadData(true);
            }
        }

        private void listView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = (HNQuestion)e.ClickedItem;
            var container = ((PullToRefreshListView)e.OriginalSource).ContainerFromItem(e.ClickedItem) as ListViewItem;
            var grid = container.ContentTemplateRoot;
            //var questionRoot = grid.GetFirstDescendantOfType<Grid>();
            //var profilePicture = grid.GetFirstDescendantOfType<Ellipse>();

            if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.Animation.ConnectedAnimationService"))
            {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("QuestionRoot", grid);
            }
        }

        private void adMainPage_AdRefreshed(object sender, RoutedEventArgs e)
        {
            if (adMainPage.HasAd)
                adMainPage.Visibility = Visibility.Visible;
            else
                adMainPage.Visibility = Visibility.Collapsed;
        }

        private void adMainPage_ErrorOccurred(object sender, Microsoft.Advertising.WinRT.UI.AdErrorEventArgs e)
        {
            adMainPage.Visibility = Visibility.Collapsed;
            //adMainPage.Refresh();
        }
    }
}
