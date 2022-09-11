using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        builder.Configuration.GetConnectionString( "Users" ),
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
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes( builder.Configuration["JWT"] ) ),
          ClockSkew = TimeSpan.Zero
        };
      } );

    builder.Services.AddAuthorization( options =>
    {
      options.AddPolicy( "IsAdmin", policy => policy.RequireClaim( "role", "Admin" ) );
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