using System.ComponentModel;

namespace HiNative.API.Models
{
    public class HNAnswer : INotifyPropertyChanged
    {
        public bool isUser { get; set; }
        public bool isOP { get; set; }
        public int id { get; set; }
        public string content { get; set; }
        public int likes_count { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public bool quick { get; set; }
        public bool liked { get; set; }
        public string timeago { get; set; }
        public int? image_id { get; set; }
        public string image_url { get; set; }
        public int? audio_id { get; set; }
        public string audio_url { get; set; }
        public int? stamp_id { get; set; }
        public int? bookmark_id { get; set; }
        public HNUserProfile user { get; set; }
        public HNKeyword choiced_keyword { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}