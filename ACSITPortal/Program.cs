using ACSITPortal.Data;
using ACSITPortal.Helpers;
using ACSITPortal.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ACSITPortalContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ACSITPortal"));
}, ServiceLifetime.Scoped);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<MailerService>();
builder.Services.AddScoped<SessionManager>();

builder.Services.AddMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
