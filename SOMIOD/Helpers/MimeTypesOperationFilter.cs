using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace SOMIOD.Helpers
{
    public class MimeTypesOperationFilter : IOperationFilter
    {
        private static readonly string[] SupportedAccepts = new[] { "application/xml" };
        private static readonly string[] SupportedContentTypes = new[] { "application/xml" };

        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            operation.consumes = SupportedAccepts;
            operation.produces = SupportedContentTypes;
        }
    }
}