using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SB.Server.Common.Managers;
using SB.Server.Root.Casinos;

namespace SB.Server.WebApp;

public class UserRecord
{
  public string Email { get; set; }
  public string Username { get; set; }
  public string Password { get; set; }
  public List<ClaimRecord> Claims { get; set; }
}

public class ClaimRecord
{
  public string Type { get; set; }
  public string Value { get; set; }
}
public class UserSeeding
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly IConfiguration _configuration;

  public UserSeeding(UserManager<ApplicationUser> userManager, IConfiguration configuration)
  {
    _userManager = userManager;
    _configuration = configuration;
  }
  public async Task SeedDatabase()
  {
    await CreateUsersAndClaims(_userManager, _configuration);
  }

  private async Task CreateUsersAndClaims( UserManager<ApplicationUser> userManager, IConfiguration configuration)
  {
    var usersToCreate = new List<UserRecord>
    {
      configuration.GetSection("PowerUser").Get<UserRecord>(),
      configuration.GetSection("BasicAccessUser").Get<UserRecord>()
    };

    foreach (var userRecord in usersToCreate)
    {
      var user = await CreateUser(userManager, userRecord);
      if (user == null)
      {
        //TODO log that couldn't be created
        //TODO move this to proper create user task
        continue;
      }
      //TODO need to investigate if claim can exist multiple times in db, with same userId-Type-Value pair
      var currentClaims = await userManager.GetClaimsAsync(user);
      
      foreach (var claim in userRecord.Claims)
      {
        var currentClaim = currentClaims.FirstOrDefault(c => c.Type.Equals(claim.Type));
        if (currentClaim == null)
        {
          await userManager.AddClaimAsync(user, new Claim(claim.Type, claim.Value));
        }
        else
        {
          await userManager.ReplaceClaimAsync(user, currentClaim, new Claim(claim.Type, claim.Value));
        }
      }
    }
  }

  private async Task<ApplicationUser?> CreateUser(UserManager<ApplicationUser> userManager, UserRecord userRecord)
  {
    var user = await userManager.FindByEmailAsync( userRecord.Email );
    if (user != null) return user;
    user = await userManager.FindByNameAsync(userRecord.Username);
    if (user != null) return user;
    user = new ApplicationUser()
    {
      Email = userRecord.Email,
      UserName = userRecord.Username
    };
          
    var result = await userManager.CreateAsync(user, userRecord.Password);
    return result.Succeeded ? user : null;
  }
}



