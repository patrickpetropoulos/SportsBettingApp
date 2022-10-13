using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using static SB.Server.App.Common.AuthorizationConstants;

namespace SB.Server.App.Common.Startup;

public static class ServicesSetup
{
  public static IServiceCollection RegisterAllServices( this IServiceCollection services, IConfiguration configuration
    , string assemblyName )
  {
    return services
      .RegisterSwagger()
      .RegisterApiVersioning()
      .RegisterCors()
      .RegisterIdentity( configuration, assemblyName )
      .RegisterAuthentication( configuration )
      .RegisterAuthorization()
      .RegisterLogging( configuration );
  }

  private static IServiceCollection RegisterSwagger( this IServiceCollection services )
  {
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    //https://stackoverflow.com/a/62864495
    services.AddSwaggerGen( c =>
    {
      //can have another for v2 later on
      c.SwaggerDoc( "v1", new OpenApiInfo
      {
        Version = "v1",
        Title = "SportsBetting API",
        Description = "SportsBetting API Swagger Surface",
        Contact = new OpenApiContact
        {
          Name = "Patrick Petropoulos",
          Email = "patrickperopoulos@gmail.com",
          Url = new Uri( "https://www.linkedin.com/in/patrickpetropoulos/" )
        },
        License = new OpenApiLicense
        {
          Name = "MIT",
          Url = new Uri( "https://github.com/ignaciojvig/ChatAPI/blob/master/LICENSE" )
        }

      } );
      c.SwaggerDoc( "v2", new OpenApiInfo
      {
        Version = "v2",
        Title = "v2 SportsBetting API",
        Description = "SportsBetting API Swagger Surface",
        Contact = new OpenApiContact
        {
          Name = "Patrick Petropoulos",
          Email = "patrickperopoulos@gmail.com",
          Url = new Uri( "https://www.linkedin.com/in/patrickpetropoulos/" )
        },
        License = new OpenApiLicense
        {
          Name = "MIT",
          Url = new Uri( "https://github.com/ignaciojvig/ChatAPI/blob/master/LICENSE" )
        }

      } );

      c.AddSecurityDefinition( "Bearer", new OpenApiSecurityScheme
      {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                              Enter 'Bearer' [space] and then your token in the text input below.
                              \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
      } );

      c.AddSecurityRequirement( new OpenApiSecurityRequirement()
              {
                {
                  new OpenApiSecurityScheme
                  {
                    Reference = new OpenApiReference
                    {
                      Type = ReferenceType.SecurityScheme,
                      Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,

                  },
                  new List<string>()
                }
              } );

    } );

    //TODO 
    //API VERSIONING
    //Doesn't seem to work with react, will need to try again at some point
    //https://www.infoworld.com/article/3671870/how-to-version-minimal-apis-in-aspnet-core-6.html
    //Tim corey course on it

    return services;
  }

  private static IServiceCollection RegisterApiVersioning( this IServiceCollection services )
  {
    services.AddApiVersioning( o =>
    {
      o.AssumeDefaultVersionWhenUnspecified = true;
      o.DefaultApiVersion = new( 1, 0 );
      o.ReportApiVersions = true;

    } );

    services.AddVersionedApiExplorer(
    options =>
    {
      options.GroupNameFormat = "'v'VVV";
      options.SubstituteApiVersionInUrl = true;
    } );

    return services;
  }

  private static IServiceCollection RegisterCors( this IServiceCollection services )
  {
    //TODO determine correct CORS policy, can we check if in development, or when we have react app manually put url
    services.AddCors( options => options.AddPolicy( "AllowAll", p => p.AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader() ) );

    return services;
  }

  private static IServiceCollection RegisterIdentity( this IServiceCollection services, IConfiguration configuration, string assemblyName )
  {
    services.AddDbContext<ApplicationDbContext>( options =>
      options.UseSqlServer(
        configuration.GetConnectionString( "Default" ) ?? throw new InvalidOperationException(),
        b => b.MigrationsAssembly( assemblyName ) ) );

    services.AddIdentityCore<ApplicationUser>()
      .AddRoles<ApplicationRole>()
      .AddEntityFrameworkStores<ApplicationDbContext>()
      .AddDefaultTokenProviders();

    return services;
  }

  private static IServiceCollection RegisterAuthentication( this IServiceCollection services, IConfiguration configuration )
  {
    services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
      .AddJwtBearer( options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true, //Can set to true if we have an issuer/audience, if we eventually have multiple
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidIssuer = configuration.GetValue<string>( "JWT:issuer" ),
          ValidAudience = configuration.GetValue<string>( "JWT:audience" ),
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes( configuration.GetValue<string>( "JWT:signingKey" ) ) ),
          ClockSkew = TimeSpan.Zero
        };
      } );


    return services;
  }

  private static IServiceCollection RegisterAuthorization( this IServiceCollection services )
  {
    services.AddAuthorization( options =>
    {
      //Move all constants to constant file

      //TODO can require Role directly, don't need to use claims to handle rolls
      //Need to test this out
      //Can have separate policies for claims, like if people had separate data in their claims
      //Like employee ID, or TITLE, or something else that we save
      //Also can have just a claim or role, not necessarily a specific claim or role
      options.AddPolicy( Claim_Policy_IsAdmin, policy =>
        policy.RequireClaim( Claim_AccessLevel_Type, Claim_AccessLevel_Admin ) );

      //TODO move all this out
      //This locks down all endpoints unless authenticated
      options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    } );
    return services;
  }

  private static IServiceCollection RegisterLogging( this IServiceCollection services, IConfiguration configuration )
  {
    services.AddApplicationInsightsTelemetry( configuration );

    return services;
  }
}