using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GP.Microservices.Api
{
    /// <summary>
    /// Add required header parameter into swagger config
    /// </summary>
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            if (operation.OperationId.Equals("ApiAccountMeGet"))
            {
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    Description = "Authentication token",
                    In = "header",
                    Type = "string",
                    Required = true,
                    Default = "Bearer {{token}}"
                });
            }
            if (operation.OperationId.Equals("ApiAccountMeDelete"))
            {
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    Description = "Authentication token",
                    In = "header",
                    Type = "string",
                    Required = true,
                    Default = "Bearer {{token}}"
                });
            }

            if (operation.OperationId.StartsWith("ApiRemarks"))
            {
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    Description = "Authentication token",
                    In = "header",
                    Type = "string",
                    Required = true,
                    Default = "Bearer {{token}}"
                });
            }

            if (operation.OperationId.StartsWith("ApiUsers"))
            {
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    Description = "Authentication token",
                    In = "header",
                    Type = "string",
                    Required = true,
                    Default = "Bearer {{token}}"
                });
            }
        }
    }
}