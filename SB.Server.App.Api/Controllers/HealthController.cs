using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SB.Server.App.Api.Controllers;

/// <summary>
/// Class to handle Health of Controllers
/// </summary>
[Route( "api/[controller]" )]
[ApiController]
[ApiVersionNeutral]
[AllowAnonymous]
public class HealthController : ControllerBase
{
	/// <summary>
	/// Checks the health of Controller
	/// </summary>
	/// <remarks></remarks>
	/// <response code="200"></response>
	[HttpGet]
	[Route( "ping" )]
	public IActionResult Ping()
	{
		return Ok( "Everything seems great!" );
	}
}
