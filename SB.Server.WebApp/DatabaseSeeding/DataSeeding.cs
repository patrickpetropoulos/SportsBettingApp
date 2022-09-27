using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SB.Server.Common.Managers;
using SB.Server.Root.Casinos;

namespace SB.Server.WebApp;

public class DataSeeding
{
    private JObject _seedData;

    public DataSeeding()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream( "SB.Server.WebApp.DatabaseSeeding.SeedData.json" );
        if (stream == null) return;
        using var reader = new StreamReader(stream);
        var text = reader.ReadToEnd();
        _seedData =  JObject.Parse(text);
    }

    public async Task SeedDatabase()
    {
        await SeedCasinos();
    }
    public List<Casino> GetSeedCasinos()
    {
        var casinos = new List<Casino>();
        try
        {
            casinos = JsonConvert.DeserializeObject<List<Casino>>(((JArray?) _seedData?["casinos"]).ToString());
        }
        catch (Exception ex)
        {
            throw new Exception();
        }
        return casinos;
    }
    
    private async Task SeedCasinos()
    {
        var casinoManager = ServerSystem.Instance?.Get<ICasinoManager>( ManagerNames.CasinoManager);
        if (casinoManager == null)
            return;
        var currentCasinos = await casinoManager.GetAllCasinos();
        //TODO dont do this way, check if each one in data exists and fix it
        if( !currentCasinos.Any() )
        {
            foreach( var casino in GetSeedCasinos())
            {
                await casinoManager.UpsertCasino( casino );
            }
        }
    }
}