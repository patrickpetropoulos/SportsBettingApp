using Microsoft.AspNetCore.Identity;
using SB.Server.App.Api.DatabaseSeeding;
namespace SB.Server.App.Api.Startup;

/// <summary>
/// 
/// </summary>
public static class AppSetup
{
	public static void SetupApplication( WebApplication app )
	{
		ServerSystem.Instance?.SetLogger( app.Services.GetRequiredService<ILoggerFactory>() );

		// Configure the HTTP request pipeline.
		if( app.Environment.IsDevelopment() )
		{
			app.UseSwagger();
			app.UseSwaggerUI( opts =>
			{
				opts.SwaggerEndpoint( "/swagger/v2/swagger.json", "v2" );
				opts.SwaggerEndpoint( "/swagger/v1/swagger.json", "v1" );
			} );
		}
		if( app.Environment.IsEnvironment( "Testing" ) )
		{
			Console.WriteLine( "#### PATRICK : I am running Testing in AppSetup.cs" );
		}
		app.UseHttpsRedirection();

		app.UseAuthentication();
		app.UseAuthorization();

		app.MapControllers();
	}

	public static void SeedApplication( WebApplication app )
	{
		using var scope = app.Services.CreateScope();
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