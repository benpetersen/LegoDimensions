using Microsoft.EntityFrameworkCore;

namespace LegoDimensions.Models
{
    public class CharactersWithAbilityContext: DbContext
    {
        public CharactersWithAbilityContext(DbContextOptions<CharactersWithAbilityContext> options)
            : base(options)
        {
        }
        public DbSet<CharactersWithAbility> OwnedCharactersWithAbility { get; set; }
    }
}