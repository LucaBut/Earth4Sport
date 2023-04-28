using AutoMapper;
using Gruppo2.WebApp;
using Gruppo2.WebApp.Models.Profiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";





builder.Services.AddDbContext<WebAppContex>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("db"));
    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();
});


builder.Services.AddScoped<WebAppContex, WebAppContex>();
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7042",
                                              "http://localhost:5151",
                                              "http://localhost:44490").AllowAnyOrigin().AllowAnyMethod();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
