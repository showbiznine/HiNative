using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiNative.API.Models
{
    public class HNUnreadCount
    {

        public Activities activities { get; set; }

        public class Activities
        {
            public int unread_count { get; set; }
        }

    }
}
