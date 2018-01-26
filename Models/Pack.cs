using System.Collections.Generic;

namespace LegoDimensions.Models
{
    public class Pack
    {
        public int ID {get; set;}
        public int PackID { get; set; }
        public string PackName { get; set; }
        public string PackType { get; set; }
        public double Wave { get; set; }
        public ICollection<Character> Characters { get; set; }
    }


}