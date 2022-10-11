using Newtonsoft.Json.Linq;
using SB.Utilities;

namespace TheOddsApi.Model
{
    public class Game
    {
        public string Id { get; set; }
        public string SportKey { get; set; }
        public string SportTitle { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public List<Odds> Odds { get; set; } = new List<Odds>();

        public void FromJson( JObject json )
        {
            Id = JSONUtilities.GetString( json, "id" );
            SportKey = JSONUtilities.GetString( json, "sport_key" );
            SportTitle = JSONUtilities.GetString( json, "sport_title" );
            StartTime = JSONUtilities.GetDateTimeOffset( json, "commence_time" );
            HomeTeam = JSONUtilities.GetString( json, "home_team" );
            AwayTeam = JSONUtilities.GetString( json, "away_team" );

            var bookMakers = (JArray)json["bookmakers"];
            foreach( JObject bookMaker in bookMakers )
            {
                var key = JSONUtilities.GetString( bookMaker, "key" );
                var title = JSONUtilities.GetString( bookMaker, "title" );
                var lastUpdate = JSONUtilities.GetDateTimeOffset( bookMaker, "last_update" );

                var markets = (JArray)bookMaker["markets"];
                foreach( JObject market in markets )
                {
                    var marketName = JSONUtilities.GetString( market, "key" );
                    var outcomes = (JArray)market["outcomes"];
                    foreach( JObject outcome in outcomes )
                    {
                        var winner = JSONUtilities.GetString( outcome, "name" );
                        var price = JSONUtilities.GetFloat( outcome, "price" );
                        var point = JSONUtilities.GetFloat( outcome, "point" );

                        var odds = new Odds()
                        {
                            Key = key,
                            Title = title,
                            LastUpdate = lastUpdate,
                            Market = marketName,
                            Winner = winner,
                            Price = price,
                            Point = point
                        };
                        Odds.Add( odds );
                    }
                }
            }
        }
    }

    public class Odds
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public DateTimeOffset LastUpdate { get; set; }
        public string Market { get; set; }
        public string Winner { get; set; }
        public float Price { get; set; }

        public float Point { get; set; }



    }
}