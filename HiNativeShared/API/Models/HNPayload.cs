using System.Collections.ObjectModel;

namespace HiNativeShared.API.Models
{
    public class HNPayload
    {
        public string activity_type { get; set; }
        public int id { get; set; }
        public int? language_id { get; set; }
        public string type { get; set; }
        public string content { get; set; }
        public object supplement { get; set; }
        public int? featured_answer_id { get; set; }
        public int views_count { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int answers_count { get; set; }
        public ObservableCollection<HNKeyword> keywords { get; set; }
        public HNUserProfile user { get; set; }
        public string timeago { get; set; }
        public string text { get; set; }
    }
}