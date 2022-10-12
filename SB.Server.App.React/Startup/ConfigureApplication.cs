using Microsoft.AspNetCore.Identity;
using SB.Server.App.Common.Endpoints;
using SB.Server.App.Common;

namespace SB.Server.App.React.Startup
{
  public static class ConfigureApplication
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
      //TODO stop migrating for right now    
      //maybe check if any users exist, if no then apply migration
      //scope.ServiceProvider.GetService<ApplicationDbContext>()?.Database.Migrate();
      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
      //Seed Users
      var userSeeding = new UserSeeding( userManager, app.Configuration );
      userSeeding.SeedDatabase().GetAwaiter().GetResult();
      //Seed all other data
      var dataSeeding = new DataSeeding();
      dataSeeding.SeedDatabase().GetAwaiter().GetResult();
    }
  }
}
