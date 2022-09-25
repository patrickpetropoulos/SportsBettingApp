using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SB.Server.Common.Managers;
using SB.Server.Root.Casinos;

namespace SB.Server.WebApp.Endpoints;

public static class CasinosEndpoints
{
    public static WebApplication MapCasinosEndpoints(this WebApplication app)
    {
        //TODO how to manage swagger to have them separated out better
        
        app.MapGetAllCasinos();
        app.MapInsertCasino();
        app.MapUpdateCasino();
        app.MapDeleteCasino();
        return app;
    }

    public static WebApplication MapGetAllCasinos(this WebApplication app)
    {
        //Better path??
        app.MapGet("/api/casinos/",
            async (ClaimsPrincipal claimsPrincipal,
                UserManager<ApplicationUser> userManager) =>
            {
                var casinoManager = ServerSystem.Instance.Get<ICasinoManager>( ManagerNames.CasinoManager );

                var casinos = await casinoManager.GetAllCasinos();
    
                return Results.Ok(new {casinos});
            });
        // .RequireAuthorization("IsAdmin");
        return app;
    }
    
    public static WebApplication MapInsertCasino(this WebApplication app)
    {
        app.MapPost("/api/casinos/",
            async (ClaimsPrincipal claimsPrincipal,
                UserManager<ApplicationUser> userManager,
                [FromBody] Casino casino) =>
            {
                //var casino = JsonConvert.DeserializeObject<Casino>(jObject?["casino"].ToString());
                
                var casinoManager = ServerSystem.Instance.Get<ICasinoManager>( ManagerNames.CasinoManager );

                await casinoManager.UpsertCasino(casino);
    
                return Results.Ok(new {casino});
            })
        .RequireAuthorization("IsAdmin");
        return app;
    }
    public static WebApplication MapUpdateCasino(this WebApplication app)
    {
        app.MapPut("/api/casinos/{id}",
                async (ClaimsPrincipal claimsPrincipal,
                    UserManager<ApplicationUser> userManager,
                    int id,
                    [FromBody] Casino casino) =>
                {
                    //var casino = JsonConvert.DeserializeObject<Casino>(jObject?["casino"].ToString());

                    if (casino.Id != id)
                    {
                        //TODO put these errors somewhere and ensure we get them
                        return Results.BadRequest("Id in url doesn't match Id in object");
                    }
                    
                    var casinoManager = ServerSystem.Instance.Get<ICasinoManager>( ManagerNames.CasinoManager );

                    await casinoManager.UpsertCasino(casino);
    
                    return Results.Ok(new {casino});
                })
            .RequireAuthorization("IsAdmin");
        return app;
    }
    
    public static WebApplication MapDeleteCasino(this WebApplication app)
    {
        //Better path??
        app.MapDelete("/api/casinos/{id}",
                async (ClaimsPrincipal claimsPrincipal,
                    UserManager<ApplicationUser> userManager,
                    int id) =>
                {
                    var casinoManager = ServerSystem.Instance.Get<ICasinoManager>( ManagerNames.CasinoManager );

                    var successful = await casinoManager.DeleteCasino(id);
                    
                    return successful ? 
                        Results.Ok("Casino with id " + id + "deleted") : 
                        Results.BadRequest("Something went wrong");
                })
            .RequireAuthorization("IsAdmin");
        return app;
    }
}