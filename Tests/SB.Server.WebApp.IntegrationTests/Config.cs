using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Respawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SB.Server.WebApp.IntegrationTests;

[SetUpFixture]
public static class Config
{
  public static WebApplicationFactory<Program>? _app = null;
  private static string _token;

  public static HttpClient? GetClientWithUnauthorizedUser()
  {
    return _app?.CreateClient();
  }
  public static HttpClient? GetClient()
  {
    var client = _app?.CreateClient();
    client?.DefaultRequestHeaders.TryAddWithoutValidation( "Authorization", $"Bearer {_token}" );

    return client;
  }

  public static HttpClient? GetClient( string token )
  {
    var client = _app?.CreateClient();
    client?.DefaultRequestHeaders.TryAddWithoutValidation( "Authorization", $"Bearer {token}" );

    return client;
  }

  [OneTimeSetUp]
  public static async Task MyOneTime()
  {
    _app = new WebApplicationFactory<Program>()
      .WithWebHostBuilder( builder =>
      {

        builder.UseEnvironment( "Testing" );

        //var startup = new Startup( _configuration );

        //builder.UseStartup<Startup>();


        // ... Configure test services
      } );

    //put code here to create app
    //delete all users and stuff from database
    //delete all other data from database
    //seed database
    var test = Directory.GetCurrentDirectory();
    var configBuilder = new ConfigurationBuilder()
      .SetBasePath( Directory.GetCurrentDirectory() )
      .AddJsonFile( "appsettings.Testing.json", true, true )
      .AddEnvironmentVariables();

    var configuration = configBuilder.Build();

    var username = configuration["PowerUser:Username"];
    var password = configuration["PowerUser:Password"];

    var client = _app?.CreateClient();
    var response = await client.GetAsync( $"/token?username={username}&password={password}" );

    var result = await response.Content.ReadAsStringAsync();
    var json = JObject.Parse( result );

    //TODO Change to JsonUtilitiesClass
    _token = GetString( json, "accessToken" );
  }
  public static String GetString( JObject obj, String name, String defaultValue = "" )
  {
    var v = (JValue)obj[name];

    if( v == null )
    {
      return defaultValue;
    }

    var str = ( v ).Value;

    if( str == null )
    {
      return defaultValue;
    }

    return str.ToString();
  }

}
