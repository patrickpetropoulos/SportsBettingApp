using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
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
      //Check by email and username if user already exists, if not create it
      var user = await userManager.FindByEmailAsync( userRecord.Email );
      if (user != null) continue;
      user = await userManager.FindByNameAsync(userRecord.Username);
      if (user != null) continue;
      user = new ApplicationUser()
      {
        Email = userRecord.Email,
        UserName = userRecord.Username
      };
          
      var result = await userManager.CreateAsync(user, userRecord.Password);
      if (!result.Succeeded) continue;
      //TODO need to investigate if claim can exist multiple times in db, with same userId-Type-Value pair
      foreach (var claim in userRecord.Claims)
      {
        await userManager.AddClaimAsync(user, new Claim(claim.Type, claim.Value));
      }
    }
  }
  
  public static async Task SeedCasinos()
  {
    var casinoManager = ServerSystem.Instance.Get<ICasinoManager>( "CasinoManager" );

    var currentCasinos = await casinoManager.GetAllCasinos();
    if( !currentCasinos.Any() )
    {
      foreach( var casino in GetListOfCasinos() )
      {
        await casinoManager.UpsertCasino( casino );
      }
    }
    else
    {
      //_casinoList = currentCasinos;
    }
  }
  
  public static List<Casino> GetListOfCasinos()
  {
    var casinoList = new List<Casino>();
    casinoList.Add( new Casino()
    {
      Name = "Borgata",
      CountryCode = "US"
    } );
    casinoList.Add( new Casino()
    {
      Name = "Bellagio",
      CountryCode = "US"
    } );
    casinoList.Add( new Casino()
    {
      Name = "Montreal Casino",
      CountryCode = "CA"
    } );

    return casinoList;
  }
}



