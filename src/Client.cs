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
        private String HOME_PATH;
        public Core core;
        public RiotApi api;
        public SummonerHandler summonerHandler;
        public MatchHandler matchHandler;

        public Client() {
            HOME_PATH = HOME_PATH = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\QueueStats\";

            summonerHandler = SummonerHandler.getInstance();
            core = Core.getInstance();
            api = core.getRiotApi();
            matchHandler = MatchHandler.getInstance();
        }

        public async Task<Boolean> updateSummoner(string summonerName, Region region)
        {
            return await summonerHandler.setSummoner(summonerName, region);
        }

        public async Task<Boolean> updateSummoner(TrackedSummoner summoner) {
            return await updateSummoner(summoner.Name, summoner.Region);
        }
    }
}
