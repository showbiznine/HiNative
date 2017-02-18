using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiNativeShared.API.Models
{
    public class HNAttachmentResponse
    {
        public HNImage image { get; set; }
        public HNAudio audio { get; set; }
        public string status { get; set; }
    }
}
