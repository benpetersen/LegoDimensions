using System.Collections.Generic;

namespace LegoDimensions.Models
{
    public class Pack
    {
        public int ID { get; set; }
        public string PackName { get; set; }
        public string PackType { get; set; }
        public int Wave { get; set; }
        public ICollection<Character> Characters { get; set; }
    }


}