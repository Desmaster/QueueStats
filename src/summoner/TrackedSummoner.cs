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
        public TrackedSummoner(long id, String summonerName, Region region) {
            Id = id;
            Name = summonerName;
            Region = region;
        }

        [JsonProperty("Id")]
        public long Id {
            get;
            set;
        }

        [JsonProperty("Name")]
        public String Name {
            get;
            set;
        }

        [JsonProperty("region")]
        public Region Region {
            get;
            set;
        }

        public override string ToString()
        {
            return Name + " - " + Region.ToString();
        }
    }
}
