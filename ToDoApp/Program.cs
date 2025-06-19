using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ToDoApp.DAL;
using ToDoApp.Data;
using ToDoApp.DataContext;
using ToDoApp.Mapper;
using ToDoApp.Services;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, false)
                //.AddJsonFile($"appsettings.{envLower}.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddSingleton<IConfiguration>(config);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            // DAL
            builder.Services.AddDbContextFactory<ToDoListContext>(opt =>
                opt.UseSqlServer("Name=ConnectionStrings:ToDoAppConn"));

            builder.Services.AddTransient<ToDoListContext>();
            builder.Services.AddSingleton<IToDoRepo, ToDoRepo>();

            // Services
            builder.Services.AddSingleton<IToDoService, ToDoService>();
            builder.Services.AddSingleton<WeatherForecastService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }


            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}
