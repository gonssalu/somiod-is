using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Xml.Serialization;
using SOMIOD.Exceptions;

namespace SOMIOD.Helpers
{
    public class BrokerException : Exception
    {
        public BrokerException() : base("An unknown message broker error has happened")
        {
        }
        public BrokerException(string message) : base(message)
        {
        }
    }
}
