using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Infrastructure
{
    public class FileUploadOperation : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var parameterDesc = context.ApiDescription.ParameterDescriptions;
            var hasFileUpload = parameterDesc.Any(x => x.Type == typeof(IFormFile));
            if (hasFileUpload)
            {
                var files = parameterDesc.Where(x => x.Type == typeof(IFormFile)).ToList();
                foreach (var item in files)
                {
                    var param = operation.Parameters.First(x => x.Name == item.Name);
                    operation.Parameters.Remove(param);

                    operation.Parameters.Add(new NonBodyParameter
                    {
                        Name = item.Name,
                        In = "formData",
                        Description = "Upload File",
                        Required = true,
                        Type = "file"
                    });
                }
            }
        }
    }
}
