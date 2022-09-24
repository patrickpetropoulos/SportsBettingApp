using System.Net.Http.Json;
using System.Runtime;
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
  private static IConfiguration? _configuration;
  
  //TODO Move all clients to their own parameters, therefore dont need to get them each time test is call

  // TODO can set up multiple clients with different claims/roles and test all of them
  // Have many "power users" for all testing of all endpoints
  public static HttpClient GetClientWithUnauthorizedUser()
  {
    return _app.CreateClient();
  }

  public static async Task<HttpClient> GetAuthorizedClientWithAdminAccessLevel()
  {
    var client = _app.CreateClient();

    var token = await GetTokenForUser("poweruser");

    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {token}");

    return client;
  }
  public static async Task<HttpClient> GetAuthorizedClientWithoutAdminAccessLevel()
  {
    var client = _app.CreateClient();

    var token = await GetTokenForUser("basicaccessuser");

    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {token}");

    return client;
  }

  [OneTimeSetUp]
  public static void MyOneTime()
  {
    _app = new WebApplicationFactory<Program>()
      .WithWebHostBuilder(builder =>
      {

        //TODO eventually have other server/DB just for testing
        builder.UseEnvironment("Development");

        //var startup = new Startup( _configuration );

        //builder.UseStartup<Startup>();


        // ... Configure test services
      });



    //Move all this out to it's own methods, so can create different users from appSetting based on roles,
    //and make sure they are assigned those roles before running tests


    //put code here to create app
    //delete all users and stuff from database
    //delete all other data from database
    //seed database

    var jsonPath = "appsettings.json";

    //Check if testing file exists
    //Need this check for building and testing in azure devops
    //Since in azure substitution is done, can handle having separate db for testing.
    //For now in code, when running tests locally, do this check
    if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json")))
    {
      Console.WriteLine("#### PATRICK : App settings Development Exists");
      jsonPath = "appsettings.Development.json";
    }

    var configBuilder = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile(jsonPath, true, true)
      .AddEnvironmentVariables();


    _configuration = configBuilder.Build();
  }

  public static async Task<string> GetTokenForUser(string username)
  {
    var user = GetUserRecordFromConfig(username);
    if (user == null)
    {
      throw new NotSupportedException();
    }
    return await GetTokenByCredentials(user.Username, user.Password);
  }
  
  public static UserRecord? GetUserRecordFromConfig(string username)
  {
    var users = _configuration?.GetSection("InitialUsers").Get<UserRecord[]>();

    return users?.FirstOrDefault(c => c.Username.Equals(username));
  }
  
  private static async Task<string> GetTokenByCredentials(string username, string password)
  {
    var client = _app.CreateClient();
    var response = await client.PostAsJsonAsync($"/token", new {Username = username, Password = password});

    var result = await response.Content.ReadAsStringAsync();
    var json = JObject.Parse(result);

    return JSONUtilities.GetString(json, "accessToken");
  }
}