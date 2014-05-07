using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace src.stats {
    [Serializable]
    class ChampionPlayed {

        [JsonProperty("championId")]
        public int championId { get; set; }
    
        [JsonProperty("timesPlayed")]
        public int timesPlayed { get; set; }
    }
}
