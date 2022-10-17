using Microsoft.AspNetCore.Mvc;

namespace SB.Server.App.Api.Controllers;

[Route( "api/v{version:apiVersion}/[controller]" )]
[ApiController]
[ApiVersionNeutral]
public class HealthController : ControllerBase
{
	[HttpGet]
	[Route( "ping" )]
	public IActionResult Ping()
	{
		return Ok( "Everything seems great!" );
	}
}
