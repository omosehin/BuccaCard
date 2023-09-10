using API.Middleware;
using AspNetCoreRateLimit;
using Buccacard.GateWayAPI;
using Buccacard.Infrastructure.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

var key = Encoding.UTF8.GetBytes(configuration["JwtOptions:Secret"]);

builder.Services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("CanCreateCard", policy => policy.RequireClaim("Role", "Admin"));

});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
   .AddJwtBearer(options =>
   {       
       options.TokenValidationParameters = new TokenValidationParameters()
       {
           ValidateIssuer = false,
           ValidateAudience = false,
           IssuerSigningKey = new SymmetricSecurityKey(key),
           ValidateIssuerSigningKey = false,
           RequireExpirationTime = true,
           ValidateLifetime = true,
           ClockSkew = TimeSpan.Zero
       };
   });
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy", builder => builder.AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed((hosts) => true));
});

builder.Services.AddTransient<IResponseService, ResponseService>();
builder.Services.AddTransient<IBaseHttpClient, BaseHttpClient>();
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddInMemoryRateLimiting();

var app = builder.Build();

app.UseCors("CORSPolicy");
// app.UsePathBase(new PathString("/gateway"));
app.UseRouting(); //Without this line the default "Hello World" page will not be displayed
app.UseIpRateLimiting();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers()); //Without this line the default "Hello World" page will not be displayed
app.MapGet("/", () => "Hello World, this is gateway api !");

app.Run();

