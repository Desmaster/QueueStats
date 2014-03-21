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
        private RiotApi api;
        private Core core;


        public List<TrackedSummoner> trackedSummoners;
        private static SummonerHandler instance;
        private Summoner selectedSummoner;

        private SummonerHandler() {
            core = Core.getInstance();
            api = core.getRiotApi();

            HOME_PATH = core.getHomePath();


            if (!Directory.Exists(HOME_PATH + @"summoners\")) {
                Directory.CreateDirectory(HOME_PATH + @"summoners\");
            }

            if (!File.Exists(HOME_PATH + "trackedSummoners.json")) {
                File.Create(HOME_PATH + "trackedSummoners.json");
            }

            trackedSummoners = getTrackedSummoners();
        }

        public static SummonerHandler getInstance() {
            if (instance == null) {
                instance = new SummonerHandler();
            }

            return instance;
        }

        public Summoner getSummoner() {
            return selectedSummoner;
        }

        public void setSummoner(String name, String region) {
            selectedSummoner = api.GetSummoner(Util.resolveRegion(region), name);
        }

        private List<TrackedSummoner> getTrackedSummoners() {
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

        public void trackSummoner(String name, Region region) {
            if (!isTracked(name, region)) {
                trackedSummoners.Add(new TrackedSummoner(name, region));
                updateTrackedSummoners();
            }
        }

        public void untrackSummoner(String name, Region region) {
            trackedSummoners.RemoveAll(x => x.summonerName == name && x.region == region);
            updateTrackedSummoners();

        }

        public bool isTracked(String name, Region region) {
            return trackedSummoners.Any(x => x.summonerName == name && x.region == region);
        }
    }
}
