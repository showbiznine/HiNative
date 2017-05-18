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
    public sealed partial class ProfilePage : Page
    {
        Compositor _compositor;

        public ProfilePage()
        {
            this.InitializeComponent();
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            SetupAnimations();
        }

        private void SetupAnimations()
        {
            var topBarShowAnimation = _compositor.CreateScalarKeyFrameAnimation();
            topBarShowAnimation.Target = "Translation.Y";
            topBarShowAnimation.Duration = TimeSpan.FromMilliseconds(300);
            topBarShowAnimation.InsertKeyFrame(0, -100f);
            topBarShowAnimation.InsertKeyFrame(1, 0);

            ElementCompositionPreview.SetImplicitShowAnimation(recTop, topBarShowAnimation);

            var topBarHideAnimation = _compositor.CreateScalarKeyFrameAnimation();
            topBarHideAnimation.Target = "Translation.Y";
            topBarHideAnimation.Duration = TimeSpan.FromMilliseconds(300);
            topBarHideAnimation.InsertKeyFrame(1, -100f);

            ElementCompositionPreview.SetIsTranslationEnabled(recTop, true);
            ElementCompositionPreview.SetImplicitShowAnimation(recTop, topBarShowAnimation);
            ElementCompositionPreview.SetImplicitHideAnimation(recTop, topBarHideAnimation);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.Animation.ConnectedAnimationService"))
            {
                var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("ProfilePicture");
                if (anim != null)
                {
                    //ellProfilePicture.Opacity = 1;
                    anim.TryStart(ellProfilePicture);
                }
                var usernameAnim = ConnectedAnimationService.GetForCurrentView().GetAnimation("Username");
                if (usernameAnim != null)
                {
                    usernameAnim.TryStart(lblUsername);
                }
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ProfilePicture", ellProfilePicture);
            }
        }
    }
}
