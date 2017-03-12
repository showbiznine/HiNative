using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace HiNative.Controls
{
    public sealed partial class NewQuestionControl : UserControl
    {
        private Compositor _compositor;
        private Visual _newQuestionPanel;
        private ScalarKeyFrameAnimation _openAnimation;
        private ScalarKeyFrameAnimation _closeAnimation;
        private bool _menuOpen = false;
        private CompositeTransform _newQBtnTransform;

        public EventHandler<ItemClickEventArgs> ListviewItemCick;

        public NewQuestionControl()
        {
            this.InitializeComponent();
        }

        private void grdNewQButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_menuOpen)
                CloseMenu();
            else
                OpenMenu();
        }

        private void stkAskQuestion_Loaded(object sender, RoutedEventArgs e)
        {
            SetupAnimations();
            stkAskQuestion.Margin = new Thickness(0, 0, 0, -lstNewQuestion.ActualHeight);
        }

        private void lstNewQuestion_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #region Composition
        private void SetupAnimations()
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _newQuestionPanel = ElementCompositionPreview.GetElementVisual(stkAskQuestion);

            #region Synced offset

            //Keep the offsets of the pan area and PaneRoot on sync
            var offsetExpressionAnimation = _compositor.CreateExpressionAnimation();
            offsetExpressionAnimation.Expression = "button.Offset.Y";
            offsetExpressionAnimation.SetReferenceParameter("button", _newQuestionPanel);

            #endregion
        }

        private void grdNewQButton_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            //_newQBtnTransform = grdNewQButton.GetCompositeTransform();
        }

        private void grdNewQButton_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            //var y = e.Delta.Translation.Y;

            //var offset = _newQuestionPanel.Offset.Y;

            //// keep the pan within the boundary
            ////if (offset < 0) return;
            ////if (offset > lstNewQuestion.ActualHeight)
            ////{
            ////    _newQuestionPanel.Offset = new Vector3(0, (float)(_newQuestionPanel.Offset.Y - lstNewQuestion.ActualHeight), 0);
            ////    return;
            ////}

            //_newQuestionPanel.Offset = new Vector3(0, offset + (float)y, 0);
            //// while we are panning the PanArea on X axis, let's sync the PaneRoot's position X too
        }

        private void grdNewQButton_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            //var y = e.Velocities.Linear.Y;

            //var initialTranslation = lstNewQuestion.ActualHeight;

            //// ignore a little bit velocity (+/-0.1)
            //if (y <= -0.1)
            //{
            //    OpenMenu();
            //}
            //else if (y > -0.1 && y < 0.1)
            //{
            //    if (Math.Abs(_newQBtnTransform.TranslateY) < Math.Abs(lstNewQuestion.ActualHeight) / 2)
            //    {
            //        OpenMenu();
            //    }
            //    else
            //    {
            //        CloseMenu();
            //    }
            //}
            //else
            //{
            //    CloseMenu();
            //}
        }

        private void OpenMenu()
        {
            #region Open Animation
            _openAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _openAnimation.Duration = TimeSpan.FromMilliseconds(300);
            _openAnimation.InsertKeyFrame(1.0f, (float)(_newQuestionPanel.Offset.Y - lstNewQuestion.ActualHeight));
            #endregion

            _newQuestionPanel.StartAnimation("Offset.Y", _openAnimation);
            _menuOpen = true;
        }

        private void CloseMenu()
        {
            #region Close Animation
            _closeAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _closeAnimation.Duration = TimeSpan.FromMilliseconds(300);
            _closeAnimation.InsertKeyFrame(1.0f, (float)(_newQuestionPanel.Offset.Y + lstNewQuestion.ActualHeight));
            #endregion

            _newQuestionPanel.StartAnimation("Offset.Y", _closeAnimation);
            _menuOpen = false;
        }

        private void CloseMenuInstantly()
        {
            //_newQuestionButton.Offset = new Vector3(0, (float)(lstNewQuestion.ActualHeight + adMainPage.ActualHeight), 0);
            _newQuestionPanel.Offset = new Vector3(0, (float)(_newQuestionPanel.Offset.Y + lstNewQuestion.ActualHeight), 0);
            _menuOpen = false;
        }
        #endregion

        private void lstNewQuestion_ItemClick(object sender, ItemClickEventArgs e)
        {
            ListviewItemCick(sender, e);
        }
    }
}
