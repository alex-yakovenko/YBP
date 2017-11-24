using System;

namespace YBP.Framework
{
    public abstract class YbpProcessBase
    {

        private string _className;

        public virtual string Prefix => _className.Substring(0, 16);
        public virtual string Name => _className;
        public virtual string Title => _className;

        public YbpProcessBase()
        {
            _className = GetType().Name;
        }

    }
}
