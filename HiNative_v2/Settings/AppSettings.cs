using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace HiNative.Settings
{
    public class AppSettings
    {
        private const string _themePath = "SelectedTheme";

        public ElementTheme SelectedTheme { get; set; }
        public bool AllowToast { get; set; }
        public bool AllowTile { get; set; }

        public void LoadSettings()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var theme = localSettings.Values[_themePath] as ApplicationTheme?;
        }
    }
}
