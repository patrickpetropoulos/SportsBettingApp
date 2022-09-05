using System.Net;
using Newtonsoft.Json.Linq;

namespace SB.Server.WebApp.IntegrationTests;

public class TokenTests
{
    [Test]
    public async Task GetToken_TestClaimsSucceded()
    {
        var client = Config.GetClient();
        var response = await client.GetAsync( "/test" );
        
        Assert.AreEqual( HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadAsStringAsync();
        var allJson = JObject.Parse( result );

        var json = (JObject?)allJson["value"];
        var user = (JObject?)json?["user"];
        var username = user?["userName"] + "";
        Assert.AreEqual( "poweruser", username );
        
        var roles = (JObject?) json?["roles"];

        var roleNames = (JArray)roles?["result"];

        Assert.AreEqual(1, roleNames.Count);
        Assert.AreEqual("Admin", roleNames[0] + "");

    }
    [Test]
    public async Task GetToken_TestClaimsFailed()
    {
        var client = Config.GetClientWithUnauthorizedUser();
        var response = await client.GetAsync( "/test" );
        
        Assert.AreEqual( HttpStatusCode.Unauthorized, response.StatusCode);
    }
}