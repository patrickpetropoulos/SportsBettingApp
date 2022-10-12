using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using SB.Server.App.React.Startup;
using SB.Server.ReactApp;
using SB.Server.ReactApp.Controllers;
using System.IdentityModel.Tokens.Jwt;

public class Program
{
  private static void Main( string[] args )
  {
    var builder = WebApplication.CreateBuilder( args );

    //Need to set this up if dont want all schemas text crap in JWT claims
    //https://github.com/dotnet/aspnetcore/issues/4660
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

    builder.Services.AddApiVersioning( o =>
    {
      o.AssumeDefaultVersionWhenUnspecified = true;
      o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion( 1, 0 );
      o.ReportApiVersions = true;
      o.ApiVersionReader = ApiVersionReader.Combine(
          new QueryStringApiVersionReader( "api-version" ),
          new HeaderApiVersionReader( "X-Version" ),
          new MediaTypeApiVersionReader( "ver" ) );

    } );

    // Add services to the container.
    builder.Services.AddControllers();
   
    builder.Services.RegisterAllServices( builder.Configuration, typeof( Program ).Assembly.FullName );

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if( !app.Environment.IsDevelopment() )
    {
      // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
      app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();

    //Need to setup react proxy to allow paths
    //https://stackoverflow.com/a/72415603

    app.MapControllers();

    ConfigureApplication.SetupApplication(app);
    ConfigureApplication.SeedApplication( app );

    app.MapGet( "/api/testcasinos/",
            async () =>
            {
              return Enumerable.Range( 1, 100 ).Select( index => new WeatherForecast
              {
                Date = DateTime.Now.AddDays( index ),
                TemperatureC = Random.Shared.Next( -20, 55 ),
                Summary = WeatherForecastController.Summaries[Random.Shared.Next( WeatherForecastController.Summaries.Length )]
              } )
              .ToArray();
            } )
      .AllowAnonymous();



    app.MapFallbackToFile( "index.html" ); ;

    app.Run();
  }
}