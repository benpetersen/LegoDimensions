using System.Collections.Generic;

namespace LegoDimensions.Models
{
    public class PurchasedAbilities
    {
        public int ID { get; set; }
        public Ability Ability { get; set; }
        public List<Character> Characters { get; set; }

        public PurchasedAbilities(Ability ability, Character characters)
        {
            //Initilization for newly owned packs
            Ability = ability;
            Characters.Add(characters);
        }
    }

}