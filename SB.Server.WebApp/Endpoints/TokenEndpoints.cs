﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static SB.Server.WebApp.AuthorizationConstants;

namespace SB.Server.WebApp.Endpoints;

public class UserRecord
{
    public string Username { get; set;}
    public string Password { get; set;}
}

public static class TokenEndpoints
{
    public static WebApplication MapTokenEndpoints(this WebApplication app)
    {
        app.MapGenerateToken();
        app.MapTestToken();
        return app;
    }

    private static void MapGenerateToken(this WebApplication app)
    {
        app.MapPost("/token",
            async (UserManager<ApplicationUser> userManager, 
                IConfiguration configuration,
                [FromBody] UserRecord userRecord) => 
                    await CreateToken( userManager, configuration, userRecord))
            .AllowAnonymous();
    }

    private static void MapTestToken(this WebApplication app)
    {
        app.MapGet("/test",
            async (ClaimsPrincipal claimsPrincipal,
                UserManager<ApplicationUser> userManager) =>
            {
                var user = await HelperMethods.GetUserFromClaimsPrincipal(claimsPrincipal, userManager);
                if (user == null)
                    return Results.NotFound();
                var claims = HelperMethods.GetClaimRecordFromClaimPrincipal(claimsPrincipal);
                var roles = await userManager.GetRolesAsync(user);

                var result = new {user, roles, claims};
    
                return Results.Ok(result);
            })
            .RequireAuthorization(Claim_Policy_IsAdmin);
    }

    private static async Task<IResult> CreateToken(UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        UserRecord userRecord)
    {
        var user = await IsValidLoginInfo(userManager, userRecord);
        return user != null ? 
            Results.Ok( await GenerateToken( userManager, configuration, user ) ) : 
            Results.BadRequest();
    } 
    
    private static async Task<ApplicationUser?> IsValidLoginInfo( UserManager<ApplicationUser> userManager, UserRecord userRecord )
    {
        //Accepting username or email for login
        var user = await userManager.FindByEmailAsync( userRecord.Username ) ?? await userManager.FindByNameAsync( userRecord.Username );
        return await userManager.CheckPasswordAsync(user, userRecord.Password) ? user : null;
    }

    private static async Task<dynamic> GenerateToken( UserManager<ApplicationUser> userManager, 
      IConfiguration configuration, ApplicationUser user )
    {
        // var roles = from ur in context.UserRoles
        //             join r in context.Roles on ur.RoleId equals r.Id
        //             where ur.UserId == user.Id
        //             select new { ur.UserId, ur.RoleId, r.Name };
        var claimList = await userManager.GetClaimsAsync(user);

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

        claims.AddRange(claimList);

        var token = new JwtSecurityToken(
            new JwtHeader(
                new SigningCredentials(
                    new SymmetricSecurityKey( Encoding.UTF8.GetBytes( configuration.GetValue<string>("JWT:signingKey") ) ),
                    SecurityAlgorithms.HmacSha256 ) ),
            new JwtPayload( 
              configuration.GetValue<string>("JWT:issuer"), //Issuer
              configuration.GetValue<string>("JWT:audience"), //Audience
            claims, 
            DateTime.UtcNow, //When token is valid
            DateTime.UtcNow.AddDays(1) //When token will expire  TODO do a test that token expires
            ) );

        var output = new
        {
          AccessToken = new JwtSecurityTokenHandler().WriteToken( token ),
          Expiration = expirationDate.ToString(),
        };

        return output;
    }
}