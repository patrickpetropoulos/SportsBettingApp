using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SB.Server.App.Common;
using SB.Server.Common.Managers;
using SB.Server.Root.Casinos;
using System.Net;

namespace SB.Server.App.Api.Controllers.v1;

[Route( "api/v{verion:apiVersion}/[controller]" )]
[ApiController]
[ApiVersion( "1.0" )]
[Produces( "application/json" )]
public class CasinosController : ControllerBase
{
	private readonly UserManager<ApplicationUser> _userManager;
	public ICasinoManager? _casinoManager { get; }

	public CasinosController( UserManager<ApplicationUser> userManager )
	{
		_casinoManager = ServerSystem.Instance?.Get<ICasinoManager>( ManagerNames.CasinoManager );
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
		if( _casinoManager == null )
		{
			HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return new JsonResult( "Internal Error accessing casinos" );
		}
		var casinos = await _casinoManager.GetAllCasinos();

		return new JsonResult( new { casinos = casinos } );
	}

	/// <summary>
	/// Retrieves casino from Database by Id
	/// </summary>
	/// <remarks></remarks>
	/// <response code="200">Casino</response>
	/// <response code="404">Casino Not Found</response>
	/// <response code="500">Internal Server Error</response>
	[ProducesResponseType( typeof( Casino ), 200 )]
	[ProducesResponseType( typeof( string ), 404 )]
	[ProducesResponseType( typeof( string ), 500 )]
	[HttpGet( "{id}" )]
	[AllowAnonymous]
	public async Task<IActionResult> GetById( Guid id )
	{
		if( _casinoManager == null )
		{
			HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			return new JsonResult( "Internal Error accessing casinos" );
		}
		var casino = await _casinoManager.GetCasinoByIdAsync( id );

		return new JsonResult( new { casino = casino } );
	}


	[HttpPost]
	[Authorize( Policy = AuthorizationConstants.Claim_Policy_IsAdmin )]
	public async Task<IActionResult> Post( [FromBody] Casino casino )
	{
		if( _casinoManager == null )
		{
			HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			return new JsonResult( "Internal Error accessing casinos" );
		}
		await _casinoManager.UpsertCasino( casino );

		return new JsonResult( new { casino = casino } );
	}

	[HttpPut( "{id}" )]
	[Authorize( Policy = AuthorizationConstants.Claim_Policy_IsAdmin )]
	//TODO for put, do we need to pass in ID in url?  why are we doing that??  shouldn't we just check on body
	public async Task<IActionResult> Put( Guid id, [FromBody] Casino casino )
	{
		if( casino.Id != id )
		{
			//TODO put these errors somewhere and ensure we get them
			HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return new JsonResult( "Id in url doesn't match Id in object" );
		}

		if( _casinoManager == null )
		{
			HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			return new JsonResult( "Internal Error accessing casinos" );
		}
		await _casinoManager.UpsertCasino( casino );

		return new JsonResult( new { casino = casino } );
	}

	[HttpDelete( "{id}" )]
	[Authorize( Policy = AuthorizationConstants.Claim_Policy_IsAdmin )]
	public async Task<IActionResult> Delete( Guid id )
	{
		if( _casinoManager == null )
		{
			HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			return new JsonResult( "Internal Error accessing casinos" );
		}
		var successful = await _casinoManager.DeleteCasino( id );
		if( successful )
		{
			return new JsonResult( "Casino with id " + id + "deleted" );
		}
		else
		{
			HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			return new JsonResult( "Something went wrong" );
		}
	}
}
