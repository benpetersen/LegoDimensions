using Microsoft.EntityFrameworkCore;

namespace LegoDimensions.Models
{
    public class LegoDimensionsContext: DbContext
    {
        public LegoDimensionsContext(DbContextOptions<LegoDimensionsContext> options)
            : base(options)
        {
        }
        public DbSet<Pack> Packs { get; set; }
        public DbSet<Ability> Abilities { get; set; }
        public DbSet<CharacterAbilities> CharacterAbilities { get; set; }
        public DbSet<CharacterAbilities> PurchasedAbilities { get; set; }
    }
}