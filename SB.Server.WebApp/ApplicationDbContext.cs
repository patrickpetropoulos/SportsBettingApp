using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SB.Server.WebApp;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
  public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options )
      : base( options )
  {
  }
}

public class ApplicationUser : IdentityUser<Guid>
{
  public int SuperPrivateField { get; set; }
}

public class ApplicationRole : IdentityRole<Guid>
{
  public int Priority { get; set; }

}
