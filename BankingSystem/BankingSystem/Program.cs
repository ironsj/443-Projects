using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BankingSystem.Data;

using BankingSystem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBankService, BankService>();        // BankService added 3/07/2022  L J Kotman

builder.Services.AddDbContext<BankingSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BankingSystemContext")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    DbInitializer.Initialize(services);
}

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

app.Run();
