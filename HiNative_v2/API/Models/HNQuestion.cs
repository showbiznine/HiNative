using System.Collections.ObjectModel;

namespace HiNative.API.Models
{
    public class HNQuestion
    {
        public int? id { get; set; }
        public int? language_id { get; set; }
        public int? country_id { get; set; }
        public string language_name { get; set; }
        public string type { get; set; }
        public string content { get; set; }

        public string text { get; set; }

        public string supplement { get; set; }
        public object featured_answer_id { get; set; }
        public int? views_count { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int? answers_count { get; set; }
        public int? image_id { get; set; }
        public string image_url { get; set; }
        public int? audio_id { get; set; }
        public string audio_url { get; set; }
        public object original_image_url { get; set; }
        public bool close { get; set; }
        public string url { get; set; }
        public object bookmark_id { get; set; }
        public int? prior { get; set; }
        public int? subscribed { get; set; }
        public ObservableCollection<HNKeyword> keywords { get; set; }
        public ObservableCollection<HNQuestionKeywordsAttribute> question_keywords_attributes { get; set; }
        public HNUserProfile user { get; set; }
        public string timeago { get; set; }
        public ObservableCollection<HNAnswer> answers { get; set; }
    }

    public class HNQuestionKeywordsAttribute
    {
        public int? _destroy { get; set; }
        public string name { get; set; }
        public int? id { get; set; }
        public bool choiced { get; set; }
    }
}