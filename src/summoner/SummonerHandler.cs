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
using src.api;
using src.summoner;

namespace src {

    class SummonerHandler {
        private String HOME_PATH;
        private RiotApi api;
        private Core core;

        public List<TrackedSummoner> trackedSummoners;
        private static SummonerHandler instance;

        private Summoner selectedSummoner;

        private List<SummonerListener> summonerListeners = new List<SummonerListener>();

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

        public void register(SummonerListener listener) {
            summonerListeners.Add(listener);
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

        public void setSummoner(String name, Region region) {
            loadSummoner(name, region).ContinueWith(updateSummoner, TaskContinuationOptions.ExecuteSynchronously);
        }

        public void setSummoner(TrackedSummoner summoner) {
            loadSummoner(summoner).ContinueWith(updateSummoner, TaskContinuationOptions.ExecuteSynchronously);
        }

        private async Task loadSummoner(String name, Region region) {
            selectedSummoner = await api.GetSummonerAsync(region, name);
        }

        private async Task loadSummoner(TrackedSummoner summoner) {
            selectedSummoner = await api.GetSummonerAsync(summoner.Region, (int) summoner.Id);
        }

        private void updateSummoner(Task task) {
            for (int i = 0; i < summonerListeners.Count; i++) {
                summonerListeners[i].summonerUpdated(selectedSummoner);
            }
        }

        private List<TrackedSummoner> getTrackedSummoners() {
            if (File.ReadAllText(HOME_PATH + "trackedSummoners.json") != "") {
                return JsonConvert.DeserializeObject<List<TrackedSummoner>>(File.ReadAllText(HOME_PATH + "trackedSummoners.json"));
            }

            return new List<TrackedSummoner>();
        }

        private void updateTrackedSummoners() {
            for (int i = 0; i < trackedSummoners.Count(); i++) {
                Log.info(trackedSummoners[i].Name);
            }
            File.WriteAllText(HOME_PATH + "trackedSummoners.json", JsonConvert.SerializeObject(trackedSummoners));
        }

        public void trackSummoner(Summoner summoner) {
            if (!isTracked(summoner)) {
                trackedSummoners.Add(new TrackedSummoner(summoner.Id, summoner.Name, summoner.Region));
                updateTrackedSummoners();
            }
        }

        public void untrackSummoner(Summoner summoner) {
            trackedSummoners.RemoveAll(x => x.Id == summoner.Id && x.Region == summoner.Region);
            updateTrackedSummoners();

        }

        public bool isTracked(Summoner summoner) {
            return trackedSummoners.Any(x => x.Id == summoner.Id && x.Region == summoner.Region);
        }
    }
}
