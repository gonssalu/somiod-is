using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SOMIOD.Exceptions;
using SOMIOD.Helpers;
using SOMIOD.Models;

namespace SOMIOD.Controllers
{
    public class SomiodController : ApiController
    {
        #region Application

        [Route("api/somiod")]
        public HttpResponseMessage GetApplications()
        {
            try {
                var applications = DbHelper.GetApplications();
                return RequestHelper.CreateMessage(Request, applications);
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        [Route("api/somiod/{application}")]
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

        [Route("api/somiod")]
        public HttpResponseMessage Post([FromBody] Application newApp)
        {
            try {
                if (newApp == null)
                    throw new UnprocessableEntityException("You must provide an application with a name in the correct xml format");

                if (string.IsNullOrEmpty(newApp.Name))
                    throw new UnprocessableEntityException("You must include the name of your new application");

                DbHelper.CreateApplication(newApp.Name);
                return RequestHelper.CreateMessage(Request, "Application created");
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        [Route("api/somiod/{application}")]
        public HttpResponseMessage Put(string application, [FromBody] Application newAppDetails)
        {
            try {
                if (newAppDetails == null)
                    throw new UnprocessableEntityException("You must provide an application with a name in the correct xml format");

                if (string.IsNullOrEmpty(newAppDetails.Name))
                    throw new UnprocessableEntityException("You must include the updated name of the application");

                string newName = newAppDetails.Name;
                DbHelper.UpdateApplication(application, newName);
                return Request.CreateResponse(HttpStatusCode.OK, "Application updated");
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        [Route("api/somiod/{application}")]
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

        [Route("api/somiod/{application}/modules")]
        public HttpResponseMessage GetModules(string application)
        {
            try {
                var modules = DbHelper.GetModules(application);
                return RequestHelper.CreateMessage(Request, modules);
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        [Route("api/somiod/{application}/{module}")]
        public HttpResponseMessage GetModule(string application, string module)
        {
            try {
                var mod = DbHelper.GetModule(application, module);
                return RequestHelper.CreateMessage(Request, mod);
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        [Route("api/somiod/{application}")]
        public HttpResponseMessage PostModule(string application, [FromBody] Module newModule)
        {
            try {
                if (newModule == null)
                    throw new UnprocessableEntityException("You must provide a module with a name in the correct xml format");

                if (string.IsNullOrEmpty(newModule.Name))
                    throw new UnprocessableEntityException("You must include the name of your new module");

                DbHelper.CreateModule(application, newModule.Name);
                return RequestHelper.CreateMessage(Request, "Module created");
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        [Route("api/somiod/{application}/{module}")]
        public HttpResponseMessage PutModule(string application, string module, [FromBody] Module newModuleDetails)
        {
            try {
                if (newModuleDetails == null)
                    throw new UnprocessableEntityException("You must provide a module with a name in the correct xml format");

                if (string.IsNullOrEmpty(newModuleDetails.Name))
                    throw new UnprocessableEntityException("You must include the updated name of the module");

                string newName = newModuleDetails.Name;
                DbHelper.UpdateModule(application, module, newName);
                return Request.CreateResponse(HttpStatusCode.OK, "Module updated");
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        [Route("api/somiod/{application}/{module}")]
        public HttpResponseMessage DeleteModule(string application, string module)
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

        #region Subscription

        [Route("api/somiod/{application}/{module}/subscriptions")]
        public HttpResponseMessage PostSubscription(string application, string module, [FromBody] Subscription newSubscription)
        {
            try {
                if (newSubscription == null)
                    throw new UnprocessableEntityException("You must provide a subscription with a valid url in the correct xml format");

                if (string.IsNullOrEmpty(newSubscription.Endpoint))
                    throw new UnprocessableEntityException("You must include a url for that subscription");

                DbHelper.CreateSubscription(application, module, newSubscription);
                return RequestHelper.CreateMessage(Request, "Subscription created");
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        [Route("api/somiod/{application}/{module}/subscriptions/{subscription}")]
        public HttpResponseMessage DeleteSubscription(string application, string module, string subscription)
        {
            try {
                DbHelper.DeleteSubscription(application, module, subscription);
                return Request.CreateResponse(HttpStatusCode.OK, "Subscription was deleted");
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        #endregion

        #region Data

        [Route("api/somiod/{application}/{module}")]
        public HttpResponseMessage PostData(string application, string module, [FromBody] Data newData)
        {
            try {
                if (newData == null)
                    throw new UnprocessableEntityException("You must provide a data resource with a valid content in the correct xml format");

                if (string.IsNullOrEmpty(newData.Content))
                    throw new UnprocessableEntityException("You must include a content for that data resource");

                DbHelper.CreateData(application, module, newData.Content);
                return RequestHelper.CreateMessage(Request, "Data resource created");
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        [Route("api/somiod/{application}/{module}/{dataId}")]
        public HttpResponseMessage DeleteData(string application, string module, int dataId)
        {
            try {
                DbHelper.DeleteData(application, module, dataId);
                return Request.CreateResponse(HttpStatusCode.OK, "Data resource was deleted");
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        #endregion
    }
}
