using Buccacard.Services.NotificationService;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("ApiSettings:MailSetting"));

app.Run();
