using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SB.Server.WebApp.Endpoints;
using SB.Server.WebApp.Startup;

namespace SB.Server.WebApp;

public class Program{
  public static void Main( string[] args )
  {
    var builder = WebApplication.CreateBuilder( args );

    //Need to set this up if dont want all schemas text crap in JWT claims
    //https://github.com/dotnet/aspnetcore/issues/4660
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

    builder.Services.RegisterAllServices(builder.Configuration);
    
    var app = builder.Build();

    //TODO eventually add in logging to ApplicationInsights
    ServerSystem.CreateInstance( app.Services, app.Configuration );
    
    // Configure the HTTP request pipeline.
    if( app.Environment.IsDevelopment() )
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }
    if( app.Environment.IsEnvironment( "Testing" ) )
    {
      Console.WriteLine("#### PATRICK : I am running Testing in Program.cs");
    }
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    //app.MapControllers();

    //Mapping Endpoints
    app.MapTokenEndpoints();
    app.MapUsersEndpoints();

    app.MapCasinosEndpoints();
    
    
    using( var scope = app.Services.CreateScope() )
    {
      scope.ServiceProvider.GetService<ApplicationDbContext>()?.Database.Migrate();
      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
      //Seed Users
      var userSeeding = new UserSeeding(userManager, app.Configuration);
      userSeeding.SeedDatabase().GetAwaiter().GetResult();
      //Seed all other data
      var dataSeeding = new DataSeeding();
      dataSeeding.SeedDatabase().GetAwaiter().GetResult();
    }
    app.Run();
  }
}