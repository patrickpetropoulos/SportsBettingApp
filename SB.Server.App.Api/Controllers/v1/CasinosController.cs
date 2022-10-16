using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SB.Server.App.Common;
using SB.Server.App.Common.Endpoints;
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
  private readonly UserManager<ApplicationUser> _userManager;

  public ICasinoManager? _casinoManager { get; }

  public CasinosController(UserManager<ApplicationUser> userManager)
  {
    _casinoManager = ServerSystem.Instance?.Get<ICasinoManager>(ManagerNames.CasinoManager);
    _userManager = userManager;
  }

  /// <summary>
  /// Retrieves all casinos from Database
  /// </summary>
  /// <remarks></remarks>
  /// <response code="200"></response>
  /// <response code="500">Internal Server Error</response>
  [ProducesResponseType( typeof( Casino ), 200 )]
  [ProducesResponseType( typeof( string ), 500 )]
  [HttpGet]
  [AllowAnonymous]
  public async Task<IActionResult> Get()
  {
    if( _casinoManager == null)
    {
      HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
      return new JsonResult("Internal Error accessing casinos");
    }
    var casinos = await _casinoManager.GetAllCasinos();

    return new JsonResult( new { casinos = casinos} );
  }

  //TODO add in method to get casino by ID

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
