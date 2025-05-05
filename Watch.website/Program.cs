using Microsoft.EntityFrameworkCore;
using Watch.website.DAL;

namespace Watch.website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            }
           );

            var app = builder.Build();

            app.UseStaticFiles();

            app.MapControllerRoute(
                name: "Admin",
                pattern: "{area=exists}/{controller=Home}/{action=Index}/{id?}");


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
