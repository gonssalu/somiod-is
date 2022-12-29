using SOMIOD.Exceptions;
using System;
using System.Net;
using System.Net.Http;

namespace SOMIOD.Helpers
{
    public class RequestHelper
    {
        public static HttpResponseMessage CreateError(HttpRequestMessage request, Exception exception)
        {
            var httpStatusCode = HttpStatusCode.InternalServerError;

            if (exception is ModelNotFoundException)
                httpStatusCode = HttpStatusCode.NotFound;

            if (exception is UnprocessableEntityException)
                httpStatusCode = (HttpStatusCode) 422;

            return request.CreateErrorResponse(httpStatusCode, exception.Message);
        }

        public static HttpResponseMessage CreateMessage(HttpRequestMessage request, object obj)
        {
            return request.CreateResponse(HttpStatusCode.OK, obj, "application/xml");
        }
    }
}
