using HiNative.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiNative.ViewModels
{
    public class ViewModelLocator
    {/// <summary>
     /// Initializes a new instance of the ViewModelLocator class.
     /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                //Create design time view services and models
                SimpleIoc.Default.Register<INavigationService, NavigationService>();
            }
            else
            {
                //Create run time view services and models
                var navigationService = CreateNavigationSevice();
                SimpleIoc.Default.Register(() => navigationService);
            }

            SimpleIoc.Default.Register<ShellViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<QuestionViewModel>();
            SimpleIoc.Default.Register<ProfileViewModel>();
            SimpleIoc.Default.Register<ProfileSearchViewModel>();
            SimpleIoc.Default.Register<ComposeQuestionViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<NotificationsViewModel>();
        }

        private INavigationService CreateNavigationSevice()
        {
            var navigationService = new NavigationService();
            return navigationService;
        }

        public ShellViewModel Shell { get { return ServiceLocator.Current.GetInstance<ShellViewModel>(); } }
        public MainViewModel Main { get { return ServiceLocator.Current.GetInstance<MainViewModel>(); } }
        public LoginViewModel Login { get { return ServiceLocator.Current.GetInstance<LoginViewModel>(); } }
        public QuestionViewModel Question { get { return ServiceLocator.Current.GetInstance<QuestionViewModel>(); } }
        public ProfileViewModel Profile { get { return ServiceLocator.Current.GetInstance<ProfileViewModel>(); } }
        public ProfileSearchViewModel ProfileSearch { get { return ServiceLocator.Current.GetInstance<ProfileSearchViewModel>(); } }
        public ComposeQuestionViewModel Compose { get { return ServiceLocator.Current.GetInstance<ComposeQuestionViewModel>(); } }
        public SettingsViewModel Settings { get { return ServiceLocator.Current.GetInstance<SettingsViewModel>(); } }
        public NotificationsViewModel Notifications { get { return ServiceLocator.Current.GetInstance<NotificationsViewModel>(); } }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
