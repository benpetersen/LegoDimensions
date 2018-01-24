using Microsoft.EntityFrameworkCore;

namespace LegoDimensions.Models
{
    public class CharactersWithAbilityContext: DbContext
    {
        public CharactersWithAbilityContext(DbContextOptions<CharacterAbilitiesContext> options)
            : base(options)
        {
        }
        public DbSet<CharacterAbilities> CharacterAbilities { get; set; }
    }
}