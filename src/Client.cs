using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using src.api;
using RiotSharp;
using src.patch;
using src.summoner;

namespace src {
    class Client {
        private String HOME_PATH;
        public Core core;
        public RiotApi api;
        public SummonerHandler summonerHandler;

        public Client() {
            HOME_PATH = HOME_PATH = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\QueueStats\";

            summonerHandler = SummonerHandler.getInstance();
            core = Core.getInstance();
            api = core.getRiotApi();
        }

        public void updateSummoner(string summonerName, Region region)
        {
            summonerHandler.setSummoner(summonerName, region);
        }

        public void updateSummoner(TrackedSummoner summoner) {
            summonerHandler.setSummoner(summoner);
        }
    }
}
