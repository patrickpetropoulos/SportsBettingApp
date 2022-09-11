using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SB.Server.WebApp.Controllers;

[Route( "api/[controller]" )]
[ApiController]
public class UsersController : ControllerBase
{
  private readonly ApplicationDbContext _context;
  private readonly UserManager<ApplicationUser> _userManager;
  public UsersController( ApplicationDbContext context, UserManager<ApplicationUser> userManager )
  {
    _context = context;
    _userManager = userManager;
  }
  // GET: api/<UsersController>
  [HttpGet]
  public IResult Get()
  {
    var users = _context.Users.ToList();

    return Results.Ok(users);
  }

  // GET api/<UsersController>/5
  [HttpGet( "{id}" )]
  public string Get( int id )
  {
    return "value";
  }

  // POST api/<UsersController>
  [HttpPost]
  public void Post( [FromBody] string value )
  {
  }

  // PUT api/<UsersController>/5
  [HttpPut( "{id}" )]
  public void Put( int id, [FromBody] string value )
  {
  }

  // DELETE api/<UsersController>/5
  [HttpDelete( "{id}" )]
  public void Delete( int id )
  {
  }
}
