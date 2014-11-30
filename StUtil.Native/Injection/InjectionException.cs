using System;

namespace StUtil.Native.Injection
{
    public class InjectionException : Exception
    {
        public InjectionException(string message)
            : base(message)
        {
        }
    }
}