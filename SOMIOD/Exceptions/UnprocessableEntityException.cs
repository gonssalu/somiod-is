using System;

namespace SOMIOD.Exceptions
{
    public class UnprocessableEntityException : Exception
    {
        public UnprocessableEntityException() : base("Your request could not be fulfilled")
        {
        }
        public UnprocessableEntityException(string message) : base(message)
        {
        }
    }
}
