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
app.MapIdentityApi<ApplicationUser>();
app.MapControllers();

app.Run();
