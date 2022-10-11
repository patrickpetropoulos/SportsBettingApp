using SB.Server.App.Common;
using SB.Server.Common.Managers;
using SB.Server.ReactApp;
using SB.Server.ReactApp.Controllers;
using SB.Server.Root.Casinos;

var builder = WebApplication.CreateBuilder( args );

// Add services to the container.

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if( !app.Environment.IsDevelopment() )
{
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}" );



app.MapGet( "/api/casinos/",
        async () =>
        {
          return Enumerable.Range( 1, 100 ).Select( index => new WeatherForecast
          {
            Date = DateTime.Now.AddDays( index ),
            TemperatureC = Random.Shared.Next( -20, 55 ),
            Summary = WeatherForecastController.Summaries[Random.Shared.Next( WeatherForecastController.Summaries.Length )]
          } )
          .ToArray();
        } );



app.MapFallbackToFile( "index.html" ); ;

app.Run();