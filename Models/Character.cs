using System.Collections.Generic;

namespace LegoDimensions.Models
{
    public class Character
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Ability> Abilities { get; set;}
    }
}