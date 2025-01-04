using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using userManagement.Models;

namespace userManagement.Data
{
	public class AppDbContext : IdentityDbContext<ApplicationUser>
	{
		DbSet<ApplicationUser> Users { get; set; }
	}
}
