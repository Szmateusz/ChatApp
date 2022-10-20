using Blog;
using Blog.Hubs;
using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DBcontext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
    
});





builder.Services.AddSignalR();

builder.Services.AddIdentity<UserModel, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 2;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;



}).AddEntityFrameworkStores<DBcontext>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Account/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/messageHub");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");
// pattern: "{controller=Blog}/{action=Index}/{id?}");

app.Run();
