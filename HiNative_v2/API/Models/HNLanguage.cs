namespace HiNative.API.Models
{
    public class HNLanguage
    {
        public int? _destroy { get; set; }
        public int? id { get; set; }
        public bool? native { get; set; }
        public int? language_id { get; set; }
        public int learning_level_id { get; set; }
        public string language_name { get; set; }
    }
}