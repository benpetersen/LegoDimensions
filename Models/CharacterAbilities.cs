using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LegoDimensions.Models
{
    //associated with characterAbilities.json
    public class CharacterAbilities
    {
        public int ID { get; set;}
		public int CharacterID { get; set; }
		public string CharacterName { get; set;}
        public bool IsPurchased { get; set; }
        [NotMapped]
        public List<string> AbilityList { get; set; }
        public string Abilities { 
            get { return string.Join(",", AbilityList); }
            set { AbilityList = value.Split(',').ToList();}
        }
    }
}