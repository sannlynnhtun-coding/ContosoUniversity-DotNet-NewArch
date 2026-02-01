using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ContosoUniversity.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

RegisterServices(builder);

var app = builder.Build();

app.MapDefaultEndpoints();

ConfigureApplication(app);

app.Run();

static void RegisterServices(WebApplicationBuilder builder)
{
    var services = builder.Services;

    services.AddMiniProfiler().AddEntityFramework();

    services.AddContosoUniversityDomain(builder.Configuration.GetConnectionString("db"));

    services.AddRazorPages();

    // services.AddMvc(opt => opt.ModelBinderProviders.Insert(0, new EntityModelBinderProvider()));
}

static void ConfigureApplication(WebApplication app)
{
    app.UseMiniProfiler();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseAuthorization();

    app.MapRazorPages();
}