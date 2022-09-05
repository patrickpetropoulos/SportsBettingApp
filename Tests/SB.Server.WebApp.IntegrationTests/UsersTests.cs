using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SB.Server.WebApp.IntegrationTests;
public class UsersTests
{
  [Test]
  public async Task GetUsers_HasSuccessResponseCode()
  {
    var client = Config.GetClient();
    var response = await client.GetAsync( "/api/users" );
    var result = await response.Content.ReadAsStringAsync();
    var json = JObject.Parse( result );

    var statueCode = (int)json["statusCode"];

    Assert.AreEqual( 200, statueCode );
  }
 

}
