using HiNative.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class Shell : Page
    {
        public ShellViewModel VM;
        public Shell()
        {
            this.InitializeComponent();
            VM = this.DataContext as ShellViewModel;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            myFrame.Navigated += OnFrameNavigated;
        }

        private void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                myFrame.CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;

            //if (myFrame.Content.ToString().Contains("Main"))
            //    hmbHome.IsChecked = true;
            //else
            //    hmbHome.IsChecked = false;

            //if (myFrame.Content.ToString().Contains("Settings"))
            //    hmbSettings.IsChecked = true;
            //else
            //    hmbSettings.IsChecked = false;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (myFrame.CanGoBack)
            {
                e.Handled = true;
                myFrame.GoBack();
            }
        }

        //private async void mySplitView_Loaded(object sender, RoutedEventArgs e)
        //{
        //    await App.ViewModelLocator.Shell.CheckLoggedIn();
        //}

        private void NavigationView_SettingsInvoked(NavigationView sender, object args)
        {
            VM.navigationService.NavigateTo(typeof(SettingsPage));
        }

        private async void myFrame_Loaded(object sender, RoutedEventArgs e)
        {
            await App.ViewModelLocator.Shell.CheckLoggedIn();
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            NavView.MenuItems.Add(new NavigationMenuItem()
            { Text = "Home", Icon = new SymbolIcon(Symbol.Home), Tag = "home" });
            NavView.MenuItems.Add(new NavigationMenuItem()
            { Text = "My Profile", Icon = new SymbolIcon(Symbol.Contact), Tag = "profile" });
        }
    }
}
