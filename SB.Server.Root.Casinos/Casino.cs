using Newtonsoft.Json.Linq;
using SB.Utilities;

namespace SB.Server.Root.Casinos;

public class Casino : ICasino
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string CountryCode { get; set; }
    


    public void FromJson( JObject json )
    {
        Id = JSONUtilities.GetGuid( json, "id" );
        Name = JSONUtilities.GetString( json, "name" );
        CountryCode = JSONUtilities.GetString( json, "countryCode" );
    }

    public JObject ToJson()
    {
        var json = new JObject();

        JSONUtilities.Set( json, "id", Id );
        JSONUtilities.Set( json, "name", Name );
        JSONUtilities.Set( json, "countryCode", CountryCode );

        return json;
    }
    
    public bool BasicDataEquals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;
        else
            return obj is Casino casino
                   && Name.Equals(casino.Name)
                   && CountryCode.Equals(casino.CountryCode);
    }
}