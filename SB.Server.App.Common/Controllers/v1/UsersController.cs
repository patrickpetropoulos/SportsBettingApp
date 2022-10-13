using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SB.Server.App.Common.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SB.Server.App.Common.Controllers.v1;

[Route( "api/v{verion:apiVersion}/[controller]" )]
[ApiController]
[ApiVersion( "1.0" )]
public class UsersController : ControllerBase
{
  private readonly UserManager<ApplicationUser> userManager;

  public UsersController( UserManager<ApplicationUser> userManager )
  {
    this.userManager = userManager;
  }
  [HttpGet]
  public async Task<IResult> Get()
  {
    var user = await HelperMethods.GetUserFromClaimsPrincipal( User, userManager );
    if( user == null )
      return Results.NotFound();
    var claims = User.Claims.Select( c => new ClaimRecord() { Type = c.Type, Value = c.Value } ).ToList();
    var roles = await userManager.GetRolesAsync( user );

    var result = new { user, roles, claims };

    return Results.Ok( result );
    
  }

  [HttpDelete( "{id}" )]
  public async Task<IResult> Delete(Guid id)
  {
    //var user = await HelperMethods.GetUserFromClaimsPrincipal( claimsPrincipal, userManager );
    //if( user == null )
    //  return Results.NotFound();
    //var accessLevelClaim = claimsPrincipal.Claims.FirstOrDefault( c =>
    //        c.Type.Equals( AuthorizationConstants.Claim_AccessLevel_Type ) );

    //if( id.Equals( user.Id ) || accessLevelClaim is
    //  { Value: AuthorizationConstants.Claim_AccessLevel_Admin } )
    //{
    //  return Results.Ok( "I deleted my user" );
    //}

    return Results.Forbid();
  }


}