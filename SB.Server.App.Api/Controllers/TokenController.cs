using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SB.Server.App.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace SB.Server.App.Api.Controllers;

public class UserCredentials
{
	public string Username { get; set; }
	public string Password { get; set; }
}


[Route( "api/v{version:apiVersion}/[controller]" )]
[ApiController]
[ApiVersionNeutral]
public class TokenController : ControllerBase
{
	private readonly UserManager<ApplicationUser> userManager;
	private readonly IConfiguration configuration;

	public TokenController( UserManager<ApplicationUser> userManager,
			  IConfiguration configuration )
	{
		this.userManager = userManager;
		this.configuration = configuration;
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<IActionResult> GenerateToken( [FromBody] UserCredentials userRecord )
	{
		var user = await IsValidLoginInfo( userManager, userRecord );
		if( user != null )
		{
			return new JsonResult( await GenerateToken( userManager, configuration, user ) );
		}

		HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
		return new JsonResult( "Internal Error Generating Token" );
	}

	private static async Task<ApplicationUser?> IsValidLoginInfo( UserManager<ApplicationUser> userManager, UserCredentials userRecord )
	{
		//Accepting username or email for login
		var user = await userManager.FindByEmailAsync( userRecord.Username ) ?? await userManager.FindByNameAsync( userRecord.Username );
		return await userManager.CheckPasswordAsync( user, userRecord.Password ) ? user : null;
	}

	private static async Task<dynamic> GenerateToken( UserManager<ApplicationUser> userManager,
	  IConfiguration configuration, ApplicationUser user )
	{
		// var roles = from ur in context.UserRoles
		//             join r in context.Roles on ur.RoleId equals r.Id
		//             where ur.UserId == user.Id
		//             select new { ur.UserId, ur.RoleId, r.Name };
		var claimList = await userManager.GetClaimsAsync( user );

		//TODO fix all this, claim, clarifying it
		var expirationDate = new DateTimeOffset( DateTime.Now.AddDays( 10 ) );

		var claims = new List<Claim>
				{
					new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
					new (JwtRegisteredClaimNames.UniqueName, user.UserName),
					new (JwtRegisteredClaimNames.Email, user.Email),
					new (JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
					new (JwtRegisteredClaimNames.Exp, expirationDate.ToUnixTimeSeconds().ToString())
				};

		claims.AddRange( claimList );

		var token = new JwtSecurityToken(
			new JwtHeader(
				new SigningCredentials(
					new SymmetricSecurityKey( Encoding.UTF8.GetBytes( configuration.GetValue<string>( "JWT:signingKey" ) ) ),
					SecurityAlgorithms.HmacSha256 ) ),
			new JwtPayload(
			  configuration.GetValue<string>( "JWT:issuer" ), //Issuer
			  configuration.GetValue<string>( "JWT:audience" ), //Audience
			claims,
			DateTime.UtcNow, //When token is valid
			DateTime.UtcNow.AddDays( 1 ) //When token will expire  TODO do a test that token expires
			) );

		var output = new
		{
			AccessToken = new JwtSecurityTokenHandler().WriteToken( token ),
			Expiration = expirationDate.ToString(),
		};

		return output;
	}

}
