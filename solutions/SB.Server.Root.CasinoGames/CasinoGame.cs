using Newtonsoft.Json.Linq;
using SB.Utilities;

namespace SB.Server.Root.CasinoGames;

public class CasinoGame : ICasinoGame
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool HasSubType { get; set; }
    


    public void FromJson( JObject json )
    {
        Id = JSONUtilities.GetGuid( json, "id" );
        Name = JSONUtilities.GetString( json, "name" );
        HasSubType = JSONUtilities.GetBool( json, "hasSubType" );
    }

    public JObject ToJson()
    {
        var json = new JObject();

        JSONUtilities.Set( json, "id", Id );
        JSONUtilities.Set( json, "name", Name );
        JSONUtilities.Set( json, "hasSubType", HasSubType );

        return json;
    }
    
    public bool BasicDataEquals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;
        
        return obj is CasinoGame casinoGame
                   && Name.Equals(casinoGame.Name)
                   && HasSubType.Equals(casinoGame.HasSubType);
    }
}