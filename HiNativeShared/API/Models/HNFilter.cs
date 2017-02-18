using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiNativeShared.API.Models
{
    public class HNFilter
    {
        public bool interesting_questions_only { get; set; }
        public bool questions_not_answered_only { get; set; }
        public bool questions_with_audios_only { get; set; }
    }
}
