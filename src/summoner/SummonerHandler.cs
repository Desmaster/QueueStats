using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations;
using System.Windows.Data;
using Newtonsoft.Json;
using RiotSharp;
using src.summoner;

namespace src {

    class SummonerHandler {
        private String HOME_PATH;
        private List<TrackedSummoner> trackedSummoners;
        private static SummonerHandler instance;

        private SummonerHandler(String HOME_PATH) {
            this.HOME_PATH = HOME_PATH;

            if (!Directory.Exists(HOME_PATH + @"summoners\")) {
                Directory.CreateDirectory(HOME_PATH + @"summoners\");
            }

            if (!File.Exists(HOME_PATH + "trackedSummoners.json")) {
                File.Create(HOME_PATH + "trackedSummoners.json");
            }

            trackedSummoners = GetTrackedSummoners();
            trackSummoner("Chowtrinity", Region.euw);
            trackSummoner("DpsKenpachi", Region.euw);
            trackSummoner("Dyrus", Region.na);

            untrackSummoner("Dyrus", Region.na);
        }

        public static SummonerHandler getInstance(String HOME_PATH) {
            if (instance == null) {
                instance = new SummonerHandler(HOME_PATH);
            }

            return instance;
        }

        public object getSummoner()
        {
            return new {summonerName = "Krindle", region = Region.euw};
        }

        private List<TrackedSummoner> GetTrackedSummoners() {
            return JsonConvert.DeserializeObject<List<TrackedSummoner>>(File.ReadAllText(HOME_PATH + "trackedSummoners.json"));
        }

        private void SetTrackedSummoners() {
            File.WriteAllText(HOME_PATH + "trackedSummoners.json", JsonConvert.SerializeObject(trackedSummoners));
        }

        private void trackSummoner(String name, Region region) {
            trackedSummoners.Add(new TrackedSummoner(name, region));
            SetTrackedSummoners();
        }

        private void untrackSummoner(String name, Region region) {
            trackedSummoners.RemoveAll(x => x.summonerName == name && x.region == region);
        }
    }
}
