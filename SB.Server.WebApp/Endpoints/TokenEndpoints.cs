using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace SB.Server.WebApp.Endpoints;

public class UserRecord
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public static class TokenEndpoints
{
    public static WebApplication MapTokenEndpoints(this WebApplication app)
    {
        app.MapGenerateToken();
        app.MapTestToken();
        return app;
    }

    public static WebApplication MapGenerateToken(this WebApplication app)
    {
        app.MapPost("/token",
            async (ApplicationDbContext context, 
                UserManager<ApplicationUser> userManager, 
                IConfiguration configuration,
                [FromBody] UserRecord userRecord) => 
                await CreateToken(context, userManager, configuration, userRecord))
            .AllowAnonymous();
        return app;
    }

    public static WebApplication MapTestToken(this WebApplication app)
    {
        app.MapGet("/test",
            async (ClaimsPrincipal claimsPrincipal,
                UserManager<ApplicationUser> userManager) =>
            {
                var userEmail = claimsPrincipal.Claims.First( c => c.Type.Equals( "email" ) ).Value;

                var user = await userManager.Users.FirstOrDefaultAsync( c => c.Email.Equals( userEmail ) );
                if (user == null)
                    return Results.NotFound();
                var claims = claimsPrincipal.Claims.Select(c => new ClaimRecord(){Type = c.Type, Value = c.Value}).ToList();
                var roles = await userManager.GetRolesAsync(user!);

                var result = new {user, roles, claims};
    
                return Results.Ok(result);
            })
            .RequireAuthorization("IsAdmin");
        return app;
    }

    public static async Task<IResult> CreateToken(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        UserRecord userRecord)
    {
        if( await IsValidLoginInfo( userManager, userRecord ) )
        {
            return Results.Ok( await GenerateToken( context, userManager, configuration, userRecord ) );
        }
        else
        {
            return Results.BadRequest();
        }
    }
    
    private static async Task<bool> IsValidLoginInfo( UserManager<ApplicationUser> userManager, UserRecord userRecord )
  {
    //Accepting username or email for login
    var user = await userManager.FindByEmailAsync( userRecord.Username ) ?? await userManager.FindByNameAsync( userRecord.Username );
    return await userManager.CheckPasswordAsync( user, userRecord.Password );
  }

  private static async Task<dynamic> GenerateToken( ApplicationDbContext context, UserManager<ApplicationUser> userManager, 
      IConfiguration configuration, UserRecord userRecord )
  {
    var user = await userManager.FindByEmailAsync( userRecord.Username  ) ?? await userManager.FindByNameAsync( userRecord.Username  );
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
                new SymmetricSecurityKey( Encoding.UTF8.GetBytes( configuration.GetSection( "JWT" ).Value ) ),
                SecurityAlgorithms.HmacSha256 ) ),
        new JwtPayload( 
          //TODO move these to appsettings
          "PatricksApp", //Issuer
        "PatricksAppUser", //Audience
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