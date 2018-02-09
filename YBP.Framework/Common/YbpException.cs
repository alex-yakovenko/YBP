using System;

namespace YBP.Framework
{
    public class YbpException: Exception
    {
        public YbpException(string message) : base(message)
        {
        }
    }
}
