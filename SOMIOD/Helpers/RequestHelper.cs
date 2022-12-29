using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Xml.Serialization;
using SOMIOD.Exceptions;
using SOMIOD.Models;
using static System.Net.Mime.MediaTypeNames;

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
            var xmlDoc = Serialize(obj);
            return request.CreateResponse(HttpStatusCode.OK, xmlDoc, "application/xml");
        }

        private static XmlDocument Serialize(object obj)
        {
            var xmlDoc = new XmlDocument();
            var serializer = new XmlSerializer(obj.GetType());

            using (var ms = new MemoryStream()) {
                serializer.Serialize(ms, obj);
                ms.Position = 0;
                xmlDoc.Load(ms);
                return xmlDoc;
            }
        }
    }
}
