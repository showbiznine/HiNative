using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiNative.API.Models
{
    public class HNNewUserContainer
    {
        public HNNewUser user { get; set; }
    }

    public class HNNewUser
    {
        public string email { get; set; }
        public HNMailSettingAttributes mail_setting_attributes { get; set; }
        public string name { get; set; }
        public int? country_id { get; set; }
        public ObservableCollection<HNNotificationsAttribute> notifications_attributes { get; set; }
        public string password { get; set; }
        public string password_confirmation { get; set; }
        public string platform { get; set; }
        public HNProfileAttributes profile_attributes { get; set; }
        public ObservableCollection<HNLanguage> native_languages_attributes { get; set; }
        public ObservableCollection<HNLanguage> study_languages_attributes { get; set; }
        public ObservableCollection<HNCountry> user_interested_countries_attributes { get; set; }
        public string terms_of_use { get; set; }
    }

    public class HNMailSettingAttributes
    {
        public bool info { get; set; }
    }

    public class HNNotificationsAttribute
    {
        public string platform { get; set; }
        public string token { get; set; }
    }

    public class HNProfileAttributes
    {
        public int interface_id { get; set; }
    }
}
