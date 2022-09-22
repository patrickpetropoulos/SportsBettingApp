using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SB.Utilities;

namespace SB.Server.WebApp.IntegrationTests;

[SetUpFixture]
public static class Config
{
  private static WebApplicationFactory<Program> _app = null!;
  private static string _token = null!;

  // TODO can set up multiple clients with different claims/roles and test all of them
  // Have many "power users" for all testing of all endpoints
  public static HttpClient GetClientWithUnauthorizedUser()
  {
    return _app.CreateClient();
  }
  public static HttpClient GetClient()
  {
    var client = _app.CreateClient();
    client.DefaultRequestHeaders.TryAddWithoutValidation( "Authorization", $"Bearer {_token}" );

    return client;
  }

  public static HttpClient GetClient( string token )
  {
    var client = _app.CreateClient();
    client.DefaultRequestHeaders.TryAddWithoutValidation( "Authorization", $"Bearer {token}" );

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

    var jsonPath = "appsettings.json";

    //Check if testing file exists
    //Need this check for building and testing in azure devops
    //Since in azure substitution is done, can handle having separate db for testing.
    //For now in code, when running tests locally, do this check
    if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Testing.json")))
    {
      Console.WriteLine("#### PATRICK : App settings Testing Exists");
      jsonPath = "appsettings.Testing.json";
    }
    
    var configBuilder = new ConfigurationBuilder()
      .SetBasePath( Directory.GetCurrentDirectory() )
      .AddJsonFile( jsonPath, true, true )
      .AddEnvironmentVariables();

    
    var configuration = configBuilder.Build();

    var username = configuration["PowerUser:Username"];
    var password = configuration["PowerUser:Password"];

    var client = _app.CreateClient();
    var response = await client.GetAsync( $"/token?username={username}&password={password}" );

    var result = await response.Content.ReadAsStringAsync();
    var json = JObject.Parse( result );

    
    
    _token = JSONUtilities.GetString( json, "accessToken" );
  }
}
