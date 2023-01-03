using System;

namespace SOMIOD.Exceptions
{
    public class BrokerException : Exception
    {
        public BrokerException(string message) : base(message)
        {
        }
    }
}
