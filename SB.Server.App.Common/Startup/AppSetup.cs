using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SB.Server.App.Common.Endpoints;

namespace SB.Server.App.Common.Startup;

public static class AppSetup
{
  public static void SetupApplication( WebApplication app )
  {
    // Configure the HTTP request pipeline.
    if( app.Environment.IsDevelopment() )
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }
    if( app.Environment.IsEnvironment( "Testing" ) )
    {
      Console.WriteLine( "#### PATRICK : I am running Testing in AppSetup.cs" );
    }
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    MapAllEndpoints( app );
  }

  private static void MapAllEndpoints( WebApplication app )
  {
    //Mapping Endpoints
    app.MapTokenEndpoints()
        .MapUsersEndpoints()
        .MapCasinosEndpoints()
        .MapCasinoGamesEndpoints();
  }

  public static void SeedApplication( WebApplication app )
  {
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetService<ApplicationDbContext>()?.Database.Migrate();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    //Seed Users
    var userSeeding = new UserSeeding( userManager, app.Configuration );
    userSeeding.SeedDatabase().GetAwaiter().GetResult();
    //Seed all other data
    var dataSeeding = new DataSeeding();
    dataSeeding.SeedDatabase().GetAwaiter().GetResult();
  }
}