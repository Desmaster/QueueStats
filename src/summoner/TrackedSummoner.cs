using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RiotSharp;

namespace src.summoner {
    [Serializable]
    class TrackedSummoner {
        public TrackedSummoner(String summonerName, Region region) {
            this.summonerName = summonerName;
            this.region = region;
        }

        [JsonProperty("summonerName")]
        public String summonerName {
            get;
            set;
        }

        [JsonProperty("region")]
        public Region region {
            get;
            set;
        }
    }
}
