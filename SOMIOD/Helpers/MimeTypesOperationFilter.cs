using System.Web.Http.Description;
using Swashbuckle.Swagger;

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
