using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using HiNative.Resources.Dialogs;
using HiNative.Services;
using HiNative.Views;
using HiNative.API.Models;
using HiNative.Services;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace HiNative.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region Fields
        private INavigationService _navigationService { get { return ServiceLocator.Current.GetInstance<INavigationService>(); } }
        public bool InCall { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int FlipViewIndex { get; set; }

        #region NewUser
        public string New_Username { get; set; }
        public bool UsernameUnique { get; set; }
        public string New_Email { get; set; }
        public bool EmailUnique { get; set; }
        public string New_Password { get; set; }
        public string New_ConfirmPassword { get; set; }
        public ObservableCollection<HNLanguage> New_NativeLanguages { get; set; }
        public ObservableCollection<HNLanguage> New_StudyLanguages { get; set; }
        public ObservableCollection<HNCountry> New_NativeCountry { get; set; }
        public ObservableCollection<HNCountry> New_InterestedCountries { get; set; }
        #endregion

        #endregion

        #region Commands
        public RelayCommand LogInCommand { get; set; }
        public RelayCommand SignUpCommand { get; set; }

        public RelayCommand<TextChangedEventArgs> CheckUsernameConflictsCommand { get; set; }
        public RelayCommand<TextChangedEventArgs> CheckEmailConflictsCommand { get; set; }

        public RelayCommand AddNativeLanguageCommand { get; set; }
        public RelayCommand AddStudyLanguageCommand { get; set; }
        public RelayCommand AddNativeCountryCommand { get; set; }
        public RelayCommand AddInterestedCountryCommand { get; set; }
        #endregion

        public LoginViewModel()
        {
            if (IsInDesignMode)
            {

            }
            else
            {
                InitializeCommands();
                InitializeLists();
            }
        }

        private void InitializeLists()
        {
            New_InterestedCountries = new ObservableCollection<HNCountry>();
            New_NativeCountry = new ObservableCollection<HNCountry>();
            New_StudyLanguages = new ObservableCollection<HNLanguage>();
            New_NativeLanguages = new ObservableCollection<HNLanguage>();
        }

        private void InitializeCommands()
        {
            LogInCommand = new RelayCommand(async() =>
            {
                try
                {
                    InCall = true;
                    var res = await DataService.LogIn(Username, Password);
                    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                    localSettings.Values["User_ID"] = res.profile.user_attributes.id;
                    localSettings.Values["API_Token"] = res.token;
                    await App.ViewModelLocator.Shell.CheckLoggedIn();
                    App.ViewModelLocator.Main.InitPage();
                    _navigationService.NavigateTo(typeof(MainPage));
                    Username = string.Empty;
                    Password = string.Empty;
                    InCall = false;
                }
                catch (Exception ex)
                {
                    if (ex is HttpRequestException)
                         await new MessageDialog("We're having trouble connecting to the HiNative servers").ShowAsync();
                    else
                        await new MessageDialog("The username or password your entered was incorrect").ShowAsync();
                }
            });

            CheckUsernameConflictsCommand = new RelayCommand<TextChangedEventArgs>(args =>
            {
                //CheckUniqueUsernameAsync();
            });
            CheckEmailConflictsCommand = new RelayCommand<TextChangedEventArgs>(args =>
            {
                //CheckUniqueEmailAsync();
            });

            SignUpCommand = new RelayCommand(async () =>
            {
                await RegisterUserAsync();

            });

            #region Add Languages and Countries
            AddNativeLanguageCommand = new RelayCommand(async () =>
            {
                var diag = new SelectLanguageDialog(true);
                var res = await diag.ShowAsync();
                if (res == ContentDialogResult.Primary)
                {
                    New_NativeLanguages.Add(diag.SelectedLanguage);
                }
            });
            AddStudyLanguageCommand = new RelayCommand(async () =>
            {
                var diag = new SelectLanguageDialog(false);
                var res = await diag.ShowAsync();
                if (res == ContentDialogResult.Primary)
                {
                    New_StudyLanguages.Add(diag.SelectedLanguage);
                }
            });
            AddNativeCountryCommand = new RelayCommand(async () =>
            {
                var diag = new SelectCountriesDialog();
                var res = await diag.ShowAsync();
                if (res == ContentDialogResult.Primary)
                {
                    New_NativeCountry = new ObservableCollection<HNCountry> { diag.SelectedCountry };
                }
            });
            AddInterestedCountryCommand = new RelayCommand(async () =>
            {
                var diag = new SelectCountriesDialog();
                var res = await diag.ShowAsync();
                if (res == ContentDialogResult.Primary)
                {
                    New_InterestedCountries.Add(diag.SelectedCountry);

                }            });
            #endregion
        }

        private async Task RegisterUserAsync()
        {
            #region Validation
            await CheckUniqueEmailAsync();
            await CheckUniqueUsernameAsync();
            if (string.IsNullOrWhiteSpace(New_Username) ||
        New_Username.Length < 6)
            {
                await new MessageDialog("Your username must be at least 6 characters",
                    "Please enter a valid username").ShowAsync();
            }
            else if (string.IsNullOrWhiteSpace(New_Password) ||
                    New_Password.Length < 8)
            {
                await new MessageDialog("Your password must be at least 8 characters",
                    "Please enter a valid password").ShowAsync();
            }
            else if (New_Password != New_ConfirmPassword)
            {
                await new MessageDialog("Your passwords must match").ShowAsync();
            }
            else if (string.IsNullOrWhiteSpace(New_Email) || !IsValidEmail(New_Email))
            {
                await new MessageDialog("Please enter a valid email address").ShowAsync();
            }
            else if (!EmailUnique)
            {
                await new MessageDialog("This email address is already taken").ShowAsync();
            }
            else if (!UsernameUnique)
            {
                await new MessageDialog("This username is already taken").ShowAsync();
            }
            #endregion
            else
            {
                #region Build User

                #region Format Languages/Countries
                foreach (var item in New_NativeLanguages)
                    item.language_name = null;
                foreach (var item in New_StudyLanguages)
                    item.language_name = null;
                #endregion

                var newUser = new HNNewUserContainer
                {
                    user = new HNNewUser
                    {
                        name = New_Username,
                        email = New_Email,
                        password = New_Password,
                        password_confirmation = New_ConfirmPassword,
                        profile_attributes = new HNProfileAttributes { interface_id = 1 },
                        terms_of_use = "1",
                        platform = "android",
                        mail_setting_attributes = new HNMailSettingAttributes { info = false },

                        native_languages_attributes = New_NativeLanguages,
                        study_languages_attributes = New_StudyLanguages,
                        #region Notifications Attributes
                        notifications_attributes = new ObservableCollection<HNNotificationsAttribute>
                            {
                                new HNNotificationsAttribute
                                {
                                    platform = "android",
                                    token="fRxU1CeWSL0:APA91bFcj4kOiCfgHNIj2UZHeVK3AUK5c1PasVuJQeSei9Mckx1eiC-BV78uaABOWtVXSWHDMFn4sjba8OKmQxdo_c1MibypzeHrhoaEv0e-J54z_5tCRFZw2kiHtrD8RcS61cQa0bqv"
                                }
                            }
                        #endregion
                    }
                };
                #endregion

                try
                {
                    var createdUser = await DataService.SignUp(newUser);
                    LoggerService.LogEvent("User_registered");
                    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                    if (createdUser.token != null)
                    {
                        localSettings.Values["API_Token"] = createdUser.token;
                        localSettings.Values["User_ID"] = createdUser.profile.user_attributes.id;
                    }
                    await App.ViewModelLocator.Shell.CheckLoggedIn();
                    App.ViewModelLocator.Main.CurrentUser = App.ViewModelLocator.Shell.CurrentUser;
                    App.ViewModelLocator.Main.InitPage();
                    _navigationService.NavigateTo(typeof(MainPage));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    await new MessageDialog("Sign up failed, try a different username and/or email").ShowAsync();
                    LoggerService.LogEvent("Registration_failed");
                }
            }
        }

        private async Task CheckUniqueUsernameAsync()
        {
            var query = "https://hinative.com/api/v1/check_availability?name=" + New_Username;
            HttpClient http = new HttpClient();
            var response = await http.GetAsync(query);
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict || New_Username == "test123")
                UsernameUnique = false;
            else
                UsernameUnique = true;
        }

        private async Task CheckUniqueEmailAsync()
        {
            var query = "https://hinative.com/api/v1/check_availability?email=" + New_Email;
            HttpClient http = new HttpClient();
            var response = await http.GetAsync(query);
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                EmailUnique = false;
            else
                EmailUnique = true;
        }

        #region Validation
        public static bool IsValidEmail(string email)
        {
            // No spaces allowed
            email = email.Trim();
            int space = email.IndexOf(" ");
            if (space != -1)
                return false;

            // -------------------------
            // EMAIL MUST BE IN FORMAT:  LOCAL @ DOMAIN
            // -------------------------
            // There must be exactly one @
            int firstAtSign = email.IndexOf("@");
            if (firstAtSign == -1)
                return false;

            int lastAtSign = email.LastIndexOf("@");
            if (lastAtSign != firstAtSign)
                return false;

            // There must be a LOCAL and a DOMAIN
            string local = email.Substring(0, firstAtSign);
            string domain = email.Substring(firstAtSign + 1);

            if ((local.Length < 1) || (local.Length > 64))  // max length of 64.
                return false;
            if ((domain.Length < 1) || (domain.Length > 63))  // max length of 63.
                return false;

            // -------------------------
            // TEST LOCAL PIECE
            // -------------------------
            // Can't begin or end with . or have two .. in a row.
            if (ValidatePeriods(local) == false)
                return false;

            // All characters must be a letter, number or allowed special character.
            if (ValidateCharacters(local) == false)
                return false;

            // -------------------------
            // TEST DOMAIN PIECE  
            // -------------------------
            // Can't begin or end with . or have two .. in a row.
            if (ValidatePeriods(domain) == false)
                return false;

            // Domain is in format label.label.label
            string[] labels = domain.Split('.');

            // Test each label
            foreach (string label in labels)
            {
                if (label.Length < 1 || label.Length > 62)
                    return false;
                if (label[0] == '-' || label[label.Length - 1] == '-')
                    return false;
                if (label.Contains("--"))
                    return false;
                if (ValidateCharacters(label) == false)
                    return false;
            }

            // Last label must be all alphabetic
            string lastLabel = labels[labels.Length - 1];
            foreach (char c in lastLabel.ToCharArray())
            {
                if (Char.IsLetter(c) == false)
                    return false;
            }
            return true;
        }
        private static bool ValidatePeriods(string label)
        {
            if (string.IsNullOrEmpty(label))
                return false;

            // Can't have two periods in a row.
            if (label.Contains(".."))
                return false;

            // Can't begin or end with a period.
            if (label[0] == '.')
                return false;
            if (label[label.Length - 1] == '.')
                return false;

            return true;
        }
        private static bool ValidateCharacters(string label)
        {
            if (string.IsNullOrEmpty(label))
                return false;

            char[] allowed = { '!', '$', '%', '&', '*', '-', '=', '?', '^', '_', '{', '}', '~', '\'', '.' };
            foreach (char c in label.ToCharArray())
            {
                if (Char.IsLetterOrDigit(c))
                    continue;

                int x = c.ToString().IndexOfAny(allowed);
                if (x == -1)
                    return false;
            }

            return true;
        }
        #endregion
    }
}
