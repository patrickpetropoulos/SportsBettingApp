using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace SB.Server.WebApp.Controllers;

[ApiController]
[Route( "api" )]
public class TokenController : ControllerBase
{
  private readonly ApplicationDbContext _context;
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly IConfiguration _configuration;

  public TokenController( ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration )
  {
    _context = context;
    _userManager = userManager;
    _configuration = configuration;
  }

  public static async  Task<ApplicationUser?> GetUserFromClaims( ClaimsPrincipal user, ApplicationDbContext context )
  {
    //TODO add try catch
    var userEmail = user.Claims.First( c => c.Type.Equals( "email" ) ).Value;

    return await context.Users.FirstOrDefaultAsync( c => c.Email.Equals( userEmail ) );
  }

  [Route( "/token" )]
  [HttpGet]
  [AllowAnonymous]
  //TODO move params into body of request, figure out
  public async Task<IActionResult> Create( string username, string password )
  {
    //have db table for all tokens given out, if for user has one, put it on blacklist and give new one,
    //this manages the refresh I guess


    if( await IsValidLoginInfo( username, password ) )
    {
      return new ObjectResult( await GenerateToken( username ) );
    }
    else
    {
      return BadRequest();
    }
  }

  [Route( "/test" )]
  [HttpGet]
  //Can have multiple authorize
  [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin" )]
  public async Task<IResult> Get()
  {
    var user = await GetUserFromClaims( User, _context );
    var claims = await _userManager.GetClaimsAsync(user!);
    var roles = await _userManager.GetRolesAsync(user!);

    var result = new {user, roles, claims};
    
    return Results.Ok(result);
  }

  private async Task<bool> IsValidLoginInfo( string username, string password )
  {
    //Accepting username or email for login
    var user = await _userManager.FindByEmailAsync( username ) ?? await _userManager.FindByNameAsync( username );
    return await _userManager.CheckPasswordAsync( user, password );
  }

  private async Task<dynamic> GenerateToken( string username )
  {
    var user = await _userManager.FindByEmailAsync( username ) ?? await _userManager.FindByNameAsync( username );
    var roles = from ur in _context.UserRoles
                join r in _context.Roles on ur.RoleId equals r.Id
                where ur.UserId == user.Id
                select new { ur.UserId, ur.RoleId, r.Name };

    var expirationDate = new DateTimeOffset( DateTime.Now.AddDays( 10 ) );

    var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim("username", user.UserName),
                new Claim("email", user.Email),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, expirationDate.ToUnixTimeSeconds().ToString())
            };

    foreach( var role in roles )
    {
      claims.Add( new Claim( "role", role.Name ) );
    }

    var token = new JwtSecurityToken(
        new JwtHeader(
            new SigningCredentials(
                new SymmetricSecurityKey( Encoding.UTF8.GetBytes( _configuration.GetSection( "JWT" ).Value ) ),
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