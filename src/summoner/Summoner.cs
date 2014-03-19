using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp;

namespace src.summoner {
    class Summoner
    {
        private String name;
        private Region region;

        public Summoner(String summonerName, Region region) {
            this.name = summonerName;
            this.region = region;
        }

        public String getName()
        {
            return this.name;
        }

        public Region getRegion()
        {
            return this.region;
        }
    }
}
