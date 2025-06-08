using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PCStore.Data;
using PCStore.HealthChecks;
using PCStore.Models;
using PCStore.Repositories;
using PCStore.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<CPUValidator>(); // Add FluentValidation

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("Database");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Add Swagger service
builder.Services.AddHttpClient<ExternalProductService>();

builder.Services.AddScoped<ICPURepository, CPURepository>();
builder.Services.AddScoped<IGPURepository, GPURepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBusinessService, BusinessService>();

builder.Services.AddDbContext<PcShopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs\\log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Enable Swagger UI in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PC Store API");
        c.RoutePrefix = string.Empty; // Swagger at root
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapHealthChecks("/healthz");
app.MapControllers();
app.Run();

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
