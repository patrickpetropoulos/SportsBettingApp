using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace SB.Server.App.Common.Endpoints;

public static class UsersEndpoints
{

	public static WebApplication MapUsersEndpoints( this WebApplication app )
	{
		app.MapGetCurrentUserInfo();
		app.MapDeleteUser();
		return app;
	}

	private static void MapGetCurrentUserInfo( this WebApplication app )
	{
		app.MapGet( "/api/users/current",
				async ( ClaimsPrincipal claimsPrincipal,
					UserManager<ApplicationUser> userManager ) =>
				{
					var user = await HelperMethods.GetUserFromClaimsPrincipal( claimsPrincipal, userManager );
					if( user == null )
						return Results.NotFound();
					var claims = claimsPrincipal.Claims.Select( c => new ClaimRecord() { Type = c.Type, Value = c.Value } ).ToList();
					var roles = await userManager.GetRolesAsync( user );

					var result = new { user, roles, claims };

					return Results.Ok( result );
				} );
	}

	private static void MapDeleteUser( this WebApplication app )
	{
		app.MapDelete( "/api/users/{id}",
				async ( UserManager<ApplicationUser> userManager,
					ClaimsPrincipal claimsPrincipal,
					Guid id ) =>
				{
					var user = await HelperMethods.GetUserFromClaimsPrincipal( claimsPrincipal, userManager );
					if( user == null )
						return Results.NotFound();
					var accessLevelClaim = claimsPrincipal.Claims.FirstOrDefault( c =>
						c.Type.Equals( AuthorizationConstants.Claim_AccessLevel_Type ) );

					if( id.Equals( user.Id ) || accessLevelClaim is
						{ Value: AuthorizationConstants.Claim_AccessLevel_Admin } )
					{
						return Results.Ok( "I deleted my user" );
					}

					return Results.Forbid();
				}
			);
	}

}