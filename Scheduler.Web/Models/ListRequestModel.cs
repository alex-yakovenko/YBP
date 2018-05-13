using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.Web.Models
{
    public class ListRequest<TOrder>
        where TOrder: struct
    {
        public int SkipCount { get; set; }
        public int TakeCount { get; set; }
        public TOrder? SortOrder { get; set; }
        public bool SortDesc { get; set; }

        public ListRequest()
        {
            TakeCount = 50;
        }
    }
}
