using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SB.Server.App.Common;
using SB.Server.Root.CasinoGames;
using System.Net;

namespace SB.Server.WebApp.IntegrationTests;

public class CasinoGamesTests
{
  public static async Task<List<CasinoGame>> GetAllCasinoGames( HttpClient client )
  {
    var response = await client.GetAsync( "/api/casinogames/" );

    Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );

    var result = await response.Content.ReadAsStringAsync();
    var json = JObject.Parse( result );

    return JsonConvert.DeserializeObject<List<CasinoGame>>( ( (JArray?)json?["casinogames"] ).ToString() );
  }

  [Test]
  public async Task GetAllCasinos_EnsureContainsFullSeedDataList()
  {
    var client = await Config.GetAuthorizedClientWithoutAdminAccessLevel();

    var casinoGames = await GetAllCasinoGames( client );

    var expectedList = new DataSeeding().GetSeedCasinoGames();

    //If any of the known seed data casinos do not appear in list, Fail
    foreach( var casinoGame in expectedList )
    {
      if( !casinoGames.Any( c => c.BasicDataEquals( casinoGame ) ) )
      {
        Assert.Fail();
      }
    }
  }
}