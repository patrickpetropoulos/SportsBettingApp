using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SB.Server.Common.Managers;
using SB.Server.Root.CasinoGames;
using SB.Server.Root.Casinos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SB.Server.App.Common.Controllers.v1;

[Route( "api/v{verion:apiVersion}/[controller]" )]
[ApiController]
[ApiVersion( "1.0" )]
public class CasinoGamesController : ControllerBase
{
  [HttpGet]
  [AllowAnonymous]
  public async Task<IResult> Get()
  {
    var casinoGameManager = ServerSystem.Instance?.Get<ICasinoGameManager>( ManagerNames.CasinoGameManager );
    if( casinoGameManager == null )
      return Results.BadRequest();
    var casinoGames = await casinoGameManager.GetAllCasinoGames();

    return Results.Ok( new { casinogames = casinoGames } );
  }
}
