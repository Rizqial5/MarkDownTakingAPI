using Microsoft.EntityFrameworkCore;

namespace MarkDownTaking.API.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<MDData> MDDatas => Set<MDData>();
    }
}