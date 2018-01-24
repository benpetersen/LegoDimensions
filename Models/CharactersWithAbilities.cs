using System.Collections.Generic;

namespace LegoDimensions.Models
{
    //associated with characterAbilities.json
    public class CharacterAbilities
    {
        public Ability Abilities { get; set; }
        public List<Character> Characters { get; set; }
    }
}