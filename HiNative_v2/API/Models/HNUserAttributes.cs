using System.Collections.ObjectModel;

namespace HiNative.API.Models
{
    public class HNUserAttributes
    {
        public int id { get; set; }
        public string name { get; set; }
        public ObservableCollection<HNLanguage> native_languages_attributes { get; set; }
        public ObservableCollection<HNLanguage> study_languages_attributes { get; set; }
        public ObservableCollection<HNCountry> user_interested_countries_attributes { get; set; }
    }
}