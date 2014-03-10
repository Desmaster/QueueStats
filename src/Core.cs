using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RiotSharp;
using src.api;

namespace src {

    class Core {

        private RiotApi riotApi;
        private StaticRiotApi staticApi;
        private Summoner summoner;
        private Region region;

        private static Core instance;

        private Core(String apiKey, bool isProdApi) {
            riotApi = RiotApi.GetInstance(apiKey, isProdApi);
            staticApi = StaticRiotApi.GetInstance(apiKey);
        }

        public static Core getInstance(String apiKey, bool isProdApi) {
            if(instance == null) {
                instance = new Core(apiKey, isProdApi);
            }
            return instance;
        }

        public ChampionStatic getChampion(String name) {
            return null;
        }

        public void setSummoner(Summoner summoner) {
            this.summoner = summoner;
        }

        public Summoner getSummoner() {
            return summoner;
        }

        public void setRegion(Region region) {
            this.region = region;
        }

        public Region getRegion() {
            return region;
        }

        public RiotApi getRiotApi() {
            return riotApi;
        }

        public StaticRiotApi getStaticRiotApi() {
            return staticApi;
        }

    }

}
