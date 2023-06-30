using AutoMapper;
using Gruppo2.WebApp;
using Gruppo2.WebApp.Models.Profiles;
using Microsoft.EntityFrameworkCore;
using Coravel;
using Gruppo2.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";




//database per utenti
builder.Services.AddDbContext<WebAppContex>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("db"));
    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();
});



//database per azienda
builder.Services.AddDbContext<DBAdminContext>(optionsAdmin =>
{
    optionsAdmin.UseSqlServer(builder.Configuration.GetConnectionString("dbAdmin"));
    optionsAdmin.EnableSensitiveDataLogging();
    optionsAdmin.EnableDetailedErrors();
});


//database per azienda
builder.Services.AddScoped<DBAdminContext, DBAdminContext>();

//database per utenti
builder.Services.AddScoped<WebAppContex, WebAppContex>();



builder.Services.AddSingleton<InfluxDBService>();
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7042",
                                              "http://localhost:5151",
                                              "http://localhost:44490").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
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