using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

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
//builder.Services.AddCors(options => {
//    options.AddPolicy("CORSPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true));
//});
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);


builder.Services.AddControllers()

        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });
var app = builder.Build();
//app.UseCors("CORSPolicy");
app.UseRouting(); //Without this line the default "Hello World" page will not be displayed
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers()); //Without this line the default "Hello World" page will not be displayed
app.UseAuthentication();
app.UseOcelot().Wait();
app.MapGet("/", () => "Hello World, this is gateway api !");

app.Run();
