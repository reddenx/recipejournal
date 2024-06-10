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
    public class SiteConfig : IDbConfig, IAuthenticationConfiguration
    {
        private readonly IConfiguration _config;

        public SiteConfig(IConfiguration config)
        {
            _config = config;
        }

        public string ConnectionString => _config.GetConnectionString("recipe");

        public string Domain => _config.GetSection("authentication")["domain"];
        public string AuthBaseUrl => _config.GetSection("authentication")["baseUrl"];
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
#if DEBUG
            builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
#endif

            var loggingConfig = builder.Configuration.GetSection("SmtLoggingConfiguration");

            // Add services to the container.
            builder.Services.AddSingleton<IDbConfig, SiteConfig>();
            builder.Services.AddHttpClient();
            builder.Services.AddSmtLogging(c =>
            {
                c.CurrentLogLevel = Microsoft.Extensions.Logging.LogLevel.Error;
                c.Port = int.Parse(loggingConfig["port"]);
                c.Host = loggingConfig["host"];
                c.Secret = loggingConfig["secret"];
            });

            builder.Services.AddSingleton<IAuthenticationConfiguration, SiteConfig>();
            
#if DEBUG
            builder.Services.AddSingleton<IShoppingRepository, MockShoppingRepository>();
            builder.Services.AddSingleton<IRecipeRepository, MockRecipeRepository>();
            builder.Services.AddSingleton<IUserRepository, MockUserRepository>();
            builder.Services.AddSingleton<IJournalRepository, MockJournalRepository>();
            builder.Services.AddSingleton<IAuthenticationUtility, MockAuthenticationUtility>();
#else
            builder.Services.AddSingleton<IShoppingRepository, ShoppingRepository>();
            builder.Services.AddSingleton<IRecipeRepository, RecipeRepository>();
            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.Services.AddSingleton<IJournalRepository, JournalRepository>();
            builder.Services.AddSingleton<IAuthenticationUtility, AuthenticationUtility>();
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

            app.UseSmtLoggingEndpoints();
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