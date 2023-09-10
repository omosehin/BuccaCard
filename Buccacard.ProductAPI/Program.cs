using API.Middleware;
using Buccacard.Infrastructure.Utility;
using Buccacard.ProductAPI;
using Buccacard.Services;
using Buccacard.Services.ProductService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICardService, CardService > ();
builder.Services.AddScoped<IResponseService, ResponseService>();

// Adding Authentication
var key = Encoding.UTF8.GetBytes(configuration["JwtOptions:Secret"]);
builder.Services.AddAuthorization();
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
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });
var app = builder.Build();
app.UseRouting(); 
app.UseMiddleware<ProductExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers()); 
app.MapGet("/", () => "Hello World, this is product management API service !");
await RunMigration();
app.Run();

async Task RunMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
      await  _db.Database.MigrateAsync();
    }
}