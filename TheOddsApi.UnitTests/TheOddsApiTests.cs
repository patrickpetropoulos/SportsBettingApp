using Newtonsoft.Json.Linq;
using System.Reflection;
using TheOddsApi.Parser;

namespace TheOddsApi.UnitTests;

public class TheOddsApiTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [TestCase( "TheOddsApi.UnitTests.TestData.MLB.oddsapi-mlb-sport888-decimal.json", 12, 1, 3 )]
    [Parallelizable( ParallelScope.All )]
    public void ValidateParser_ParsesOddsAPIFileCorrectly( string path, int numGames, int numBookMakers, int numMarkets )
    {
        var assembly = Assembly.GetExecutingAssembly();
        using( var stream = assembly.GetManifestResourceStream( path ) )
        using( var reader = new StreamReader( stream ) )
        {
            string text = reader.ReadToEnd();

            var games = JArray.Parse( text );
            Assert.That( games, Has.Count.EqualTo( numGames ) );

            foreach( var game in games )
            {
                Assert.That( game["sport_title"] + "", Is.EqualTo( "MLB" ) );

                var bookmakers = (JArray)game["bookmakers"];
                Assert.That( bookmakers, Has.Count.EqualTo( numBookMakers ) );
                foreach( var bookMaker in bookmakers )
                {
                    var markets = (JArray)bookMaker["markets"];
                    Assert.That( markets, Has.Count.EqualTo( numMarkets ) );
                }
            }
        }
    }

    [Test]
    [TestCase( "TheOddsApi.UnitTests.TestData.MLB.oddsapi-mlb-sport888-decimal.json", 12, 6 )]
    [Parallelizable( ParallelScope.All )]
    public void ValidateParser_ParsesOddsAPIFileFully( string path, int numGames, int oddsPerGame )
    {
        var assembly = Assembly.GetExecutingAssembly();
        using( var stream = assembly.GetManifestResourceStream( path ) )
        using( var reader = new StreamReader( stream ) )
        {
            string text = reader.ReadToEnd();
            var games = OddsParser.ParseGame( text );

            Assert.That( games, Has.Count.EqualTo( numGames ) );

            foreach( var game in games )
            {
                Assert.That( game.Odds, Has.Count.EqualTo( oddsPerGame ) );
            }
        }
    }
}
