using System;

namespace SOMIOD.Exceptions
{
    public class ModelNotFoundException : Exception
    {
        public ModelNotFoundException(string message, bool suffix = true) : base(message + (suffix?" not found":""))
        {
        }
    }
}
