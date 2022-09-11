using Newtonsoft.Json.Linq;
using TheOddsApi.Model;

namespace TheOddsApi.Parser
{
  public class OddsParser
  {
    public static List<Game> ParseGame( string text )
    {
      var gamesToReturn = new List<Game>();

      var games = JArray.Parse( text );

      foreach( JObject game in games )
      {
        var gameObj = new Game();
        gameObj.FromJson( game );
        gamesToReturn.Add( gameObj );
      }
      return gamesToReturn;
    }
  }
}