using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IReturnNodePointerProject.Controllers;
using IReturnNodePointerProject.Areas;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace IReturnNodePointerProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //enables session state and must be called before add controllerswithViews()

            builder.Services.AddMemoryCache();
            builder.Services.AddSession(options =>
            {
                //options.IdleTimeout = TimeSpan.FromSeconds(60 * 5); //not currently used but the code to change the timeout feature on the session  (default is 20 min)
                options.Cookie.HttpOnly = false; //would allow client side scripts to access cookies, security issue so false 
                options.Cookie.IsEssential = true; //forces them to use cookies or not run the page, is essential for us as user permissions and stuff need to be set with an account 
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

			builder.Services.AddHttpContextAccessor();

			builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineStoreContext")));


            builder.Services.AddRazorPages();

            builder.Services.AddControllers(options =>
            {
                options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //Before useendpoints() which is the mappcontrollerroute 
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseRouting();
            app.UseAuthentication(); 

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "Product",
                pattern: "{controller=Product}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "custom",
                pattern: "{controller}/{action}"/*{activeConf}/div-{activeDiv}*/);
  

            //generic admin account 
            /*
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<>>();
                string email = "admin@admin.com";
                string password = "Password1!";
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new Models.User();
                    user.Email = email;
                    user.UserName = email;

                    await userManager.CreateAsync(user, password);

                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
            */
            app.Run();
        }
    }
}