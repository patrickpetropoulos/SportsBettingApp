using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SB.Server.App.Common.Controllers;

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
