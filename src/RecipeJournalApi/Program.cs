using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecipeJournalApi.Controllers;
using RecipeJournalApi.Infrastructure;
using SMT.Utilities.Logging;
using System;
using System.Threading.Tasks;

namespace RecipeJournalApi
{
    public class SiteConfig : IDbConfig, IAuthenticationProxyConfiguration
    {
        private readonly IConfiguration _config;

        public SiteConfig(IConfiguration config)
        {
            _config = config;
        }

        public string ConnectionString => _config.GetConnectionString("recipe");
        public string AccountServerUrl => _config.GetSection("AuthenticationConfiguration")["Url"];
        public string AccountIntegrationName => _config.GetSection("AuthenticationConfiguration")["IntegrationName"];
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            
            var loggingConfig = builder.Configuration.GetSection("SmtLoggingConfiguration");

            // Add services to the container.
            builder.Services.AddSingleton<IDbConfig, SiteConfig>();
            builder.Services.AddSingleton<IAuthenticationProxyConfiguration, SiteConfig>();
            builder.Services.AddHttpClient();
            builder.Services.AddSmtLogging(c =>
            {
                c.CurrentLogLevel = Microsoft.Extensions.Logging.LogLevel.Error;
                c.Port = int.Parse(loggingConfig["port"]);
                c.Host = loggingConfig["host"];
                c.Secret = loggingConfig["secret"];
            });
            // builder.Services.AddSmtLoggingEndpoints(c =>
            // {
            //     c.BaseUrl = c.Secret = loggingConfig["endpointBaseUrl"];
            // });
#if DEBUG
            builder.Services.AddSingleton<IAuthenticationProxy, MockAuthProxy>();
            builder.Services.AddSingleton<IRecipeRepository, MockRecipeRepository>();
            builder.Services.AddSingleton<IUserRepository, MockUserRepository>();
            builder.Services.AddSingleton<IShoppingRepository, MockShoppingRepository>();
            builder.Services.AddSingleton<IJournalRepository, MockJournalRepository>();
#else
            builder.Services.AddSingleton<IAuthenticationProxy, AuthenticationProxy>();
            builder.Services.AddSingleton<IShoppingRepository, ShoppingRepository>();
            builder.Services.AddSingleton<IRecipeRepository, RecipeRepository>();
            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.Services.AddSingleton<IJournalRepository, JournalRepository>();
#endif

            builder.Services.AddControllers();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/";
                options.Cookie.Name = "recipe-auth";
                options.Cookie.HttpOnly = false;
                options.Cookie.MaxAge = TimeSpan.FromDays(365);

                options.Events.OnRedirectToAccessDenied
                    = options.Events.OnRedirectToLogout
                    = options.Events.OnRedirectToLogin
                    = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseStaticFiles();

            // app.UseSmtLoggingEndpoints();
            app.UseSmtTracingHeaderInterpreter();

            // Configure the HTTP request pipeline.
            // if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.MapControllers();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Run();
        }
    }
}