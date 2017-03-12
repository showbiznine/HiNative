using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiNative.API.Models
{
    public class HNAnswerResult
    {
        public HNAnswer answer { get; set; }
        public HNImage image { get; set; }
        public HNAudio audio { get; set; }
    }
}
