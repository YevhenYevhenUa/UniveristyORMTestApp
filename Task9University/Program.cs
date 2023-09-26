using Task9University.Middleware;
using Task9.University.Infrastructure.Services;
using Task9.University.Infrastructure.Data;
using Task9University;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<OperationCancelledExceptionFilter>();
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Seed();


app.Run();
