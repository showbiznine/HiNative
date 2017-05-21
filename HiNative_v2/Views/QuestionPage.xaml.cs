using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HiNative.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuestionPage : Page
    {
        private Compositor _compositor;
        private Visual _attachmentsMenu;
        private Visual _answerTextBox;
        private ScalarKeyFrameAnimation _openAnimation;
        private ScalarKeyFrameAnimation _closeAnimation;
        private CompositeTransform _newBtnTransform;
        private bool _menuOpen;

        public QuestionPage()
        {
            this.InitializeComponent();

            stkAttachments.Loaded += (s, e) =>
            {
                SetupAnimations();
                CloseMenuInstantly();
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("ProfilePicture");
            if (anim != null)
            {
                anim.TryStart(ellProfilePicture, new UIElement[] { grdQuestion });
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back && ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.Animation.ConnectedAnimationService"))
            {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ProfilePicture", ellProfilePicture);
            }
        }

        #region Composition
        private void SetupAnimations()
        {
            //var root = ElementCompositionPreview.GetElementVisual(stkAskQuestion);
            //root.Offset = new Vector3(0f, (float)lstNewQuestion.ActualHeight, 0f);

            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _attachmentsMenu = ElementCompositionPreview.GetElementVisual(stkAttachments);
            _answerTextBox = ElementCompositionPreview.GetElementVisual(txtAnswer);

            #region Synced offset

            //Keep the offsets of the pan area and PaneRoot on sync
            var offsetExpressionAnimation = _compositor.CreateExpressionAnimation();
            offsetExpressionAnimation.Expression = "menu.Offset.X";
            offsetExpressionAnimation.SetReferenceParameter("menu", _attachmentsMenu);
            _answerTextBox.StartAnimation("Offset.X", offsetExpressionAnimation);

            #endregion

            #region Open Animation
            _openAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _openAnimation.Duration = TimeSpan.FromMilliseconds(300);
            _openAnimation.InsertKeyFrame(1.0f, (((float)stkAttachments.ActualWidth) - (float)btnAttach.ActualWidth));
            //_openAnimation.InsertKeyFrame(1.0f, 0f);
            #endregion

            #region Close Animation
            _closeAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _closeAnimation.Duration = TimeSpan.FromMilliseconds(300);
            _closeAnimation.InsertKeyFrame(1.0f, 0f);
            //_closeAnimation.InsertKeyFrame(1.0f, -(((float)stkAttachments.ActualWidth) - (float)btnAttach.ActualWidth));
            #endregion
        }

        private void attBtn_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            _newBtnTransform = btnAttach.GetCompositeTransform();
        }

        private void attBtn_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var x = e.Delta.Translation.X;

            var offset = _attachmentsMenu.Offset.X;

            // keep the pan within the boundary
            if (offset > 0) return;
            if (offset < -stkAttachments.ActualWidth)
            {
                _attachmentsMenu.Offset = new System.Numerics.Vector3(-(float)btnAttach.ActualWidth, 0, 0);
                return;
            }

            _attachmentsMenu.Offset = new System.Numerics.Vector3(offset + (float)x, 0, 0);
            // while we are panning the PanArea on X axis, let's sync the PaneRoot's position X too
        }

        private void attBtn_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var x = e.Velocities.Linear.X;

            var initialTranslation = -stkAttachments.ActualWidth;

            // ignore a little bit velocity (+/-0.1)
            if (x <= -0.1)
            {
                OpenMenu();
            }
            else if (x > -0.1 && x < 0.1)
            {
                if (Math.Abs(_newBtnTransform.TranslateX) < Math.Abs(stkAttachments.ActualWidth) / 2)
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
            _attachmentsMenu.StartAnimation("Offset.X", _openAnimation);
            _menuOpen = true;
        }

        private void CloseMenu()
        {
            _attachmentsMenu.StartAnimation("Offset.X", _closeAnimation);
            _menuOpen = false;
        }

        private void CloseMenuInstantly()
        {
            //_attachmentsMenu.Offset = new System.Numerics.Vector3(-(((float)stkAttachments.ActualWidth) - (float)btnAttach.ActualWidth), 0, 0);
            _menuOpen = false;
        }
        #endregion

        private void btnAttach_Click(object sender, RoutedEventArgs e)
        {
            if (_menuOpen)
                CloseMenu();
            else
                OpenMenu();
        }

        private void adQuestionPage_ErrorOccurred(object sender, Microsoft.Advertising.WinRT.UI.AdErrorEventArgs e)
        {

        }

        private void adQuestionPage_AdRefreshed(object sender, RoutedEventArgs e)
        {

        }
    }
}
