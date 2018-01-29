using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LegoDimensions.Models
{
    public class PurchasedAbilities
    {
        public int ID { get; set; }
        public int CharacterID { get; set; }
        public string AbilityName { get; set; }
        [NotMapped]
        public List<string> CharacterList { get; set; }
        public string Characters { 
            get { return string.Join(",", CharacterList); }
            set { CharacterList = value.Split(',').ToList();}
        }
    }

}