using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LegoDimensions.Models
{
    public class Pack
    {
        public int ID {get; set;}
        public int PackID { get; set; }
        public string PackName { get; set; }
        public string PackType { get; set; }
        public double Wave { get; set; }
        
        [NotMapped]
        public ICollection<int> CharacterList { get; set; }
        public string CharacterIDs 
        { 
            get { return string.Join(",", CharacterList); }
            set { CharacterList = value.Split(',').Select(Int32.Parse).ToList(); }
        }
    }


}