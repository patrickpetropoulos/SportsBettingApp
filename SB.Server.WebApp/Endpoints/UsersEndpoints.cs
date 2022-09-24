using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace SB.Server.WebApp.Endpoints;

public static class UsersEndpoints
{
    
    public static WebApplication MapUsersEndpoints(this WebApplication app)
    {
        app.MapGetCurrentUserInfo();
        app.MapDeleteUser();
        return app;
    }

    public static WebApplication MapGetCurrentUserInfo(this WebApplication app)
    {
        //Better path??
        app.MapGet("/api/users/current",
                async (ClaimsPrincipal claimsPrincipal,
                    UserManager<ApplicationUser> userManager) =>
                {
                    var userEmail = claimsPrincipal.Claims.First( c => c.Type.Equals( "email" ) ).Value;

                    var user = await userManager.Users.FirstOrDefaultAsync( c => c.Email.Equals( userEmail ) );
                    if (user == null)
                        return Results.NotFound();
                    var claims = claimsPrincipal.Claims.Select(c => new ClaimRecord(){Type = c.Type, Value = c.Value}).ToList();
                    var roles = await userManager.GetRolesAsync(user!);

                    var result = new {user, roles, claims};
    
                    return Results.Ok(result);
                });
            // .RequireAuthorization("IsAdmin");
        return app;
    }

    public static WebApplication MapDeleteUser(this WebApplication app)
    {
        app.MapDelete("/api/users/{id}",
                async (ApplicationDbContext context,
                    UserManager<ApplicationUser> userManager,
                    IConfiguration configuration,
                    string id) =>
                {
                    //TODO can either be admin or current user itself, check both
                    Console.WriteLine("hwlt");
                }
            );
        return app;
    }
    
}