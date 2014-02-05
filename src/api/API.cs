using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace src.api {

    class API {

        private static String region;
        private static String version;

        private const String HOST = "http://tdegroot.nl/api/qstats/";

        public const String CHAMPION_GET = "/api/lol/{region}/v1.1/champion";

        public const String GAME_RECENT = "/api/lol/{region}/v1.3/game/by-summoner/{summonerId}/recent";

        public const String LEAGUE_CHALLENGER = "/api/lol/{region}/v2.3/league/challenger";
        public const String LEAGUE_BY_SUMMONER = "/api/lol/{region}/v2.3/league/by-summoner/{summonerId}";
        public const String LEAGUE_BY_SUMMONER_ENTRY = "/api/lol/{region}/v2.3/league/by-summoner/{summonerId}/entry";

        public const String STATIC_CHAMPIONS = "/api/lol/static-data/{region}/v1/champion";
        public const String STATIC_CHAMPION_ID = "/api/lol/static-data/{region}/v1/champion/{id}";
        public const String STATIC_ITEMS = "/api/lol/static-data/{region}/v1/item";
        public const String STATIC_ITEM_ID = "/api/lol/static-data/{region}/v1/item/{id}";
        public const String STATIC_MASTERIES = "/api/lol/static-data/{region}/v1/mastery";
        public const String STATIC_MASTERY_ID = "/api/lol/static-data/{region}/v1/mastery/{id}";
        public const String STATIC_RUNES = "/api/lol/static-data/{region}/v1/rune";
        public const String STATIC_RUNE_ID = "/api/lol/static-data/{region}/v1/rune/{id}";
        public const String STATIC_SUMMONER_SPELLS = "/api/lol/static-data/{region}/v1/summoner-spell";
        public const String STATIC_SUMMONER_SPELL_ID = "/api/lol/static-data/{region}/v1/summoner-spell/{id}";
        public const String STATIC_REALM = "/api/lol/static-data/{region}/v1/realm";

        public const String STATS_SUMMARY = "/api/lol/{region}/v1.2/stats/by-summoner/{summonerId}/summary";
        public const String STATS_RANKED = "/api/lol/{region}/v1.2/stats/by-summoner/{summonerId}/ranked";

        public const String SUMMONER_MASTERIES = "/api/lol/{region}/v1.3/summoner/{summonerIds}/masteries";
        public const String SUMMONER_RUNES = "/api/lol/{region}/v1.3/summoner/{summonerIds}/runes";
        public const String SUMMONER_BY_NAME = "/api/lol/{region}/v1.3/summoner/by-name/{summonerNames}";
        public const String SUMMONER_SUMMONERS = "/api/lol/{region}/v1.3/summoner/{summonerIds}";
        public const String SUMMONER_SUMMONERS_NAMES = "/api/lol/{region}/v1.3/summoner/{summonerIds}/name";

        public const String TEAM_BY_SUMMONER = "/api/lol/{region}/v2.2/team/by-summoner/{summonerId}";

        private static HttpWebRequest request;

        public static void init(String region) {
            setRegion(region);
            version = loadVersion(region);
        }

        public static Champion getChampionById(int id) {
            String name = load(STATIC_CHAMPION_ID, new { region = region, id = id });
            return getChampionByName(name);
        }

        public static Champion getChampionByName(String name) {
            Champion champion;
            return null;
        }

        public static void setRegion(String region) {
            API.region = region;
        }

        public static String loadVersion(String region) {
            String json = load(STATIC_REALM, new {region = region});
            Realm realm = JsonConvert.DeserializeObject<Realm>(json);
            return realm.v;
        }

        public static String load(String type, object args) {
            String url = HOST + "index.php?url=" + ReplaceArguments(type, args);
            Console.WriteLine(url);
            request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamData = response.GetResponseStream();
            StreamReader reader = new StreamReader(streamData, Encoding.UTF8);
            string json = reader.ReadToEnd();
            return json.Substring(0, json.Length - 1);
        }

        /**
         * Author: Lasse V. Karlsen
         **/
        private static string ReplaceArguments(string input, object arguments) {
            if(arguments == null || input == null)
                return input;

            var argumentsType = arguments.GetType();
            var re = new Regex(@"\{(?<name>[^}]+)\}");
            return re.Replace(input, match => {
                var pi = argumentsType.GetProperty(match.Groups["name"].Value);
                if(pi == null)
                    return match.Value;

                return (pi.GetValue(arguments) ?? string.Empty).ToString();
            });
        }

    }
}
