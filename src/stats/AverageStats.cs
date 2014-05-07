using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RiotSharp;

namespace src.stats {
    [Serializable]
    class AverageStats {

        [JsonProperty("top5Champions")]
        public List<ChampionPlayed> champions { get; set; }

        [JsonProperty("avgKills")]
        public double averageKills { get; set; }

        [JsonProperty("avgDeaths")]
        public double averageDeaths { get; set; }
        
        [JsonProperty("avgAssists")]
        public double averageAssists { get; set; }
        
        [JsonProperty("totalKills")]
        public int totalKills { get; set; }

        [JsonProperty("totalDeaths")]
        public int totalDeaths { get; set; }

        [JsonProperty("totalAssists")]
        public int totalAssists { get; set; }

        [JsonProperty("totalDoubleKills")]
        public int totalDoubleKills { get; set; }

        [JsonProperty("totalTripleKills")]
        public int totalTripleKills { get; set; }

        [JsonProperty("totalQuadraKills")]
        public int totalQuadraKills { get; set; }

        [JsonProperty("totalPentaKills")]
        public int totalPentaKills { get; set; }

        [JsonProperty("totalMagicDamageDealt")]
        public long totalMagicDamageDealt { get; set; }
        
        [JsonProperty("totalPhysicalDamageDealt")]
        public long totalPhysicalDamageDealt { get; set; }

        [JsonProperty("totalTrueDamageDealt")]
        public long totalTrueDamageDealt { get; set; }

        [JsonProperty("totalDamageDealt")]
        public long totalDamageDealt { get; set; }

        [JsonProperty("totalMagicDamageTaken")]
        public long totalMagicDamageTaken { get; set; }

        [JsonProperty("totalPhysicalDamageTaken")]
        public long totalPhysicalDamageTaken { get; set; }

        [JsonProperty("totalTrueDamageTaken")]
        public long totalTrueDamageTaken { get; set; }

        [JsonProperty("totalDamageTaken")]
        public long totalDamageTaken { get; set; }
    }
}
