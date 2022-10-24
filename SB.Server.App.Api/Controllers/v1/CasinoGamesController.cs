using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SB.Server.Common.Managers;
using SB.Server.Root.CasinoGames;
using SB.Server.App.Api;
using System.Net;

namespace SB.Server.App.Api.Controllers.v1;

[Route( "api/v{verion:apiVersion}/[controller]" )]
[ApiController]
[ApiVersion( "1.0" )]
public class CasinoGamesController : ControllerBase
{
	private readonly UserManager<ApplicationUser> _userManager;
	public ICasinoGameManager? _casinoGameManager { get; }

	public CasinoGamesController( UserManager<ApplicationUser> userManager )
	{
		_casinoGameManager = ServerSystem.Instance?.Get<ICasinoGameManager>( ManagerNames.CasinoGameManager );
		_userManager = userManager;
	}

	/// <summary>
	/// Retrieves all casino Games from Database
	/// </summary>
	/// <remarks></remarks>
	/// <response code="200"></response>
	/// <response code="500">Internal Server Error</response>
	[ProducesResponseType( typeof( CasinoGame ), 200 )]
	[ProducesResponseType( typeof( string ), 500 )]
	[HttpGet]
	[AllowAnonymous]
	public async Task<IActionResult> Get()
	{
		if( _casinoGameManager == null )
		{
			HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return new JsonResult( "Internal Error accessing casino games" );
		}
		var casinoGames = await _casinoGameManager.GetAllCasinoGames();

		return new JsonResult( new { casinogames = casinoGames } );
	}
}
