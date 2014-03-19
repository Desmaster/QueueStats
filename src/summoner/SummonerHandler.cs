using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Data;
using Newtonsoft.Json;
using RiotSharp;
using src.summoner;

namespace src {

    class SummonerHandler {
        private String HOME_PATH;
        private List<TrackedSummoner> trackedSummoners;
        private static SummonerHandler instance;
        private TrackedSummoner selectedSummoner = new TrackedSummoner("Krindle", Region.euw);

        private SummonerHandler() {
            HOME_PATH = Core.getInstance().getHomePath();

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

        public static SummonerHandler getInstance() {
            if (instance == null) {
                instance = new SummonerHandler();
            }

            return instance;
        }

        public TrackedSummoner getSummoner() {
            return selectedSummoner;
        }

        private List<TrackedSummoner> GetTrackedSummoners() {
            if (File.ReadAllText(HOME_PATH + "trackedSummoners.json") != "") {
                return JsonConvert.DeserializeObject<List<TrackedSummoner>>(File.ReadAllText(HOME_PATH + "trackedSummoners.json"));
            }

            return new List<TrackedSummoner>();
        }

        private void updateTrackedSummoners() {
            for (int i = 0; i < trackedSummoners.Count(); i++) {
                Log.info(trackedSummoners[i].summonerName);
            }
            File.WriteAllText(HOME_PATH + "trackedSummoners.json", JsonConvert.SerializeObject(trackedSummoners));
        }

        private void trackSummoner(String name, Region region) {
            trackedSummoners.Add(new TrackedSummoner(name, region));
            updateTrackedSummoners();
        }

        private void untrackSummoner(String name, Region region) {
            trackedSummoners.RemoveAll(x => x.summonerName == name && x.region == region);
            updateTrackedSummoners();
        }
    }
}
