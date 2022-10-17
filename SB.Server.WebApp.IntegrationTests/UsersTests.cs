using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SB.Server.App.Common;
using System.Net;
using static SB.Server.App.Common.AuthorizationConstants;

namespace SB.Server.WebApp.IntegrationTests;
public class UsersTests
{
	public string Endpoint = "";
	public void SetEndpoint( string version )
	{
		Endpoint = $"/api/{version}/users/";
	}

	#region CurrentUserTests
	[TestCase( "v1" )]
	public async Task GetCurrentUserInfo_AuthorizedUser_IsAdmin( string version )
	{
		SetEndpoint( version );
		var client = await Config.GetAuthorizedClientWithAdminAccessLevel();
		var response = await client.GetAsync( Endpoint );

		Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );

		var json = JObject.Parse( await response.Content.ReadAsStringAsync() );

		var applicationUser = JsonConvert.DeserializeObject<ApplicationUser>( ( (JObject?)json?["user"] ).ToString() );

		Assert.AreEqual( "poweruser", applicationUser?.UserName );
		Assert.AreEqual( "patrickpetropoulos@gmail.com", applicationUser?.Email );

		var claims = JsonConvert.DeserializeObject<List<ClaimRecord>>( ( (JArray?)json?["claims"] ).ToString() );

		var accessLevel = claims.FirstOrDefault( c => c.Type.Equals( Claim_AccessLevel_Type ) );

		Assert.AreEqual( accessLevel.Value, Claim_AccessLevel_Admin );
	}

	[TestCase( "v1" )]
	public async Task GetCurrentUserInfo_AuthorizedUser_IsNotAdmin( string version )
	{
		SetEndpoint( version );
		var client = await Config.GetAuthorizedClientWithoutAdminAccessLevel();
		var response = await client.GetAsync( Endpoint );

		Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );

		var json = JObject.Parse( await response.Content.ReadAsStringAsync() );

		var applicationUser = JsonConvert.DeserializeObject<ApplicationUser>( ( (JObject?)json?["user"] ).ToString() );

		Assert.AreEqual( "basicaccessuser", applicationUser?.UserName );
		Assert.AreEqual( "patrickpetropoulos@protonmail.com", applicationUser?.Email );

		var claims = JsonConvert.DeserializeObject<List<ClaimRecord>>( ( (JArray?)json?["claims"] ).ToString() );

		var accessLevel = claims.FirstOrDefault( c => c.Type.Equals( Claim_AccessLevel_Type ) );
		Assert.AreEqual( accessLevel, null );
	}
	[TestCase( "v1" )]
	public async Task GetCurrentUserInfo_NotAuthorizedUser( string version )
	{
		SetEndpoint( version );
		var client = Config.GetClientWithUnauthorizedUser();
		var response = await client.GetAsync( Endpoint );

		Assert.AreEqual( HttpStatusCode.Unauthorized, response.StatusCode );
	}
	#endregion
}
