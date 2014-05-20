using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using RiotSharp;
using src.stats;
using src.summoner;
using src.util;

namespace src.matches {
    class MatchHandler {
        private static MatchHandler instance;

        private Core core;
        private RiotApi api;
        private SummonerHandler summonerHandler;

        private string HOME_PATH;

        private List<Game> games;
        private AverageStats stats;

        private MatchHandler() {
            core = Core.getInstance();
            api = core.getRiotApi();
            summonerHandler = SummonerHandler.getInstance();

            HOME_PATH = core.getHomePath() + @"matches\";

            //Check for folders
            if (!Directory.Exists(HOME_PATH)) {
                Directory.CreateDirectory(HOME_PATH);
            }

            updateTrackedInfo();
        }

        public static MatchHandler getInstance() {
            if (instance == null) {
                instance = new MatchHandler();
            }
            return instance;
        }

        private async void updateTrackedInfo() {
            foreach (TrackedSummoner summoner in summonerHandler.trackedSummoners) {
                if (!Directory.Exists(HOME_PATH + summoner.Name + @"\")) {
                    Directory.CreateDirectory(HOME_PATH + summoner.Name + @"\");
                }

                updateMatches(await api.GetSummonerAsync(summoner.Region, summoner.Name));
            }
        }

        private async void updateMatches(Summoner summoner)
        {
            games = await summoner.GetRecentGamesAsync();
            foreach (Game game in games) {
                if (!Directory.EnumerateFiles(HOME_PATH + summoner.Name).Any(x => x == game.GameId.ToString())) {
                    File.WriteAllText(HOME_PATH + summoner.Name + @"\" + game.GameId + ".json",
                        await Task.Factory.StartNew(() => JsonConvert.SerializeObject(game)));
                }
            }

            updateAverageStats(summoner);
        }

        public async void updateAverageStats(Summoner summoner) {
            stats = new AverageStats();

            List<ChampionPlayed> championsPlayed = new List<ChampionPlayed>();

            foreach (Game game in summonerHandler.isTracked(summoner) ? await loadTrackedMatches(summoner.Name) : games) {
                //champions stats
                if (championsPlayed.Exists(x => x.championId == game.ChampionId)) {
                    championsPlayed.Find(x => x.championId == game.ChampionId).timesPlayed++;
                } else {
                    championsPlayed.Add(new ChampionPlayed() {
                        championId = game.ChampionId, timesPlayed = 1
                    });
                    Console.WriteLine(Util.resolveChampionId(game.ChampionId));
                }
            }

            stats.champions = championsPlayed.OrderByDescending(x => x.timesPlayed).Take(5).ToList();

            stats.averageKills = games.Average(x => x.Statistics.ChampionsKilled);
            stats.averageDeaths = games.Average(x => x.Statistics.Assists);
            stats.averageAssists = games.Average(x => x.Statistics.NumDeaths);

            stats.totalKills = games.Sum(x => x.Statistics.ChampionsKilled);
            stats.totalDeaths = games.Sum(x => x.Statistics.NumDeaths);
            stats.totalAssists = games.Sum(x => x.Statistics.Assists);

            stats.totalDoubleKills = games.Sum(x => x.Statistics.DoubleKills);
            stats.totalTripleKills = games.Sum(x => x.Statistics.TripleKills);
            stats.totalQuadraKills = games.Sum(x => x.Statistics.QuadraKills);
            stats.totalPentaKills = games.Sum(x => x.Statistics.PentaKills);

            stats.totalMagicDamageDealt = games.Sum(x => x.Statistics.MagicDamageDealtToChampions);
            stats.totalPhysicalDamageDealt = games.Sum(x => x.Statistics.PhysicalDamageDealtToChampions);
            stats.totalTrueDamageDealt = games.Sum(x => x.Statistics.TrueDamageDealtToChampions);
            stats.totalDamageDealt = games.Sum(x => x.Statistics.TotalDamageDealtToChampions);

            stats.totalMagicDamageTaken = games.Sum(x => x.Statistics.MagicDamageTaken);
            stats.totalPhysicalDamageTaken = games.Sum(x => x.Statistics.PhysicalDamageTaken);
            stats.totalTrueDamageTaken = games.Sum(x => x.Statistics.TrueDamageTaken);
            stats.totalDamageTaken = games.Sum(x => x.Statistics.TotalDamageTaken);

            stats.averageGoldEarned = games.Average(x => x.Statistics.GoldEarned);
            stats.goldEarned = games.Sum(x => x.Statistics.GoldEarned);
            stats.goldSpent = games.Sum(x => x.Statistics.GoldSpent);

            if (summonerHandler.isTracked(summoner))
                File.WriteAllText(HOME_PATH + summoner.Name + @"\averageStats.json", await Task.Factory.StartNew(() => JsonConvert.SerializeObject(stats)));
        }

        public async Task<List<Game>> loadTrackedMatches(String summonerName) {
            List<Game> games = new List<Game>();
            String path = HOME_PATH + summonerName + @"\";
            foreach (String filePath in Directory.EnumerateFiles(path)) {
                if (filePath.Contains("averageStats"))
                    continue;
                String fileContent = File.ReadAllText(filePath);
                Game game = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Game>(fileContent));

                games.Add(game);
            }

            return games;
        }

        public async Task<AverageStats> getAverageStats(Summoner summoner) {
            if (summonerHandler.isTracked(summoner)) {
                return
                    await
                        Task.Factory.StartNew(
                            () =>
                                JsonConvert.DeserializeObject<AverageStats>(
                                    File.ReadAllText(HOME_PATH + summoner.Name + @"\averageStats.json")));
            } else
            {
                games = await summoner.GetRecentGamesAsync();
                updateAverageStats(summoner);
                return stats;
            }
        }
    }
}
