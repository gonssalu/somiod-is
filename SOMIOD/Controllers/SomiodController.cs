﻿using System;
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

        [Route("api/somiod/{appName}/modules")]
        public HttpResponseMessage GetModules(string appName)
        {
            try {
                var modules = DbHelper.GetModules(appName);
                return RequestHelper.CreateMessage(Request, modules);
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        [Route("api/somiod/{appName}/modules/{moduleName}")]
        public HttpResponseMessage GetModule(string appName, string moduleName)
        {
            try {
                var module = DbHelper.GetModule(appName, moduleName);
                return RequestHelper.CreateMessage(Request, module);
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        [Route("api/somiod/{application}")]
        public HttpResponseMessage Post(string application, [FromBody] Module newModule)
        {
            // return null;
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

        [Route("api/somiod/{appName}/modules/{moduleName}")]
        public HttpResponseMessage Put(string appName, string moduleName, [FromBody] Module newModuleDetails)
        {
            try {
                if (newModuleDetails == null)
                    throw new UnprocessableEntityException("You must provide a module with a name in the correct xml format");

                if (string.IsNullOrEmpty(newModuleDetails.Name))
                    throw new UnprocessableEntityException("You must include the updated name of the module");

                string newName = newModuleDetails.Name;
                DbHelper.UpdateModule(appName, moduleName, newName);
                return Request.CreateResponse(HttpStatusCode.OK, "Module updated");
            }
            catch (Exception e) {
                return RequestHelper.CreateError(Request, e);
            }
        }

        [Route("api/somiod/{application}/{module}")]
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
