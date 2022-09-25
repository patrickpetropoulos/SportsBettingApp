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
public class DbSeeding
{

  public static async Task SeedDatabase(UserManager<ApplicationUser> userManager, IConfiguration configuration)
  {
    await CreateUsersAndClaims(userManager, configuration);
    await SeedCasinos();
  }
  public static async Task CreateUsersAndClaims( UserManager<ApplicationUser> userManager, IConfiguration configuration)
  {
    var usersToCreate = configuration.GetSection("InitialUsers").Get<UserRecord[]>();

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

  public static async Task<ApplicationUser?> CreateUser(UserManager<ApplicationUser> userManager, UserRecord userRecord)
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
  
  public static async Task SeedCasinos()
  {
    var casinoManager = ServerSystem.Instance.Get<ICasinoManager>( ManagerNames.CasinoManager);

    var currentCasinos = await casinoManager.GetAllCasinos();
    if( !currentCasinos.Any() )
    {
      foreach( var casino in GetSeedCasinos() )
      {
        await casinoManager.UpsertCasino( casino );
      }
    }
    else
    {
      //_casinoList = currentCasinos;
    }
  }
  
  public static List<Casino> GetSeedCasinos()
  {
    var assembly = Assembly.GetExecutingAssembly();
    using( var stream = assembly.GetManifestResourceStream( "SB.Server.WebApp.SeedData.Casino_Seed_Data.json" ) )
    using (var reader = new StreamReader(stream))
    {
      string text = reader.ReadToEnd();
      var json = JArray.Parse(text);

      return JsonConvert.DeserializeObject<List<Casino>>((json).ToString());
    }
  }
}



