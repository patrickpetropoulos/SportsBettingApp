using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SB.Server.App.Common;
using SB.Server.Common.Managers;
using SB.Server.Root.Casinos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SB.Server.App.Api.Controllers.v1;

[Route( "api/v{verion:apiVersion}/[controller]" )]
[ApiController]
[ApiVersion("1.0")]
[Produces( "application/json" )]
public class CasinosController : ControllerBase
{
  /// <summary>
  /// Retrieves a specific product by unique id
  /// </summary>
  /// <remarks>Awesomeness!</remarks>
  /// <response code="200">Product created</response>
  /// <response code="400">Product has missing/invalid values</response>
  /// <response code="500">Oops! Can't create your product right now</response>
  [ProducesResponseType( typeof( Casino ), 200 )]
  [ProducesResponseType( typeof( IDictionary<string, string> ), 400 )]
  [ProducesResponseType( 500 )]
  [HttpGet]
  [AllowAnonymous]
  public async Task<IActionResult> Get()
  {
    var casinoManager = ServerSystem.Instance?.Get<ICasinoManager>( ManagerNames.CasinoManager );
    if( casinoManager == null )
      return new JsonResult( "error" );
    var casinos = await casinoManager.GetAllCasinos();

    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    return new JsonResult( new { casinosLister = casinos } );


    //var result = Results.Ok(new { casinos } );

    //return result;
    //return Results.Ok( new { casinos } );
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
