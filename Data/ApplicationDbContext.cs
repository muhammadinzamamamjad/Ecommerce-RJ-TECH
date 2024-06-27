using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RJTECH_Authentication_.Models;

namespace RJTECH_Authentication_.Data
{
    public class ApplicationDbContext : IdentityDbContext<MyUsers>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<RJTECH_Authentication_.Models.Product> Product { get; set; } = default!;
    }
}
