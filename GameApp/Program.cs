using GameApp.Models.Data;
using GameApp.Services.Interface;
using GameApp.Services.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var connectionStr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<GameAppDbContext>(
    Options => Options.UseSqlServer(connectionStr));


builder.Services.AddScoped<IGameConfigurationRepository, GameConfigurationRepository>();
builder.Services.AddScoped<IGameRegistrationRepository, GameRegistrationRepository>();


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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Add this route for the GamerRegistration controller
    endpoints.MapControllerRoute(
        name: "gamerRegistration",
        pattern: "{controller=GamerRegistration}/{action=GamerIndex}/{id?}");
});


app.Run();
