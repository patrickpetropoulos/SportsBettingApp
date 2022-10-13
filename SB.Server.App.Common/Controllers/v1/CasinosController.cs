using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SB.Server.Common.Managers;
using SB.Server.Root.Casinos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SB.Server.App.Common.Controllers.v1
{
  [Route( "api/v{verion:apiVersion}/[controller]" )]
  [ApiController]
  [ApiVersion("1.0")]
  public class CasinosController : ControllerBase
  {
    [HttpGet]
    [AllowAnonymous]
    public async Task<IResult> Get()
    {
      var casinoManager = ServerSystem.Instance?.Get<ICasinoManager>( ManagerNames.CasinoManager );
      if( casinoManager == null )
        return Results.BadRequest();
      var casinos = await casinoManager.GetAllCasinos();

      return Results.Ok( new { casinos } );
    }

    [HttpPost]
    [Authorize( Policy = AuthorizationConstants.Claim_Policy_IsAdmin )]
    public async Task<IResult> Post([FromBody] Casino casino )
    {
      var casinoManager = ServerSystem.Instance?.Get<ICasinoManager>( ManagerNames.CasinoManager );
      if( casinoManager == null )
        return Results.BadRequest();
      await casinoManager.UpsertCasino( casino );

      return Results.Ok( new { casino } );
    }

    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationConstants.Claim_Policy_IsAdmin)]
    //TODO for put, do we need to pass in ID in url?  why are we doing that??  shouldn't we just check on body
    public async Task<IResult> Put( Guid id, [FromBody] Casino casino )
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
    }

    [HttpDelete( "{id}" )]
    [Authorize( Policy = AuthorizationConstants.Claim_Policy_IsAdmin )]
    public async Task<IResult> Delete( Guid id )
    {
      var casinoManager = ServerSystem.Instance?.Get<ICasinoManager>( ManagerNames.CasinoManager );
      if( casinoManager == null )
        return Results.BadRequest();
      var successful = await casinoManager.DeleteCasino( id );

      return successful ?
              Results.Ok( "Casino with id " + id + "deleted" ) :
              Results.BadRequest( "Something went wrong" );
    }
  }
}
