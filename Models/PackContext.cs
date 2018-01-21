using Microsoft.EntityFrameworkCore;

namespace LegoDimensions.Models
{
    public class PackContext: DbContext
    {
        public PackContext(DbContextOptions<PackContext> options)
            : base(options)
        {
        }
        public DbSet<Pack> Packs { get; set; }
    }
}