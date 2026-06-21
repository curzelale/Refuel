using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;
using Refuel.Application;
using Refuel.Persistence;
using Refuel.Persistence.Identity;
using RefuelAPI.Middleware;
using RefuelAPI.Services;
using Scalar.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
});

builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options =>
    builder.Configuration.GetSection("Identity").Bind(options))
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<RefuelDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddHostedService<AdminSeederService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Refuel API",
        Version = "v1",
        Description = "**Refuel API** allows you to easily manage car refueling. " +
                      "Track fuel stops for your vehicles, record costs and quantities, and monitor consumption and fuel prices over time."
    });

    options.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        In = ParameterLocation.Header,
        Description = "Enter the bearer access token obtained from POST /login"
    });

    options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("BearerAuth", doc, null),
            new List<string>()
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Refuel.Application.xml"), true);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(opt => opt.RouteTemplate = "openapi/{documentName}.json");
    app.MapScalarApiReference("/docs", options =>
    {
        options.WithTitle("Refuel API")
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .AddPreferredSecuritySchemes("BearerAuth")
            .AddHttpAuthentication("BearerAuth", auth =>
            {
                auth.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";
            });
    });
}

app.UseCors(p => 
    p.SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
var allowRegistration = app.Configuration.GetValue<bool>("AllowRegistration");
app.MapIdentityApi<ApplicationUser>()
   .AddEndpointFilter(async (context, next) =>
   {
       if (!allowRegistration && context.HttpContext.Request.Path.StartsWithSegments("/register"))
           return Results.Problem("Registration is disabled.", statusCode: StatusCodes.Status403Forbidden);
       return await next(context);
   });
app.MapControllers();

app.Run();
