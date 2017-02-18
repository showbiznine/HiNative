namespace HiNativeShared.API.Models
{
    public class HNActivity
    {
        public int id { get; set; }
        public bool read_flag { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string action_user_image_url { get; set; }
        public string action_user_name { get; set; }
        public int action_user_id { get; set; }
        public string timeago { get; set; }
        public HNPayload payload { get; set; }
    }
}