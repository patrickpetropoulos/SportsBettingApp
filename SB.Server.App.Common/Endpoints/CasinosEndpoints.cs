using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SB.Server.Common.Managers;
using SB.Server.Root.Casinos;
using static SB.Server.App.Common.AuthorizationConstants;

namespace SB.Server.App.Common.Endpoints;

public static class CasinosEndpoints
{
  public static WebApplication MapCasinosEndpoints( this WebApplication app )
  {
    //TODO how to manage swagger to have them separated out better

    return app.MapGetAllCasinos()
        .MapInsertCasino()
        .MapUpdateCasino()
        .MapDeleteCasino();
  }

  private static WebApplication MapGetAllCasinos( this WebApplication app )
  {
    //Better path??
    app.MapGet( "/api/casinos/",
        async () =>
        {
          var casinoManager = ServerSystem.Instance?.Get<ICasinoManager>( ManagerNames.CasinoManager );
          if( casinoManager == null )
            return Results.BadRequest();
          var casinos = await casinoManager.GetAllCasinos();

          return Results.Ok( new { casinos } );
        } );
    return app;
  }

  private static WebApplication MapInsertCasino( this WebApplication app )
  {
    app.MapPost( "/api/casinos/",
        async ( [FromBody] Casino casino ) =>
        {
          var casinoManager = ServerSystem.Instance?.Get<ICasinoManager>( ManagerNames.CasinoManager );
          if( casinoManager == null )
            return Results.BadRequest();
          await casinoManager.UpsertCasino( casino );

          return Results.Ok( new { casino } );
        } )
    .RequireAuthorization( Claim_Policy_IsAdmin );
    return app;
  }

  private static WebApplication MapUpdateCasino( this WebApplication app )
  {
    app.MapPut( "/api/casinos/{id}",
            async ( Guid id,
                [FromBody] Casino casino ) =>
            {
              if( casino.Id != id )
              {
                //TODO put these errors somewhere and ensure we get them
                return Results.BadRequest( "Id in url doesn't match Id in object" );
              }

              var casinoManager = ServerSystem.Instance?.Get<ICasinoManager>( ManagerNames.CasinoManager );
              if( casinoManager == null )
                return Results.BadRequest();

              await casinoManager.UpsertCasino( casino );

              return Results.Ok( new { casino } );
            } )
        .RequireAuthorization( Claim_Policy_IsAdmin );
    return app;
  }

  private static WebApplication MapDeleteCasino( this WebApplication app )
  {
    //Better path??
    app.MapDelete( "/api/casinos/{id}",
            async ( Guid id ) =>
            {
              var casinoManager = ServerSystem.Instance?.Get<ICasinoManager>( ManagerNames.CasinoManager );
              if( casinoManager == null )
                return Results.BadRequest();
              var successful = await casinoManager.DeleteCasino( id );

              return successful ?
                      Results.Ok( "Casino with id " + id + "deleted" ) :
                      Results.BadRequest( "Something went wrong" );
            } )
        .RequireAuthorization( Claim_Policy_IsAdmin );
    return app;
  }
}