using System.Collections.Generic;

namespace LegoDimensions.Models
{
    public class PurchasedAbilities
    {
        public Ability Ability { get; set; }
        public List<Character> Characters { get; set; }
    }
}