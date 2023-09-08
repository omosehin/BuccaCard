using Buccacard.Services.NotificationService;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });
var app = builder.Build();
app.UseRouting(); //Without this line the default "Hello World" page will not be displayed
app.UseEndpoints(endpoints => endpoints.MapControllers()); //Without this line the default "Hello World" page will not be displayed

app.MapGet("/", () => "Hello World, this is notification service!");

app.Run();