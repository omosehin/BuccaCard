using API.Middleware;
using Buccacard.Repository.DbContext;
using Buccacard.Services.ReportService;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("UserConnectionString"));
});

builder.Services.AddDbContext<ProductDbContext>(options => { 
    options.UseSqlServer(configuration.GetConnectionString("ProductConnectionString"));
    });

builder.Services.AddDbContext<ReportDbContext>(options => { 
    options.UseSqlServer(configuration.GetConnectionString("ReportConnectionString"));
});

builder.Services.AddHangfire(config => config
.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage( configuration.GetConnectionString("ReportConnectionString")));


builder.Services.AddScoped<IReportJob, ReportJob>();

builder.Services.AddHangfireServer();
builder.Services.AddCors(o => o.AddPolicy("CoresPolicy", builder =>
{
    builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });
var app = builder.Build();



app.UseRouting();
app.UseMiddleware<ExceptionMiddleware>();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.MapGet("/", () => "Hello World, this is report management API service !");
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Buccacard Report Dashboard",
});
RecurringJob.AddOrUpdate<IReportJob>(v => v.GetCreatedCard(), Cron.Daily(07, 00), TimeZoneInfo.Utc);

await RunMigration();
app.Run();

async Task RunMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<ReportDbContext>();
        await _db.Database.MigrateAsync();
    }
}