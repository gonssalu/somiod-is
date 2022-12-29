using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SOMIOD.Exceptions;
using SOMIOD.Helpers;

namespace SOMIOD.Controllers
{
    public class SomiodController : ApiController
    {

        // POST: api/Somiod
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

        // GET: api/Somiod
        public HttpResponseMessage GetApplications()
        {
            try {
                var apps = DbHelper.GetApplications();
                return RequestHelper.CreateMessage(Request, apps);
            }
            catch (Exception e)
            {
                return RequestHelper.CreateError(Request, e);
            }
        }

        // GET: api/Somiod/application
        public HttpResponseMessage GetApplication(string application)
        {
            try {
                var app = DbHelper.GetApplication(application);
                return Request.CreateResponse(HttpStatusCode.OK, app);
            }
            catch (Exception e)
            {
                return RequestHelper.CreateError(Request, e);
            }
        }

        // public void Put(int id, [FromBody] string value)
        // {
        // }
        //// PUT: api/Somiod/application
        public HttpResponseMessage PutApplication(string application, [FromBody] string newName)
        {
            try {
                DbHelper.UpdateApplication(application, newName);
                return Request.CreateResponse(HttpStatusCode.OK, "Application updated");
            }
            catch (Exception e)
            {
                return RequestHelper.CreateError(Request, e);
            }
        }

        // DELETE: api/Somiod/application
        public HttpResponseMessage Delete(string application)
        {
            try {
                DbHelper.DeleteApplication(application);
                return Request.CreateResponse(HttpStatusCode.OK, "Application was deleted");
            }
            catch (Exception e)
            {
                return RequestHelper.CreateError(Request, e);
            }
        }

        // DELETE: api/Somiod/application/module
        public HttpResponseMessage Delete(string application, string module)
        {
            try
            {
                DbHelper.DeleteModule(application, module);
                return Request.CreateResponse(HttpStatusCode.OK, "Module was deleted");
            }
            catch (Exception e)
            {
                return RequestHelper.CreateError(Request, e);
            }
        }
    }
}
