using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Newtonsoft.Json;

using RiotSharp;
using src.api;

namespace src {

    class Core {

        private RiotApi riotApi;
        private StaticRiotApi staticApi;
        private ChampionListStatic championList;
        private ItemListStatic itemList;
        private Summoner summoner;
        private Region region;

		public Boolean summonerSet = false;
		public Boolean regionSet = false;

        private String HOME_PATH = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\QueueStats\";
        private String CURRENT_PATH;
        private String CHAMPION_PATH;
        private String ITEM_PATH;

        private static Core instance;

        private Core(Region region, String apiKey, bool isProdApi) {
            riotApi = RiotApi.GetInstance(apiKey, isProdApi);
            staticApi = StaticRiotApi.GetInstance(apiKey);
            API.init(region);
            updateRegion(region);
            load();
        }

        public static Core getInstance(Region region, String apiKey, bool isProdApi) {
            if(instance == null) {
                instance = new Core(region, apiKey, isProdApi);
            }
            return instance;
        }

        private void load() {
            if(File.Exists(CHAMPION_PATH + "list.json")) {
                championList = JsonConvert.DeserializeObject<ChampionListStatic>(File.ReadAllText(CHAMPION_PATH + "list.json"));
            } else {
                championList = staticApi.GetChampions(region, ChampionData.all);
                var json = JsonConvert.SerializeObject(championList);
                if(!Directory.Exists(CHAMPION_PATH)) {
                    Directory.CreateDirectory(CHAMPION_PATH);
                }
                File.WriteAllText(CHAMPION_PATH + "list.json", json);
            }

            if(File.Exists(ITEM_PATH + "list.json")) {
                itemList = JsonConvert.DeserializeObject<ItemListStatic>(File.ReadAllText(ITEM_PATH + "list.json"));
            } else {
                itemList = staticApi.GetItems(region, ItemData.all);
                var json = JsonConvert.SerializeObject(itemList);
                if(!Directory.Exists(ITEM_PATH)) {
                    Directory.CreateDirectory(ITEM_PATH);
                }
                File.WriteAllText(ITEM_PATH + "list.json", json);
            }
        }

        private void updateRegion(Region region) {
            CURRENT_PATH = HOME_PATH + API.getVersion() + @"\";
            CHAMPION_PATH = HOME_PATH + @"champions\";
            ITEM_PATH = HOME_PATH + @"items\";
        }


        public ChampionStatic getChampion(String name) {
            ChampionStatic champion = null;
            if(!Directory.Exists(CHAMPION_PATH)) {
                Directory.CreateDirectory(CHAMPION_PATH);
            }
            name = Regex.Replace(name, @"[^0-9a-zA-Z]+", "").Trim();
            if(File.Exists(CHAMPION_PATH + name + ".json")) {
                String json = File.ReadAllText(CHAMPION_PATH + name + ".json");
                champion = JsonConvert.DeserializeObject<ChampionStatic>(json);
            } else {
                foreach(KeyValuePair<int, String> pair in championList.Keys) {
                    if(Regex.Replace(pair.Value, @"[^0-9a-zA-Z]+", "").Trim() == name) {
                        champion = getChampion(pair.Key);
                        break;
                    }
                }                
            }
            return champion;
        }

        public ChampionStatic getChampion(int id) {
            if(id > 0) {
                return staticApi.GetChampion(region, id, ChampionData.all);
            }
            return null;
        }

        public void setSummoner(Summoner summoner) {
			summonerSet = true;
            this.summoner = summoner;
        }

        public Summoner getSummoner() {
            return summoner;
        }

        public void setRegion(Region region) {
			regionSet = true;
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
