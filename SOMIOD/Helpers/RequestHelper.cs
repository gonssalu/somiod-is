using SOMIOD.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace SOMIOD.Helpers
{
    public class RequestHelper
    {
        public static HttpResponseMessage GenerateError(HttpRequestMessage request, Exception exception) {
            
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

            if (exception is ModelNotFoundException)
                httpStatusCode = HttpStatusCode.NotFound;
            if (exception is UnprocessableEntityException)
                httpStatusCode = (HttpStatusCode)422;
            
            return request.CreateErrorResponse(httpStatusCode, exception.Message);
        }
    }
}