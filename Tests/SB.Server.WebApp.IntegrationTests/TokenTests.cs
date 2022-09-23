using System.Net;
using Newtonsoft.Json.Linq;

namespace SB.Server.WebApp.IntegrationTests;

public class TokenTests
{
    [Test]
    public async Task GetToken_TestRoles_Succeeded()
    {
        var client = Config.GetClient();
        var response = await client.GetAsync( "/test" );
        
        Assert.AreEqual( HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse( result );

        var user = (JObject?)json?["user"];
        var username = user?["userName"] + "";
        Assert.AreEqual( "poweruser", username );

        var roles = (JArray?) json?["roles"];
        Assert.AreEqual(1, roles!.Count);
        Assert.AreEqual("Admin", roles[0] + "");
    }
    [Test]
    public async Task GetToken_TestRoles_Failed()
    {
        var client = Config.GetClientWithUnauthorizedUser();
        var response = await client.GetAsync( "/test" );
        
        Assert.AreEqual( HttpStatusCode.Unauthorized, response.StatusCode);
    }
}