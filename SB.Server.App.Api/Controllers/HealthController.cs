using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SB.Server.App.Api.Controllers;

[Route( "api/[controller]" )]
[ApiController]
[ApiVersionNeutral]
[AllowAnonymous]
public class HealthController : ControllerBase
{
	[HttpGet]
	[Route( "ping" )]
	public IActionResult Ping()
	{
		return Ok( "Everything seems great!" );
	}
}
