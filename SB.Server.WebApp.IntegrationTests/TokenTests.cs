namespace SB.Server.WebApp.IntegrationTests;

public class TokenTests
{
  // [Test]
  // public async Task GetToken_TestRoles_Succeeded()
  // {
  //     var client = await Config.GetAuthorizedClientWithAdminAccessLevel();
  //     var response = await client.GetAsync( "/api/users/current" );
  //     
  //     Assert.AreEqual( HttpStatusCode.OK, response.StatusCode);
  //     
  //     var result = await response.Content.ReadAsStringAsync();
  //     var json = JObject.Parse( result );
  //     
  //     var applicationUser = JsonConvert.DeserializeObject<ApplicationUser>(((JObject?) json?["user"]).ToString());
  //     
  //     Assert.AreEqual( "poweruser", applicationUser?.UserName );
  //     Assert.AreEqual("patrickpetropoulos@gmail.com", applicationUser?.Email);
  //
  //     var claims = JsonConvert.DeserializeObject<List<ClaimRecord>>(((JArray?) json?["claims"]).ToString());
  //     
  //     Assert.AreEqual(9, claims!.Count);
  //
  //     var accessLevel = claims.FirstOrDefault(c => c.Type.Equals(Claim_AccessLevel_Type));
  //     
  //     Assert.AreEqual(accessLevel.Value, Claim_AccessLevel_Admin);
  //
  // }
  // [Test]
  // public async Task GetToken_TestRoles_Failed()
  // {
  //     var client = Config.GetClientWithUnauthorizedUser();
  //     var response = await client.GetAsync( "/test" );
  //     
  //     Assert.AreEqual( HttpStatusCode.Unauthorized, response.StatusCode);
  // }
}