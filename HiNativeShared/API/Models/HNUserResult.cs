using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiNativeShared.API.Models
{
    public class HNUserResult
    {
        public string token { get; set; }
        public HNUserProfile profile { get; set; }
    }
}
