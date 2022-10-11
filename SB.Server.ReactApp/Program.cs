using System.IdentityModel.Tokens.Jwt;
using SB.Server.App.Common;

var builder = WebApplication.CreateBuilder( args );

// Add services to the container.

//builder.Services.AddControllersWithViews();

//Need to set this up if dont want all schemas text crap in JWT claims
//https://github.com/dotnet/aspnetcore/issues/4660
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

builder.Services.RegisterAllServices( builder.Configuration, typeof( Program ).Assembly.FullName );


var app = builder.Build();

// Configure the HTTP request pipeline.
if( !app.Environment.IsDevelopment() )
{
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}


//TODO eventually add in logging to ApplicationInsights
ServerSystem.CreateInstance( app.Services, app.Configuration );

AppSetup.SetupApplication( app );
AppSetup.SeedApplication( app );


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller}/{action=Index}/{id?}" );

app.MapFallbackToFile( "index.html" ); ;

app.Run();
