using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SB.Server.Root.Casinos;

namespace SB.Server.WebApp.Endpoints;

public static class CasinosEndpoints
{
    public static WebApplication MapCasinosEndpoints(this WebApplication app)
    {
        app.MapGetAllCasinos();
        return app;
    }

    public static WebApplication MapGetAllCasinos(this WebApplication app)
    {
        //Better path??
        app.MapGet("/api/casinos/",
            async (ClaimsPrincipal claimsPrincipal,
                UserManager<ApplicationUser> userManager) =>
            {
                var casinoManager = ServerSystem.Instance.Get<ICasinoManager>( "CasinoManager" );

                var currentCasinos = await casinoManager.GetAllCasinos();
    
                return Results.Ok(new {currentCasinos});
            });
        // .RequireAuthorization("IsAdmin");
        return app;
    }
}