using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOMIOD.Exceptions
{
    public class ModelNotFoundException : Exception
    {
        public ModelNotFoundException() : base("Resource not found")
        {
        }
        public ModelNotFoundException(string message) : base(message + " not found")
        {
        }
    }
}
