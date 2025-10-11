using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class JsonPatchExampleFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.RequestBody == null) return;
        if (!operation.RequestBody.Content.TryGetValue("application/json-patch+json", out var media)) return;

        var arr = new OpenApiArray
        {
            new OpenApiObject
            {
                ["op"]   = new OpenApiString("replace"),
                ["path"] = new OpenApiString("/fileInfo/extension"),
                ["value"]= new OpenApiString(".png")
            },
            new OpenApiObject
            {
                ["op"]   = new OpenApiString("replace"),
                ["path"] = new OpenApiString("/userId"),
                ["value"]= new OpenApiInteger(88)
            }
        };

        media.Example = arr;
    }
}