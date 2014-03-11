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
using System.Configuration;

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
            loadLists();
        }

        public static Core getInstance(Region region, String apiKey, bool isProdApi) {
            if(instance == null) {
                instance = new Core(region, apiKey, isProdApi);
            }
            return instance;
        }

        private void loadLists() {
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
            API.init(region);
            CURRENT_PATH = HOME_PATH + API.getVersion() + @"\";
            CHAMPION_PATH = CURRENT_PATH + @"champions\";
            ITEM_PATH = CURRENT_PATH + @"items\";
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
                foreach(KeyValuePair<String, ChampionStatic> pair in championList.Champions) {
                    if(Regex.Replace(pair.Value.Name, @"[^0-9a-zA-Z]+", "").Trim() == name) {
                        champion = pair.Value;
                        break;
                    }
                }
            }
            return champion;
        }

        public ChampionStatic getChampion(int id) {
            foreach(KeyValuePair<String, ChampionStatic> pair in championList.Champions) {
                if(pair.Value.Key == id) {
                    return pair.Value;
                }
            }
            return staticApi.GetChampion(region, id, ChampionData.all);
        }

        public ItemStatic getItem(String name) {
            ItemStatic item = null;
            name = Regex.Replace(name, @"[^0-9a-zA-Z]+", "").Trim();
            if(File.Exists(ITEM_PATH + name + ".json")) {
                item = JsonConvert.DeserializeObject<ItemStatic>(File.ReadAllText(ITEM_PATH + name + ".json"));
            } else {
                foreach(KeyValuePair<int, ItemStatic> pair in itemList.Items) {
                    if(Regex.Replace(pair.Value.Name, @"[^0-9a-zA-Z]+", "").Trim() == name) {
                        item = pair.Value;
                        break;
                    }
                }
            }
            return item;
        }

        public ItemStatic getItem(int id) {
            return staticApi.GetItem(region, id, ItemData.all);
        }

        public static bool getProperty(String name) {
            return (bool)Properties.Settings.Default.PropertyValues[name].PropertyValue;
        }

        public static int getProperty(String name) {
            return (int)Properties.Settings.Default.PropertyValues[name].PropertyValue;
        }

        public static String getProperty(String name) {
            return (String)Properties.Settings.Default.PropertyValues[name].PropertyValue;
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
