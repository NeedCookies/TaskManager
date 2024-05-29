using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Abstractions;
using TaskManagerApi.Models;
using TaskManagerApi.Repository;

namespace TaskManagerApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            // Add services to the container.

            builder.Services.AddControllersWithViews();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "TaskManagerApi", Version = "v1" });
            });

            builder.Services.AddDbContext<TaskManagerDbContext>(
                options =>
                {
                    options.UseNpgsql(configuration.GetConnectionString(nameof(TaskManagerDbContext)));
                });

            builder.Services.AddScoped<ITaskManagerRepository, TaskManagerRepository>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                options => builder.Configuration.Bind("JwtSettings", options))
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                options => builder.Configuration.Bind("CookieSettings", options));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManagerApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=TaskItems}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
