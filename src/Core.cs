using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

using RiotSharp;
using src.api;
using src.patch;

namespace src {

    class Core {

        private readonly RiotApi riotApi;
        private readonly StaticRiotApi staticApi;
        private ChampionListStatic championList;
        private ItemListStatic itemList;
        private SummonerSpellListStatic spellList;
        private Summoner summoner;
        private Region region;

        public Boolean summonerSet = false;
        public Boolean regionSet = false;

        private readonly String HOME_PATH = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\QueueStats\";
        private String CURRENT_PATH;

        private static Core instance;

        private Core(Region region, String apiKey, bool isProdApi) {
            riotApi = RiotApi.GetInstance(apiKey, isProdApi);
            staticApi = StaticRiotApi.GetInstance(apiKey);
            updateRegion(region);
            loadLists();
            Log.info("Initialized Core");
        }

        public static Core getInstance() {
            if (instance == null) {
                instance = new Core(Region.na, Settings.getProperty("api_key"), true);
            }
            return instance;
        }

        public String getHomePath() {
            return HOME_PATH;
        }

        public String getCurrentPath() {
            return CURRENT_PATH;
        }

        public String getAssetsPath() {
            return CURRENT_PATH + @"img\";
        }

        private void loadLists() {
            if (File.Exists(CURRENT_PATH + "championList.json")) {
                championList = JsonConvert.DeserializeObject<ChampionListStatic>(File.ReadAllText(CURRENT_PATH + "championList.json"));
            } else {
                championList = staticApi.GetChampions(region, ChampionData.all);
                var json = JsonConvert.SerializeObject(championList);
                File.WriteAllText(CURRENT_PATH + "championList.json", json);
            }

            if (File.Exists(CURRENT_PATH + "itemList.json")) {
                itemList = JsonConvert.DeserializeObject<ItemListStatic>(File.ReadAllText(CURRENT_PATH + "itemList.json"));
            } else {
                itemList = staticApi.GetItems(region, ItemData.all);
                var json = JsonConvert.SerializeObject(itemList);
                File.WriteAllText(CURRENT_PATH + "itemList.json", json);
            }

            if (File.Exists(CURRENT_PATH + "spellList.json")) {
                spellList = JsonConvert.DeserializeObject<SummonerSpellListStatic>(File.ReadAllText(CURRENT_PATH + "spellList.json"));
            } else {
                spellList = staticApi.GetSummonerSpells(region, SummonerSpellData.all);
                var json = JsonConvert.SerializeObject(spellList);
                File.WriteAllText(CURRENT_PATH + "spellList.json", json);
            }
        }

        private void updateRegion(Region region) {
            API.init(region);

            if (!Directory.Exists(HOME_PATH))
                Directory.CreateDirectory(HOME_PATH);

            CURRENT_PATH = HOME_PATH + API.getVersion() + @"\";

            if (!Directory.Exists(CURRENT_PATH))
                Directory.CreateDirectory(CURRENT_PATH);
        }

        public ChampionListStatic getChampionList() {
            return championList;
        }

        public ItemListStatic getItemList() {
            return itemList;
        }

        public SummonerSpellListStatic getSpellList() {
            return spellList;
        }

        public ChampionStatic getChampion(String name) {
            return (from pair in championList.Champions
                    where pair.Value.Name == name
                    select pair.Value).FirstOrDefault();
        }

        public ChampionStatic getChampion(int id) {
            return (from pair in championList.Champions
                    where pair.Value.Id == id
                    select pair.Value).FirstOrDefault();
        }

        public ItemStatic getItem(String name) {
            return (from pair in itemList.Items
                    where pair.Value.Name == name
                    select pair.Value).FirstOrDefault();
        }

        public ItemStatic getItem(int id) {
            return (from pair in itemList.Items
                    where pair.Key == id
                    select pair.Value).FirstOrDefault();
        }

        public SummonerSpellStatic getSpell(String name) {
            return (from pair in spellList.SummonerSpells
                    where pair.Value.Name == name
                    select pair.Value).FirstOrDefault();
        }

        public SummonerSpellStatic getSpell(int id) {
            return (from pair in spellList.SummonerSpells
                    where pair.Value.Id == id
                    select pair.Value).FirstOrDefault();
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
