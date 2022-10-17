using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SB.Server.App.Common;

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
//TODO remove ApplicationRole
public class ApplicationRole : IdentityRole<Guid>
{
	public int Priority { get; set; }
}
