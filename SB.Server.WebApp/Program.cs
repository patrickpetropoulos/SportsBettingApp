using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace SB.Server.WebApp;

public class Program{
  public static void Main( string[] args )
  {
    var builder = WebApplication.CreateBuilder( args );

    //Need to set this up if dont want all schemas text crap in JWT claims
    //https://github.com/dotnet/aspnetcore/issues/4660
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();


    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddCors( options => options.AddPolicy( "AllowAll", p => p.AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader() ) );


    builder.Services.AddDbContext<ApplicationDbContext>( options =>
      options.UseSqlServer(
        builder.Configuration.GetConnectionString( "Default" ),
        b => b.MigrationsAssembly( typeof( ApplicationDbContext ).Assembly.FullName ) ) );

    builder.Services.AddIdentityCore<ApplicationUser>()
      .AddRoles<ApplicationRole>()
      .AddEntityFrameworkStores<ApplicationDbContext>()
      .AddDefaultTokenProviders();

    builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
      .AddJwtBearer( options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = false, 
          ValidateAudience = false,
          ValidateLifetime = true,
          //ValidIssuer = builder.Configuration.GetValue<string>("KEY"),
          //ValidAudience = builder.Configuration.GetValue<string>("KEY"),
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes( builder.Configuration["JWT"] ) ),
          ClockSkew = TimeSpan.Zero
        };
      } );

    builder.Services.AddAuthorization( options =>
    {
      //Move all constants to constant file

      //TODO can require Role directly, don't need to use claims to handle rolls
      //Need to test this out
      //Can have separate policies for claims, like if people had separate data in their claims
      //Like employee ID, or TITLE, or something else that we save
      //Also can have just a claim or role, not necessarily a specific claim or role
      options.AddPolicy( "IsAdmin", policy => policy.RequireClaim( "role", "Admin" ) );

      //TODO move all this out
      //This locks down all endpoints unless authenticated
      options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    } );

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if( app.Environment.IsDevelopment() )
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }
    if( app.Environment.IsEnvironment( "Testing" ) )
    {

    }
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    using( var scope = app.Services.CreateScope() )
    {
      scope.ServiceProvider.GetService<ApplicationDbContext>()?.Database.Migrate();
      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
      var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

      DbSeeding.CreateRoles( userManager, roleManager, app.Configuration ).GetAwaiter().GetResult();
      DbSeeding.CreatePowerUser( userManager, roleManager, app.Configuration ).GetAwaiter().GetResult();
    }
    app.Run();
  }
}