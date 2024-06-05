using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using TaskManagerApi.Abstractions;
using TaskManagerApi.Authentication;
using TaskManagerApi.Data;
using TaskManagerApi.Models;
using TaskManagerApi.Repository;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog.Sinks.OpenSearch;
using AutoRegisterTemplateVersion = Serilog.Sinks.OpenSearch.AutoRegisterTemplateVersion;
using CertificateValidations = OpenSearch.Net.CertificateValidations;
using System.Net;


namespace TaskManagerApi
{
    public class Program
    {
        public static string _AdminPassword { get; private set; } = "";
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            string _signingSecurityKey = configuration.GetValue<string>("SecretSettings:SigningSecurityKey");
            _AdminPassword = configuration.GetValue<string>("SecretSettings:AdminPassword");
            var signingKey = new SigningSymmetricKey(_signingSecurityKey);

            builder.Services.AddSingleton<IJwtSigningEncodingKey>(signingKey);


            builder.Services.AddControllersWithViews();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            builder.Services.AddDbContext<TaskManagerDbContext>(
                options =>
                {
                    options.UseNpgsql(configuration.GetConnectionString(nameof(TaskManagerDbContext)));
                });

            builder.Services.AddScoped<ITaskManagerRepository, TaskManagerRepository>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey.GetKey(), // Используйте ваш метод GetKey()

                        ValidateIssuer = true,
                        ValidIssuer = "DemoApp",

                        ValidateAudience = true,
                        ValidAudience = "DemoAppClient",

                        ValidateLifetime = false,

                        ClockSkew = TimeSpan.FromSeconds(5)
                    };
                });

            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            builder.Logging.ClearProviders();
            Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));
            ServicePointManager.ServerCertificateValidationCallback = (o, certificate, chain, errors) => true;
            ServicePointManager.ServerCertificateValidationCallback = CertificateValidations.AllowAll;

            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.OpenSearch(new OpenSearchSinkOptions(new Uri("https://localhost:9200"))
                {
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                    MinimumLogEventLevel = LogEventLevel.Verbose,
                    TypeName = "_doc",
                    InlineFields = false,
                    ModifyConnectionSettings = x =>
                        x.BasicAuthentication("admin", "bars123@superMyPassword")
                            .ServerCertificateValidationCallback(CertificateValidations.AllowAll)
                            .ServerCertificateValidationCallback((o, certificate, chain, errors) => true),
                    IndexFormat = "my-index-{0:yyyy.MM.dd}",
                })
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
                //c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManagerApi v1")
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Authentication}/{action=Index}/{id?}");
            /*app.MapControllerRoute(
                name: "default",
                pattern: "{controller=TaskItems}/{action=Index}/{id?}");*/

            app.Run();
            app.Logger.LogInformation("starting up");
        }
    }
}
