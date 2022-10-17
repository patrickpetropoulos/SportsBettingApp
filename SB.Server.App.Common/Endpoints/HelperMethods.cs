using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace SB.Server.App.Common.Endpoints;

public static class HelperMethods
{
	public static async Task<ApplicationUser?> GetUserFromClaimsPrincipal( ClaimsPrincipal claimsPrincipal, UserManager<ApplicationUser> userManager )
	{
		var userId = Guid.Parse( claimsPrincipal.Claims.First( c => c.Type.Equals( JwtRegisteredClaimNames.Sub ) ).Value );
		var user = await userManager.Users.FirstOrDefaultAsync( c => c.Id.Equals( userId ) );
		if( user != null )
			return user;

		var username = claimsPrincipal.Claims.First( c => c.Type.Equals( JwtRegisteredClaimNames.Name ) ).Value;
		user = await userManager.Users.FirstOrDefaultAsync( c => c.Email.Equals( username ) );
		if( user != null )
			return user;

		var userEmail = claimsPrincipal.Claims.First( c => c.Type.Equals( JwtRegisteredClaimNames.Email ) ).Value;
		user = await userManager.Users.FirstOrDefaultAsync( c => c.Email.Equals( userEmail ) );
		if( user != null )
			return user;

		return null;
	}

	public static List<ClaimRecord> GetClaimRecordFromClaimPrincipal( ClaimsPrincipal claimsPrincipal )
	{
		return claimsPrincipal.Claims.Select( c => new ClaimRecord() { Type = c.Type, Value = c.Value } ).ToList();
	}
}