using AspNetCoreRateLimit;
using Buccacard.Domain.UserManagement;
using Buccacard.Infrastructure.DTO.User;
using Buccacard.Infrastructure.Utility;
using Buccacard.Repository;
using Buccacard.Repository.DbContext;
using Buccacard.Services.UserManagementService;
using Buccacard.UserManagementAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<UserDbContext>(options => 
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentity<AppUser, IdentityRole>(opts=>opts.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();

// Adding Authentication
var key = Encoding.UTF8.GetBytes(configuration["JwtOptions:Secret"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
   .AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidAudience = configuration["JwtOptions:ValidAudience"],
        ValidIssuer = configuration["JwtOptions:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});



// builder.Services.AddTransient<RoleManager<IdentityRole>, RoleManager<IdentityRole>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IBaseHttpClient, BaseHttpClient>();
builder.Services.AddScoped<IResponseService, ResponseService>();

builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User",
        Version = "v1",
        Description = "Your API Description",
        Contact = new OpenApiContact
        {
            Name = "User Management Service",
            Email = "secureitltd",
        },
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swagger, httpReq) =>
        {
            swagger.Servers = new List<OpenApiServer> {
                        new OpenApiServer
                        {
                            Url = httpReq.Host.Value.Contains("localhost") ? $"http://{httpReq.Host.Value}"
                                : $"https://{httpReq.Host.Value}"
                        }
                    };
        });
    });

    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger";
        c.DefaultModelExpandDepth(-1);
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auser Management API");
    });
}

app.UseRouting(); //Without this line the default "Hello World" page will not be displayed
app.UseMiddleware<UserAPICustomExceptionMiddleware>();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers()); //Without this line the default "Hello World" page will not be displayed
app.UseAuthentication();
await RunMigration();
app.MapGet("/", () => "Hello World, this is user management service !");
app.Run();

async Task RunMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        _db.Database.Migrate(); 
        await Seed.SeedData(_db, userManager,roleManager);
    }
}