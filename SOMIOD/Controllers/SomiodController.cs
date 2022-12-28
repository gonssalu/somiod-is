using SOMIOD.Helper;
using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SOMIOD.Controllers
{
    public class SomiodController : ApiController
    {

        // POST: api/Somiod
        public HttpResponseMessage Post([FromBody] string name)
        {
            try{
                DbHelper.CreateApplication(name);
                return Request.CreateResponse(HttpStatusCode.OK, "Application created");
            }
            catch(Exception e){
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // GET: api/Somiod
        public HttpResponseMessage GetApplications()
        {
            try{
                var apps = DbHelper.GetApplications();
                return Request.CreateResponse(HttpStatusCode.OK, apps);
            }catch (Exception e){
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // GET: api/Somiod/application
        public HttpResponseMessage GetApplication(string application)
        {
            try
            {
                var app = DbHelper.GetApplication(application);
                if (app == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found");
                return Request.CreateResponse(HttpStatusCode.OK, app);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        //// PUT: api/Somiod/5
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/Somiod/5
        //public void Delete(int id)
        //{
        //}
    }
}
