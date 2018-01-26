using System.Collections.Generic;

namespace LegoDimensions.Models
{
    //associated with characterAbilities.json
    public class CharacterAbilities
    {
        public int ID { get; set;}
        public Character Character { get; set; }
        public List<Ability> Abilities { get; set; }
    }
}