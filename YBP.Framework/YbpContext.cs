using System;
using System.Collections.Generic;

namespace YBP.Framework
{
    public class YbpContext
    {
        private string _id;

        public string Id {
            get => _id;
            set {
                if (!string.IsNullOrWhiteSpace(_id) && _id != value)
                    throw new ApplicationException("Can't update id of existing YPB process instance");
                _id = value;
            } 
        }

        public YbpFlagsDictionary Flags => new YbpFlagsDictionary { };
    }

    public class YbpContext<T>: YbpContext
        where T: YbpProcessBase, new()
    {

    }
}
