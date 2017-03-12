using HiNative.API.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HiNative.Resources.Dialogs
{
    public sealed partial class SelectLanguageDialog : ContentDialog
    {

        public HNLanguage SelectedLanguage { get; set; }
        public ObservableCollection<HNLanguage> LanguageList { get; set; }
        public bool IsLanguageSelected { get; set; }
        public bool NativeLanguage { get; set; }
        public bool StudyLanguage { get; set; }

        public SelectLanguageDialog(bool native)
        {
            this.InitializeComponent();
            NativeLanguage = native;
            StudyLanguage = !native;
            SetupLanguageList();
        }

        private void SetupLanguageList()
        {
            LanguageList = new ObservableCollection<HNLanguage>();
            #region Languages
            LanguageList.Add(new HNLanguage { language_id = 1, language_name = "Afrikaans" });
            LanguageList.Add(new HNLanguage { language_id = 2, language_name = "Albanian" });
            LanguageList.Add(new HNLanguage { language_id = 3, language_name = "Arabic" });
            LanguageList.Add(new HNLanguage { language_id = 4, language_name = "Azerbaijani" });
            LanguageList.Add(new HNLanguage { language_id = 5, language_name = "Ainu" });
            LanguageList.Add(new HNLanguage { language_id = 6, language_name = "Armenian" });
            LanguageList.Add(new HNLanguage { language_id = 7, language_name = "Aymara" });
            LanguageList.Add(new HNLanguage { language_id = 8, language_name = "Azeri" });
            LanguageList.Add(new HNLanguage { language_id = 9, language_name = "Basque" });
            LanguageList.Add(new HNLanguage { language_id = 10, language_name = "Belarusian" });
            LanguageList.Add(new HNLanguage { language_id = 11, language_name = "Bengali" });
            LanguageList.Add(new HNLanguage { language_id = 12, language_name = "Bosnian" });
            LanguageList.Add(new HNLanguage { language_id = 13, language_name = "Bulgarian" });
            LanguageList.Add(new HNLanguage { language_id = 14, language_name = "Catalan" });
            LanguageList.Add(new HNLanguage { language_id = 15, language_name = "Cherokee" });
            LanguageList.Add(new HNLanguage { language_id = 16, language_name = "Croatian" });
            LanguageList.Add(new HNLanguage { language_id = 17, language_name = "Czech" });
            LanguageList.Add(new HNLanguage { language_id = 18, language_name = "Danish" });
            LanguageList.Add(new HNLanguage { language_id = 19, language_name = "Dutch" });
            LanguageList.Add(new HNLanguage { language_id = 20, language_name = "Dutch (Belgium)" });
            LanguageList.Add(new HNLanguage { language_id = 21, language_name = "English (UK)" });
            LanguageList.Add(new HNLanguage { language_id = 22, language_name = "English (US)" });
            LanguageList.Add(new HNLanguage { language_id = 23, language_name = "Esperanto" });
            LanguageList.Add(new HNLanguage { language_id = 24, language_name = "Estonian" });
            LanguageList.Add(new HNLanguage { language_id = 25, language_name = "Faroese" });
            LanguageList.Add(new HNLanguage { language_id = 26, language_name = "Finnish" });
            LanguageList.Add(new HNLanguage { language_id = 27, language_name = "French (Canada)" });
            LanguageList.Add(new HNLanguage { language_id = 28, language_name = "French (France)" });
            LanguageList.Add(new HNLanguage { language_id = 29, language_name = "Frisian" });
            LanguageList.Add(new HNLanguage { language_id = 30, language_name = "Galician" });
            LanguageList.Add(new HNLanguage { language_id = 31, language_name = "Georgian" });
            LanguageList.Add(new HNLanguage { language_id = 32, language_name = "German" });
            LanguageList.Add(new HNLanguage { language_id = 33, language_name = "Greek" });
            LanguageList.Add(new HNLanguage { language_id = 34, language_name = "Guarani" });
            LanguageList.Add(new HNLanguage { language_id = 35, language_name = "Gujarati" });
            LanguageList.Add(new HNLanguage { language_id = 36, language_name = "Haitian Creole" });
            LanguageList.Add(new HNLanguage { language_id = 37, language_name = "Hawaiian" });
            LanguageList.Add(new HNLanguage { language_id = 38, language_name = "Hebrew" });
            LanguageList.Add(new HNLanguage { language_id = 39, language_name = "Hindi" });
            LanguageList.Add(new HNLanguage { language_id = 40, language_name = "Hungarian" });
            LanguageList.Add(new HNLanguage { language_id = 41, language_name = "Icelandic" });
            LanguageList.Add(new HNLanguage { language_id = 42, language_name = "Indonesian" });
            LanguageList.Add(new HNLanguage { language_id = 43, language_name = "Irish" });
            LanguageList.Add(new HNLanguage { language_id = 44, language_name = "Italian" });
            LanguageList.Add(new HNLanguage { language_id = 45, language_name = "Japanese" });
            LanguageList.Add(new HNLanguage { language_id = 46, language_name = "Javanese" });
            LanguageList.Add(new HNLanguage { language_id = 47, language_name = "Kannada" });
            LanguageList.Add(new HNLanguage { language_id = 48, language_name = "Kazakh" });
            LanguageList.Add(new HNLanguage { language_id = 49, language_name = "Khmer" });
            LanguageList.Add(new HNLanguage { language_id = 50, language_name = "Klingon" });
            LanguageList.Add(new HNLanguage { language_id = 51, language_name = "Korean" });
            LanguageList.Add(new HNLanguage { language_id = 52, language_name = "Kurdish" });
            LanguageList.Add(new HNLanguage { language_id = 53, language_name = "Laotian" });
            LanguageList.Add(new HNLanguage { language_id = 54, language_name = "Latin" });
            LanguageList.Add(new HNLanguage { language_id = 55, language_name = "Latvian" });
            LanguageList.Add(new HNLanguage { language_id = 56, language_name = "Limburgish" });
            LanguageList.Add(new HNLanguage { language_id = 57, language_name = "Lithuanian" });
            LanguageList.Add(new HNLanguage { language_id = 58, language_name = "Macedonian" });
            LanguageList.Add(new HNLanguage { language_id = 59, language_name = "Malagasy" });
            LanguageList.Add(new HNLanguage { language_id = 60, language_name = "Malay" });
            LanguageList.Add(new HNLanguage { language_id = 61, language_name = "Malayalam" });
            LanguageList.Add(new HNLanguage { language_id = 62, language_name = "Malay" });
            LanguageList.Add(new HNLanguage { language_id = 63, language_name = "Maltese" });
            LanguageList.Add(new HNLanguage { language_id = 64, language_name = "Marathi" });
            LanguageList.Add(new HNLanguage { language_id = 65, language_name = "Mongolian" });
            LanguageList.Add(new HNLanguage { language_id = 66, language_name = "Nepali" });
            LanguageList.Add(new HNLanguage { language_id = 67, language_name = "Northern Sami" });
            LanguageList.Add(new HNLanguage { language_id = 68, language_name = "Norwegian (Bokmål)" });
            LanguageList.Add(new HNLanguage { language_id = 69, language_name = "Norwegian (Nynorsk)" });
            LanguageList.Add(new HNLanguage { language_id = 70, language_name = "Pashto" });
            LanguageList.Add(new HNLanguage { language_id = 71, language_name = "Persian" });
            LanguageList.Add(new HNLanguage { language_id = 72, language_name = "Polish" });
            LanguageList.Add(new HNLanguage { language_id = 73, language_name = "Portuguese (Brazil)" });
            LanguageList.Add(new HNLanguage { language_id = 74, language_name = "Portuguese (Portugal)" });
            LanguageList.Add(new HNLanguage { language_id = 75, language_name = "Punjabi" });
            LanguageList.Add(new HNLanguage { language_id = 76, language_name = "Quechua" });
            LanguageList.Add(new HNLanguage { language_id = 77, language_name = "Romanian" });
            LanguageList.Add(new HNLanguage { language_id = 78, language_name = "Romansh" });
            LanguageList.Add(new HNLanguage { language_id = 79, language_name = "Russian" });
            LanguageList.Add(new HNLanguage { language_id = 80, language_name = "Sanskrit" });
            LanguageList.Add(new HNLanguage { language_id = 81, language_name = "Serbian" });
            LanguageList.Add(new HNLanguage { language_id = 82, language_name = "Simplified Chinese (China)" });
            LanguageList.Add(new HNLanguage { language_id = 83, language_name = "Slavic" });
            LanguageList.Add(new HNLanguage { language_id = 84, language_name = "Slovak" });
            LanguageList.Add(new HNLanguage { language_id = 85, language_name = "Slovenian" });
            LanguageList.Add(new HNLanguage { language_id = 86, language_name = "Somali" });
            LanguageList.Add(new HNLanguage { language_id = 87, language_name = "Spanish (Chile)" });
            LanguageList.Add(new HNLanguage { language_id = 88, language_name = "Spanish (Colombia)" });
            LanguageList.Add(new HNLanguage { language_id = 89, language_name = "Spanish (Mexico)" });
            LanguageList.Add(new HNLanguage { language_id = 90, language_name = "Spanish (Spain)" });
            LanguageList.Add(new HNLanguage { language_id = 91, language_name = "Spanish (Venezuela)" });
            LanguageList.Add(new HNLanguage { language_id = 92, language_name = "Kiswahili" });
            LanguageList.Add(new HNLanguage { language_id = 93, language_name = "Swedish" });
            LanguageList.Add(new HNLanguage { language_id = 94, language_name = "Syriac" });
            LanguageList.Add(new HNLanguage { language_id = 95, language_name = "Tagalog" });
            LanguageList.Add(new HNLanguage { language_id = 96, language_name = "Tajik" });
            LanguageList.Add(new HNLanguage { language_id = 97, language_name = "Tamil" });
            LanguageList.Add(new HNLanguage { language_id = 98, language_name = "Tatar" });
            LanguageList.Add(new HNLanguage { language_id = 99, language_name = "Telugu" });
            LanguageList.Add(new HNLanguage { language_id = 100, language_name = "Thai" });
            LanguageList.Add(new HNLanguage { language_id = 101, language_name = "Tibet" });
            LanguageList.Add(new HNLanguage { language_id = 102, language_name = "Traditional Chinese (Hong Kong)" });
            LanguageList.Add(new HNLanguage { language_id = 103, language_name = "Traditional Chinese (Taiwan)" });
            LanguageList.Add(new HNLanguage { language_id = 104, language_name = "Tongan" });
            LanguageList.Add(new HNLanguage { language_id = 105, language_name = "Turkish" });
            LanguageList.Add(new HNLanguage { language_id = 106, language_name = "Ukrainian" });
            LanguageList.Add(new HNLanguage { language_id = 107, language_name = "Urdu" });
            LanguageList.Add(new HNLanguage { language_id = 108, language_name = "Uzbek" });
            LanguageList.Add(new HNLanguage { language_id = 109, language_name = "Vietnamese" });
            LanguageList.Add(new HNLanguage { language_id = 110, language_name = "Welsh" });
            LanguageList.Add(new HNLanguage { language_id = 111, language_name = "Xhosa" });
            LanguageList.Add(new HNLanguage { language_id = 112, language_name = "Ylanguage_iddish" });
            LanguageList.Add(new HNLanguage { language_id = 113, language_name = "Zulu" });
            #endregion
            lstLanguage.ItemsSource = LanguageList;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void lstLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsLanguageSelected = lstLanguage.SelectedItem != null;            
        }
    }
}
