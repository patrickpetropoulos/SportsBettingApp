using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SB.Server.App.Common;
using SB.Server.Common.Managers;
using SB.Server.Root.CasinoGames;
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

	[HttpGet]
	[AllowAnonymous]
	public async Task<IActionResult> Get()
	{
		if( _casinoGameManager == null )
		{
			HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return new JsonResult( "Internal Error accessing casinos" );
		}
		var casinoGames = await _casinoGameManager.GetAllCasinoGames();

		return new JsonResult( new { casinogames = casinoGames } );
	}
}
