using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiNative.API.Models
{
    public class HNLogin
    {
        public HNCredintals user { get; set; }
    }

    public class HNCredintals
    {
        public string device_token { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string platform { get; set; }
    }
}
