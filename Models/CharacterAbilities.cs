using System.Collections.Generic;

namespace LegoDimensions.Models
{
    //associated with characterAbilities.json
    public class CharacterAbilities
    {
        public Character Character { get; set; }
        public List<Abilities> Abilities { get; set; }
    }
}