using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SB.Server.Root.Casinos;

namespace SB.Server.WebApp.IntegrationTests;

public class CasinosTests
{
    [Test]
    public async Task GetAllCasinos_EnsureContainsFullList()
    {
        var client = await Config.GetAuthorizedClientWithoutAdminAccessLevel();
        var response = await client.GetAsync( "/api/casinos/" );
        
        Assert.AreEqual( HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse( result );
        
        var casinoList = JsonConvert.DeserializeObject<List<Casino>>(((JArray?) json?["currentCasinos"]).ToString());

        var expectedList = DbSeeding.GetListOfCasinos();

        foreach (var casino in expectedList)
        {
            if (!casinoList.Any(c => c.Name.Equals(casino.Name) && c.CountryCode.Equals(casino.CountryCode)))
            {
                Assert.Fail();
            }
        }
    }
}