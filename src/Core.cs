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

        private String HOME_PATH = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\QueueStats\";
        private String CURRENT_PATH;

        private static Core instance;

        private Core(Region region, String apiKey, bool isProdApi) {
            riotApi = RiotApi.GetInstance(apiKey, isProdApi);
            staticApi = StaticRiotApi.GetInstance(apiKey);
            updateRegion(region);
            loadLists();
            Log.info("Initialized Core");
        }

        public static Core getInstance(Region region, String apiKey, bool isProdApi) {
            if(instance == null) {
                instance = new Core(region, apiKey, isProdApi);
            }
            return instance;
        }

        private void loadLists() {
            if(File.Exists(CURRENT_PATH + "championList.json")) {
                championList = JsonConvert.DeserializeObject<ChampionListStatic>(File.ReadAllText(CURRENT_PATH + "championList.json"));
            } else {
                championList = staticApi.GetChampions(region, ChampionData.all);
                var json = JsonConvert.SerializeObject(championList);
                File.WriteAllText(CURRENT_PATH + "championList.json", json);
            }

            if(File.Exists(CURRENT_PATH + "itemList.json")) {
                itemList = JsonConvert.DeserializeObject<ItemListStatic>(File.ReadAllText(CURRENT_PATH + "itemList.json"));
            } else {
                itemList = staticApi.GetItems(region, ItemData.all);
                var json = JsonConvert.SerializeObject(itemList);
                File.WriteAllText(CURRENT_PATH + "itemList.json", json);
            }
        }

        private void updateRegion(Region region) {
            API.init(region);

            if(!Directory.Exists(HOME_PATH))
                Directory.CreateDirectory(HOME_PATH);

            CURRENT_PATH = HOME_PATH + API.getVersion() + @"\";

            if(!Directory.Exists(CURRENT_PATH))
                Directory.CreateDirectory(CURRENT_PATH);
        }

        public ChampionStatic getChampion(String name) {
            foreach(KeyValuePair<String, ChampionStatic> pair in championList.Champions) {
                if(pair.Value.Name == name) {
                    return pair.Value;
                }
            }
            return null;
        }

        public ChampionStatic getChampion(int id) {
            foreach(KeyValuePair<String, ChampionStatic> pair in championList.Champions) {
                if(pair.Value.Key == id) {
                    return pair.Value;
                }
            }
            return null;
        }

        public ItemStatic getItem(String name) {
            foreach(KeyValuePair<int, ItemStatic> pair in itemList.Items) {
                if(pair.Value.Name == name) {
                    return pair.Value;
                }
            }
            return null;
        }

        public ItemStatic getItem(int id) {
            foreach(KeyValuePair<int, ItemStatic> pair in itemList.Items) {
                if(pair.Key == id) {
                    return pair.Value;
                }
            }
            return null;
        }

        public static bool getPropertyBool(String name) {
            return (bool)Properties.Settings.Default.PropertyValues[name].PropertyValue;
        }

        public static int getPropertyInt(String name) {
            return (int)Properties.Settings.Default.PropertyValues[name].PropertyValue;
        }

        public static String getPropertyString(String name) {
            return (String)Properties.Settings.Default.PropertyValues[name].PropertyValue;
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
