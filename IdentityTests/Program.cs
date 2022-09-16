using IdentityTests.Extensions;
using IdentityTests.WebApi.Helpers;
using IdentityTests.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecurityTests.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ProductDbContext>(opts =>
{
    opts.UseSqlServer("Data Source=localhost;Initial Catalog = IdentityAppData; Integrated Security = True");
});

builder.Services.AddDbContext<IdentityDbContext>(opts =>
{
opts.UseSqlServer("Data Source=localhost;Initial Catalog = IdentityAppUserData; Integrated Security = True"
, opts => opts.MigrationsAssembly("IdentityTests"));
});
builder.Services.AddDefaultIdentity<IdentityUser>()
.AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.ConfigureApplicationCookie(opts => {
    opts.Events.DisableRedirectionForApiClients();
});

builder.Services.AddAuthentication()
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
    opts.TokenValidationParameters.ValidateAudience = false;
    opts.TokenValidationParameters.ValidateIssuer = false;
    // opts.TokenValidationParameters.IssuerSigningKey = newSymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["BearerTokens:Key"]));
    opts.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mySuperSecretKey"));
});

builder.Services.AddScoped<IUserService, UserService>();

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

app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
