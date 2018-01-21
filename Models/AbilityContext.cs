using Microsoft.EntityFrameworkCore;

namespace LegoDimensions.Models
{
    public class AbilityContext: DbContext
    {
        public AbilityContext(DbContextOptions<AbilityContext> options)
            : base(options)
        {
        }
        public DbSet<Ability> Abilities { get; set; }
    }
}