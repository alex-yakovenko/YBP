using System;
using System.Collections.Generic;

namespace YBP.Framework
{
    public class YbpFlagsDictionary: Dictionary<string, bool>
    {
        public YbpFlagsDictionary()
        {
        }

        public YbpFlagsDictionary(Dictionary<string, bool> dict): base(dict)
        {
        }

        public bool AlreadyExecuted<TAction>()
        {
            return AlreadyExecuted(typeof(TAction));
        }

        public bool AlreadyExecuted(Type actionType)
        {
            var key = $"{actionType.Name}_Executed";
            return ContainsKey(key) && this[key];
        }

        internal void MarkAlreadyExecuted(Type actionType)
        {
            var key = $"{actionType.Name}_Executed";
            this[key] = true;
        }
    }
}
