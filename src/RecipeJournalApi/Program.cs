using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecipeJournalApi.Controllers;
using RecipeJournalApi.Infrastructure;
using System;
using System.Threading.Tasks;

namespace RecipeJournalApi
{
    public class SiteConfig : IDbConfig
    {
        private readonly IConfiguration _config;

        public SiteConfig(IConfiguration config)
        {
            _config = config;
        }

        public string ConnectionString => _config.GetConnectionString("recipe");
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Add services to the container.
            builder.Services.AddSingleton<IDbConfig, SiteConfig>();
#if DEBUG
            builder.Services.AddSingleton<IRecipeRepository, MockRecipeRepository>();
            builder.Services.AddSingleton<IUserRepository, MockUserRepository>();
            builder.Services.AddSingleton<IShoppingRepository, MockShoppingRepository>();
            builder.Services.AddSingleton<IJournalRepository, MockJournalRepository>();
#else
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
            app.UseRouting();
            app.MapControllers();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.Run();
        }
    }
}