using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiNative.API.Models
{
    public class HNBookmarkRoot
    {
        public HNBookmark bookmark { get; set; }
    }

    public class HNBookmark
    {
        public int id { get; set; }
        public int bookmarkable_id { get; set; }
        public string bookmarkable_type { get; set; }
        public string created_at { get; set; }
        public int question_id { get; set; }
        public object problem_id { get; set; }
        public HNAnswer answer { get; set; }
        public bool first_bookmark { get; set; }
    }
}
