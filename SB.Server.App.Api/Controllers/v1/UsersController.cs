using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SB.Server.App.Common;
using SB.Server.App.Common.Endpoints;
using System.Net;

namespace SB.Server.App.Api.Controllers.v1;

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
	public async Task<IActionResult> Get()
	{
		var user = await HelperMethods.GetUserFromClaimsPrincipal( User, userManager );
		if( user == null )
		{
			HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
			return new JsonResult( "User not found" );
		}
		var claims = User.Claims.Select( c => new ClaimRecord() { Type = c.Type, Value = c.Value } ).ToList();
		var roles = await userManager.GetRolesAsync( user );

		var result = new { user, roles, claims };

		return new JsonResult( result );

	}

	[HttpDelete( "{id}" )]
	public async Task<IResult> Delete( Guid id )
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