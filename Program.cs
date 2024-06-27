using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RJTECH_Authentication_.Data;
using RJTECH_Authentication_.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<MyUsers>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    //options.Cookie.MaxAge = TimeSpan.FromDays(7);
    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.AddAuthorization(options => {

    
    options.AddPolicy("adminonly", policy => policy.RequireClaim(ClaimTypes.Email, "administrator@gmail.com"));

   // options.AddPolicy("BusinessHoursOnly", policy =>
   //  policy.RequireAssertion(context => DateTime.Now.Hour >= 9
   //                                     && DateTime.Now.Hour < 11
   //      ));
   // options.AddPolicy("DepartmentsOnly", policy =>
   // policy.RequireClaim("department", "SE"));

   // options.AddPolicy("AsiaOnly", policy =>
   //policy.RequireClaim(ClaimTypes.Country, ));

});
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
