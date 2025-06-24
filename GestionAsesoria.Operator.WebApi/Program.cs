using Asp.Versioning;
using GestionAsesoria.Operator.Application.Extensions;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Application.Interfaces.Services.ExternalRequest;
using GestionAsesoria.Operator.Infrastructure.Persistence.Configurations;
using GestionAsesoria.Operator.Infrastructure.Persistence.Extensions;
using GestionAsesoria.Operator.Infrastructure.Persistence.Repositories;
using GestionAsesoria.Operator.Infrastructure.Persistence.Services.ExternalRequest;
using GestionAsesoria.Operator.Infrastructure.Shared.Services;
using GestionAsesoria.Operator.WebApi.Extensions;
using GestionAsesoria.Operator.WebApi.Filters;
using GestionAsesoria.Operator.WebApi.Managers.Preferences;
using GestionAsesoria.Operator.WebApi.Middlewares;
using Google.Apis.Auth.AspNetCore3;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using GestionAsesoria.Operator.Application.Services;
using GestionAsesoria.Operator.Infrastructure.Persistence.Services;

try
{
    var builder = WebApplication.CreateBuilder(args);
    var _configuration = builder.Configuration;

    var Cors = "Cors";

    builder.Services.AddHttpClient();


    //Company
    builder.Services.AddScoped<IActorCompanyService, ActorCompanyService>();

    //follow
    builder.Services.AddScoped<IFollowService, FollowService>();

    builder.Services.AddScoped<IFollowRepositoryAsync, FollowRepositoryAsync>();

    // Registrar EmailSettings desde appsettings.json
    builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

    builder.Services.AddTransient<IEmailService, EmailService>();

    string configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

    builder.Services.AddSingleton<IMessageService>(provider =>
    {
        var env = provider.GetRequiredService<IHostEnvironment>();
        var jsonFilePath = Path.Combine(env.ContentRootPath, "messages.json");

        return new MessageService(
            env,
            provider.GetRequiredService<ILogger<MessageService>>(),
            jsonFilePath
        );
    });

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
        options.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddGoogleOpenIdConnect(options =>
    {
        options.ClientId = _configuration["GoogleAuth:ClientId"];
        options.ClientSecret = _configuration["GoogleAuth:ClientSecret"];
        options.CallbackPath = "/signin-google";
    });

    builder.Services.AddScoped<ITeacherService, TeacherService>();


    ////Configuración de Google Calendar
    builder.Services.Configure<GoogleCalendarOptions>
    (builder.Configuration.GetSection("GoogleCalendarOptions"));
    //builder.Services.AddScoped<IGoogleCalendarService, GoogleCalendarService>();

    builder.Configuration.AddEnvironmentVariables();

    builder.Services.AddScoped<IPreProfessionalInternshipRepositoryAsync, PreProfessionalInternshipRepositoryAsync>();
    builder.Services.AddScoped<IPreProfessionalInternshipService, IPreProfessionalInternshipService>();

    builder.Services.AddForwarding(_configuration);
    builder.Services.AddCurrentUserService();
    builder.Services.AddSerialization();
    builder.Services.AddDatabase(_configuration);
    builder.Services.AddServerStorage();
    builder.Services.AddScoped<ServerPreferenceManager>();
    builder.Services.AddIdentity();
    builder.Services.AddJwtAuthentication(builder.Services.GetApplicationSettings(_configuration));
    builder.Services.AddSignalR();
    builder.Services.AddApplicationLayer();
    builder.Services.AddApplicationServices();
    builder.Services.AddRepositories();
    builder.Services.AddSharedInfrastructure(_configuration);
    builder.Services.AddMemoryCache();
    builder.Services.AddInfrastructureMappings();
    builder.Services.ConfigureHangfireServices(_configuration);
    builder.Services.AddLogging();
    builder.Services.AddControllers().AddValidators();
    builder.Services.AddRazorPages();

    builder.Services.AddApiVersioning(config =>
    {
        config.DefaultApiVersion = new ApiVersion(1, 0);
        config.AssumeDefaultVersionWhenUnspecified = true;
        config.ReportApiVersions = true;
    });
    builder.Services.AddLazyCache();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.RegisterSwagger();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: Cors,
            builder =>
            {
                builder.WithOrigins("*");
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
    });

    var app = builder.Build();
    app.UseCors(Cors);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP Academico API v1");
            options.RoutePrefix = "swagger";
            options.DocumentTitle = "Documentación de la API";
        });
    }

    app.UseForwarding(_configuration);
    app.UseHttpsRedirection();
    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.UseHangfireDashboard("/jobs", new DashboardOptions
    {
        DashboardTitle = "ERP Academic Consulting",
        Authorization = new[] { new HangfireAuthorizationFilter() }
    });

    app.UseEndpoints();
    app.ConfigureSwagger();
    app.Initialize(_configuration);
    app.Run();
}
catch (Exception ex)
{
    Log.Warning(ex, "An error occurred starting the application");
}
finally
{
    Log.CloseAndFlush();
}