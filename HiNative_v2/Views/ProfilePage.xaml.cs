using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
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
        public ProfilePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.Animation.ConnectedAnimationService"))
            //{
            //    var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("profilePicture");
            //    if (anim != null)
            //    {
            //        ellipseProfilePicture.Opacity = 1;
            //        anim.TryStart(ellipseProfilePicture);
            //    }
            //    var usernameAnim = ConnectedAnimationService.GetForCurrentView().GetAnimation("userName");
            //    if (usernameAnim != null)
            //    {
            //        usernameAnim.TryStart(lblUsername);
            //    } 
            //}
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                //ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("profilePicture", profilePicture);
            }
        }
    }
}
