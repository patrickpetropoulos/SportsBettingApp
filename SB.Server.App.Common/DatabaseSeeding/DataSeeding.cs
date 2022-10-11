using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SB.Server.Common.Managers;
using SB.Server.Root.CasinoGames;
using SB.Server.Root.Casinos;
using System.Reflection;

namespace SB.Server.App.Common;

public class DataSeeding
{
  private JObject _seedData;

  public DataSeeding()
  {
    var assembly = Assembly.GetExecutingAssembly();
    using var stream = assembly.GetManifestResourceStream( "SB.Server.App.Common.DatabaseSeeding.SeedData.json" );
    if( stream == null ) return;
    using var reader = new StreamReader( stream );
    var text = reader.ReadToEnd();
    _seedData = JObject.Parse( text );
  }

  public async Task SeedDatabase()
  {
    await SeedCasinos();
    await SeedCasinoGames();
  }
  public List<Casino> GetSeedCasinos()
  {
    var casinos = new List<Casino>();
    try
    {
      casinos = JsonConvert.DeserializeObject<List<Casino>>( ( (JArray?)_seedData?["casinos"] ).ToString() );
    }
    catch( Exception ex )
    {
      throw new Exception();
    }
    return casinos;
  }

  private async Task SeedCasinos()
  {
    var casinoManager = ServerSystem.Instance?.Get<ICasinoManager>( ManagerNames.CasinoManager );
    if( casinoManager == null )
      return;
    var currentCasinos = await casinoManager.GetAllCasinos();
    //TODO dont do this way, check if each one in data exists and fix it
    if( !currentCasinos.Any() )
    {
      foreach( var casino in GetSeedCasinos() )
      {
        await casinoManager.UpsertCasino( casino );
      }
    }
  }

  public List<CasinoGame> GetSeedCasinoGames()
  {
    var casinoGames = new List<CasinoGame>();
    try
    {
      casinoGames = JsonConvert.DeserializeObject<List<CasinoGame>>( ( (JArray?)_seedData?["casinoGames"] ).ToString() );
    }
    catch( Exception ex )
    {
      throw new Exception();
    }
    return casinoGames;
  }

  private async Task SeedCasinoGames()
  {
    var casinoGameManager = ServerSystem.Instance?.Get<ICasinoGameManager>( ManagerNames.CasinoGameManager );
    if( casinoGameManager == null )
      return;
    var currentCasinoGames = await casinoGameManager.GetAllCasinoGames();
    //TODO dont do this way, check if each one in data exists and fix it
    if( !currentCasinoGames.Any() )
    {
      foreach( var casinoGame in GetSeedCasinoGames() )
      {
        await casinoGameManager.UpsertCasinoGame( casinoGame );
      }
    }
  }
}