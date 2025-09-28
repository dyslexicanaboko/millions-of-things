using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace MillionsOfThings.WebApi;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    // Add services to the container.
    ContainerConfig.Configure(builder.Host);

    /* JWT Authentication should go here
     * 
     * NuGet: Microsoft.AspNetCore.Authentication.JwtBearer
     * Add using clauses:
     *  using Microsoft.AspNetCore.Authentication.JwtBearer;
     *  using Microsoft.IdentityModel.Tokens;
     *  using System.Text;
     */
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
      };
    });

    //https://stackoverflow.com/questions/70554844/asp-net-core-6-web-api-making-fields-required
    //The controller was making non-nullable properties required without my permission
    builder.Services
      .AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
      .AddNewtonsoftJson(
        options =>
        {
          options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
          options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
          options.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
        });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.Logger.LogInformation("Millions of things API is running");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();

      SetLooseCorsPolicyForDevelopmentPurposesOnly(app);
    }

    //This is specifically so that Dapper can deal with Postgres naming conventions
    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

    //When working with Next.js you may have to disable https to do local development
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.UseMiddleware<ResponseMiddleware>(app.Logger);

    app.Run();
  }

  private static void SetLooseCorsPolicyForDevelopmentPurposesOnly(IApplicationBuilder app)
  {
    app.UseCors(
      policy =>
      {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
      });
  }
}
