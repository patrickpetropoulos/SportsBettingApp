﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

  public static ApplicationUser? GetUserFromClaims( ClaimsPrincipal user, ApplicationDbContext context )
  {
    //TODO add try catch
    var userEmail = user.Claims.First( c => c.Type.Equals( "email" ) ).Value;

    return context.Users.FirstOrDefault( c => c.Email.Equals( userEmail ) );
  }

  [Route( "/token" )]
  [HttpGet]
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
  [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin" )]
  public async Task<IResult> Get()
  {
    var user = GetUserFromClaims( User, _context );
    var claims = _userManager.GetClaimsAsync(user);
    var roles = _userManager.GetRolesAsync(user);

    var json = new JObject();
    
    json.Add("user", user.ToString());
    json.Add("roles", roles.ToString());

    var result = new {user = user, roles = roles, claims = claims};
    
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
        new JwtPayload( claims ) );

    var output = new
    {
      AccessToken = new JwtSecurityTokenHandler().WriteToken( token ),
      Expiration = expirationDate.ToString(),
    };

    return output;
  }
}