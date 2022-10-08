using Newtonsoft.Json.Linq;

namespace SB.Server.Root.Base;

public interface IEntity
{
    void FromJson(JObject json);
    JObject ToJson();
    bool BasicDataEquals(object? obj);
}