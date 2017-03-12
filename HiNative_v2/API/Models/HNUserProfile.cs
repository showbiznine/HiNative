using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml.Media.Imaging;

namespace HiNative.API.Models
{
    public class HNUserProfile : INotifyPropertyChanged
    {
        public string name { get; set; }
        public int? id { get; set; }
        public string self_introduction { get; set; }
        public string url { get; set; }
        public string image_url { get; set; }
        public BitmapImage profile_image { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int? country_id { get; set; }
        public string big_image_url { get; set; }
        public string country { get; set; }
        public int? number_of_questions { get; set; }
        public int? number_of_answers { get; set; }
        public int? number_of_likes { get; set; }
        public int? number_of_featured_answers { get; set; }
        public int? number_of_quick_responses { get; set; }
        public int? number_of_bookmarks { get; set; }
        public int? number_of_homework { get; set; }
        public bool? teacher { get; set; }
        public int? paid_student { get; set; }
        public string premium { get; set; }
        public object premium_platform { get; set; }
        public string paid_correction_platform { get; set; }
        public int? num_tickets { get; set; }
        public HNUserAttributes user_attributes { get; set; }

        public string native_language { get; set; }
        public ObservableCollection<HNLanguage> native_languages { get; set; }
        public ObservableCollection<HNLanguage> study_languages { get; set; }
        public ObservableCollection<HNCountry> user_interested_countries_attributes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}