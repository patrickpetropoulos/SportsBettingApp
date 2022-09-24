using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static SB.Server.WebApp.AuthorizationConstants;

namespace SB.Server.WebApp.IntegrationTests;
public class UsersTests
{
  #region CurrentUserTests
  [Test]
  public async Task GetCurrentUserInfo_AuthorizedUser_IsAdmin()
  {
    var client = await Config.GetAuthorizedClientWithAdminAccessLevel();
    var response = await client.GetAsync( "/api/users/current" );
        
    Assert.AreEqual( HttpStatusCode.OK, response.StatusCode);
        
    var result = await response.Content.ReadAsStringAsync();
    var json = JObject.Parse( result );
        
    var applicationUser = JsonConvert.DeserializeObject<ApplicationUser>(((JObject?) json?["user"]).ToString());
        
    Assert.AreEqual( "poweruser", applicationUser?.UserName );
    Assert.AreEqual("patrickpetropoulos@gmail.com", applicationUser?.Email);

    var claims = JsonConvert.DeserializeObject<List<ClaimRecord>>(((JArray?) json?["claims"]).ToString());
        
    //Assert.AreEqual(9, claims!.Count);

    var accessLevel = claims.FirstOrDefault(c => c.Type.Equals(Claim_AccessLevel_Type));
        
    Assert.AreEqual(accessLevel.Value, Claim_AccessLevel_Admin);
  }
  
  [Test]
  public async Task GetCurrentUserInfo_AuthorizedUser_IsNotAdmin()
  {
    var client = await Config.GetAuthorizedClientWithoutAdminAccessLevel();
    var response = await client.GetAsync( "/api/users/current" );
        
    Assert.AreEqual( HttpStatusCode.OK, response.StatusCode);
        
    var result = await response.Content.ReadAsStringAsync();
    var json = JObject.Parse( result );
        
    var applicationUser = JsonConvert.DeserializeObject<ApplicationUser>(((JObject?) json?["user"]).ToString());
        
    Assert.AreEqual( "basicaccessuser", applicationUser?.UserName );
    Assert.AreEqual("patrickpetropoulos@protonmail.com", applicationUser?.Email);

    var claims = JsonConvert.DeserializeObject<List<ClaimRecord>>(((JArray?) json?["claims"]).ToString());
        
    //Assert.AreEqual(9, claims!.Count);

    var accessLevel = claims.FirstOrDefault(c => c.Type.Equals(Claim_AccessLevel_Type));
    Assert.AreEqual(accessLevel, null);
  }
  
  [Test]
  public async Task GetCurrentUserInfo_NotAuthorizedUser()
  {
    var client = Config.GetClientWithUnauthorizedUser();
    var response = await client.GetAsync( "/api/users/current" );
        
    Assert.AreEqual( HttpStatusCode.Unauthorized, response.StatusCode);
  }
  #endregion
}
