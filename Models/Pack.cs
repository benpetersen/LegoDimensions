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
        public bool IsPurchased { get; set; }
    }


}



/*
"packName" : "Chase McCain",
		"packType" : "Fun",
		"packNumber": 71266,
		"waveNumber": 8,
		"characters" : [{
			"name": "Chase McCain",
			"abilities": ["Acrobat", "Detective Scanner", "Drill", "Grapple", "Silver Lego Blowup","Tracking", "Uniform Changing", "Water Spray"]
		}]
 */