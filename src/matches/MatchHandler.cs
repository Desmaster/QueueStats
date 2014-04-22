using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using RiotSharp;
using src.summoner;
using src.util;

namespace src.matches {
    class MatchHandler {
        private static MatchHandler instance;

        private Core core;
        private RiotApi api;
        private SummonerHandler summonerHandler;

        private string HOME_PATH;

        private MatchHandler() {
            core = Core.getInstance();
            api = core.getRiotApi();
            summonerHandler = SummonerHandler.getInstance();

            HOME_PATH = core.getHomePath() + @"matches\";

            //Check for folders
            if (!Directory.Exists(HOME_PATH)) {
                Directory.CreateDirectory(HOME_PATH);
            }

            foreach (TrackedSummoner summoner in summonerHandler.trackedSummoners) {
                if (!Directory.Exists(HOME_PATH + summoner.Name + @"\")) {
                    Directory.CreateDirectory(HOME_PATH + summoner.Name + @"\");
                }

                updateMatches(summoner);
            }

        }

        public static MatchHandler getInstance() {
            if (instance == null) {
                instance = new MatchHandler();
            }
            return instance;
        }

        private async Task<Summoner> processSummoner(Region region, long id) {
            return await api.GetSummonerAsync(region, (int)id);
        }

        private async Task<List<Game>> processMatch(Task<Summoner> task) {
            return await task.Result.GetRecentGamesAsync();
        }

        private async void updateMatches(TrackedSummoner tsummoner) {
            try {
                Task<Summoner> summonerTask = api.GetSummonerAsync(tsummoner.Region, (int)tsummoner.Id);
                Summoner summoner = await summonerTask;
                Task<List<Game>> matchTask = summoner.GetRecentGamesAsync();

                try {
                    List<Game> games = await matchTask;
                    foreach (Game game in games) {
                        if (!Directory.EnumerateFiles(HOME_PATH + tsummoner.Name).Any(x => x == game.GameId.ToString())) {
                            File.WriteAllText(HOME_PATH + tsummoner.Name + @"\" + game.GameId + ".json",
                                JsonConvert.SerializeObject(game));
                        }
                    }
                } catch (Exception e) {
                    Console.WriteLine(e.Message);

                }
            } catch (Exception e) {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
