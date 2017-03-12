using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiNative.API.Models
{
    public class HNActivities
    {
        public ObservableCollection<HNActivity> activities { get; set; }
        public HNPagination pagination { get; set; }
    }
}
