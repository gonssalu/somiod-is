using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SOMIOD.Helpers;

namespace SOMIOD.Controllers
{
    public class SomiodController : ApiController
    {
        #region Application

        // GET: api/Somiod
        [Route("api/Somiod")]
        public HttpResponseMessage GetApplications()
        {
            try {
                var applications = DbHelper.GetApplications();
                var xmlDoc = RequestHelper.Serialize(applications);
                return RequestHelper.CreateMessage(Request, xmlDoc);
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        // GET: api/Somiod/application
        [Route("api/Somiod/{application}")]
        public HttpResponseMessage GetApplication(string application)
        {
            try {
                var app = DbHelper.GetApplication(application);
                return RequestHelper.CreateMessage(Request, app);
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        // POST: api/Somiod
        [Route("api/Somiod")]
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
