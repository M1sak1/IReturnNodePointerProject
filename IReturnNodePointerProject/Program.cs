using Microsoft.EntityFrameworkCore;

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

builder.Services.AddDbContext<OnlineStoreContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineStoreContext")));

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

app.UseAuthorization();

app.MapControllerRoute(    
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "custom",
    pattern: "{controller}/{action}"/*{activeConf}/div-{activeDiv}*/);

app.Run();
