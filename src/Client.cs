using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using src.api;
using RiotSharp;
using src.matches;
using src.patch;
using src.summoner;

namespace src {
    class Client {

        public SummonerHandler summonerHandler;

        public Client() {

            summonerHandler = SummonerHandler.getInstance();
        }

        public async Task<Boolean> updateSummoner(string summonerName, Region region) {
            return await summonerHandler.setSummoner(summonerName, region);
        }
    }
}
