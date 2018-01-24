using Microsoft.EntityFrameworkCore;

namespace LegoDimensions.Models
{
    public class AbilitiesContext: DbContext
    {
        public CharactersWithAbilityContext(DbContextOptions<AbilitiesContext> options)
            : base(options)
        {
        }
        public DbSet<Ability> Abilities { get; set; }
    }
}