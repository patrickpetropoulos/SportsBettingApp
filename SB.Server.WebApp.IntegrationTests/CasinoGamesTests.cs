using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SB.Server.App.Common;
using SB.Server.Root.CasinoGames;
using SB.Utilities;
using System.Net;

namespace SB.Server.WebApp.IntegrationTests;

public class CasinoGamesTests
{
  public string Endpoint = "";
  public void SetEndpoint( string version )
  {
    Endpoint = $"/api/{version}/casinogames/";
  }

  public async Task<List<CasinoGame>> GetAllCasinoGames( HttpClient client )
  {
    var response = await client.GetAsync( Endpoint );

    Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );

    var json = JsonHelper.GetObjectFromValueTag( await response.Content.ReadAsStringAsync() );

    return JsonConvert.DeserializeObject<List<CasinoGame>>( ( (JArray?)json?["casinogames"] ).ToString() );
  }

  [TestCase( "v1" )]
  public async Task GetAllCasinos_EnsureContainsFullSeedDataList( string version )
  {
    SetEndpoint( version );

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