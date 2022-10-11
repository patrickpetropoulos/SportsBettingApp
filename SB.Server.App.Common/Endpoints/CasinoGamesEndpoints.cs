using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SB.Server.Common.Managers;
using SB.Server.Root.CasinoGames;

namespace SB.Server.App.Common.Endpoints;

public static class CasinoGamesEndpoints
{
  public static WebApplication MapCasinoGamesEndpoints( this WebApplication app )
  {
    return app.MapGetAllCasinoGames();
  }

  private static WebApplication MapGetAllCasinoGames( this WebApplication app )
  {
    app.MapGet( "/api/casinogames/",
        async () =>
        {
          var casinoGameManager = ServerSystem.Instance?.Get<ICasinoGameManager>( ManagerNames.CasinoGameManager );
          if( casinoGameManager == null )
            return Results.BadRequest();
          var casinoGames = await casinoGameManager.GetAllCasinoGames();

          return Results.Ok( new { casinogames = casinoGames } );
        } );
    return app;
  }
}