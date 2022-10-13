using SB.Server.App.Common;
using System.IdentityModel.Tokens.Jwt;
using SB.Server.App.Common.Startup;

namespace SB.Server.WebApp;

public class Program
{
  public static void Main( string[] args )
  {
    var builder = WebApplication.CreateBuilder( args );

    //Need to set this up if dont want all schemas text crap in JWT claims
    //https://github.com/dotnet/aspnetcore/issues/4660
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

    builder.Services.RegisterAllServices( builder.Configuration, typeof( Program ).Assembly.FullName );

    var app = builder.Build();
    //TODO eventually add in logging to ApplicationInsights
    ServerSystem.CreateInstance( app.Services, app.Configuration );

    AppSetup.SetupApplication( app );
    AppSetup.SeedApplication( app );

    app.Run();
  }
}