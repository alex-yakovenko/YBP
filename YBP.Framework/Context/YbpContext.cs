using System;
using System.Collections.Generic;

namespace YBP.Framework
{
    public class YbpContext
    {
        public int StoredId { get; internal set; }
        public int StoredActionId { get; internal set; }

        private string _id;

        public string Id {
            get => _id;
            set {
                if (!string.IsNullOrWhiteSpace(_id) && _id != value)
                    throw new ApplicationException("Can't update id of existing YPB process instance");
                _id = value;
            }
        }
        
        public YbpFlagsDictionary Flags  { get; internal set;}
        public YbpSecurityContext Security => new YbpSecurityContext { };
    }

    public class YbpContext<T>: YbpContext
        where T: YbpProcessBase, new()
    {


    }
}
