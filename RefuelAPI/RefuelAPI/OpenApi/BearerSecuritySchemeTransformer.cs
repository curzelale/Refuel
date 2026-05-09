using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace RefuelAPI.OpenApi;

public class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();

        if (document.Components.SecuritySchemes is null)
            document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            In = ParameterLocation.Header,
            Description = "Enter the bearer access token obtained from POST /login"
        };

        return Task.CompletedTask;
    }
}
