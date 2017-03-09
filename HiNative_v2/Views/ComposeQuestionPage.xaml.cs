using HiNative.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HiNative.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ComposeQuestionPage : Page
    {
        ComposeQuestionViewModel Vm;

        public ComposeQuestionPage()
        {
            this.InitializeComponent();
            Vm = DataContext as ComposeQuestionViewModel;

            InputPane.GetForCurrentView().Showing += (s, args) =>
            {
                const double extraHeightBuffer = 20.0;

                UIElement focused = FocusManager.GetFocusedElement() as UIElement;
                if (null != focused)
                {
                    GeneralTransform gt = focused.TransformToVisual(this);
                    Point focusedPoint = gt.TransformPoint(new Point(0, focused.RenderSize.Height - 1));
                    double bottomOfFocused = focusedPoint.Y + extraHeightBuffer;
                    double windowHeight = Window.Current.Bounds.Height;
                    double topOfInputPane = windowHeight - args.OccludedRect.Height;
                    if (bottomOfFocused > topOfInputPane)
                    {
                        var trans = new TranslateTransform();
                        trans.Y = -(bottomOfFocused - topOfInputPane);
                        this.RenderTransform = trans;
                    }

                    args.EnsuredFocusedElementInView = true;
                }
            };

            InputPane.GetForCurrentView().Hiding += (s, args) =>
            {
                var trans = new TranslateTransform();
                trans.Y = 0;
                this.RenderTransform = trans;
                args.EnsuredFocusedElementInView = false;
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.ViewModelLocator.Shell.RequestAd();
        }

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            myScrollViewer.ScrollToElement(sender as UIElement);
        }
    }
}
