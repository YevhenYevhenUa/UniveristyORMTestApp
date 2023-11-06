using Task9University.Middleware;
using Task9.University.Infrastructure.Services;
using Task9.University.Infrastructure.Data;
using Task9University;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => 
{ 
    options.SignIn.RequireConfirmedAccount = true; 
})
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();



// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<OperationCancelledExceptionFilter>();
});
builder.Services.Configure<IdentityOptions>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = true;
});

builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
{
    options.LoginPath = "/Account/Login";
});

builder.Services.GetServiceDependencies();
builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.GetRepositoryDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Seed();
await app.SeedUsersAndRoles();

app.Run();
