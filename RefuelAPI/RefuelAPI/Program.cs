using Microsoft.AspNetCore.Identity;
using Refuel.Application;
using Refuel.Persistence;
using Refuel.Persistence.Identity;
using RefuelAPI.Middleware;
using RefuelAPI.OpenApi;
using RefuelAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddPersistenceServices();
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

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

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
