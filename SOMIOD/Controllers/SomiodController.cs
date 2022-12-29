using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using SOMIOD.Helpers;

namespace SOMIOD.Controllers
{
    public class SomiodController : ApiController
    {
        #region Application

        // GET: api/Somiod
        [Route("api/Somiod")]
        public FormattedContentResult<string> GetApplications()
        {
            try {
                var applications = DbHelper.GetApplications();

                var xmlDoc = new XmlDocument();
                var serializer = new XmlSerializer(applications.GetType());

                using (var ms = new MemoryStream()) {
                    serializer.Serialize(ms, applications);
                    ms.Position = 0;
                    xmlDoc.Load(ms);
                    // return Request.CreateResponse(HttpStatusCode.OK, xmlDoc.InnerXml);
                    return Content(HttpStatusCode.OK, xmlDoc.InnerXml, Configuration.Formatters.XmlFormatter);
                }
            }
            catch (Exception e) {
                // return RequestHelper.CreateError(Request, e);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
        }

        // GET: api/Somiod/application
        [Route("api/Somiod/application/{id}")]
        public HttpResponseMessage GetApplication(string application)
        {
            try {
                var app = DbHelper.GetApplication(application);
                return Request.CreateResponse(HttpStatusCode.OK, app);
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        // POST: api/Somiod
        [Route("api/Somiod/application")]
        public HttpResponseMessage Post([FromBody] string name)
        {
            try {
                DbHelper.CreateApplication(name);
                return RequestHelper.CreateMessage(Request, "Application created");
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        // public void Put(int id, [FromBody] string value)
        // {
        // }
        //// PUT: api/Somiod/application
        [Route("api/Somiod/{application}")]
        public HttpResponseMessage Put(string application, [FromBody] string newName)
        {
            try {
                DbHelper.UpdateApplication(application, newName);
                return Request.CreateResponse(HttpStatusCode.OK, "Application updated");
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        // DELETE: api/Somiod/application
        [Route("api/Somiod/{application}")]
        public HttpResponseMessage Delete(string application)
        {
            try {
                DbHelper.DeleteApplication(application);
                return Request.CreateResponse(HttpStatusCode.OK, "Application was deleted");
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        #endregion

        #region Module

        // DELETE: api/Somiod/application/module
        [Route("api/Somiod/{application}/{module}")]
        public HttpResponseMessage Delete(string application, string module)
        {
            try {
                DbHelper.DeleteModule(application, module);
                return Request.CreateResponse(HttpStatusCode.OK, "Module was deleted");
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        #endregion
    }
}
