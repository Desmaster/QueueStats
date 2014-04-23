using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using RiotSharp;

namespace src.views {
    public partial class MatchesView : Window, SummonerListener {

        private SummonerHandler summonerHandler;
        private Core core;

        private List<Game> games = new List<Game>();
        public MatchesView() {
            InitializeComponent();
            summonerHandler = SummonerHandler.getInstance();
            summonerHandler.register(this);

            core = Core.getInstance();
        }

        public void summonerUpdated(Summoner summoner) {
            if (summonerHandler.isTracked(summoner)) {
                loadTrackedMatches(summoner);
            } else {
                loadMatches(summoner);
            }

            updateMatchesList(summoner);
        }

        private void updateMatchesList(Summoner summoner) {
            matches.Children.Clear();

            foreach (Game game in games) {
                Image image = new Image();

                var championName = from pair in core.getChampionList().Champions
                                      where pair.Value.Id == game.ChampionId.ToString()
                                      select pair.Value.Name;

                image.Source = Util.CreateImage(Core.getInstance().getAssetsPath() + @"\champion\" + championName.First());
                matches.Children.Add(image);
            }
        }

        private void loadTrackedMatches(Summoner summoner) {
            String path = core.getHomePath() + @"matches\" + summoner.Name + @"\";
            foreach (String filePath in Directory.EnumerateFiles(path)) {
                games.Add(JsonConvert.DeserializeObject<Game>(File.ReadAllText(filePath)));
            }
        }

        private async void loadMatches(Summoner summoner) {
            games = await summoner.GetRecentGamesAsync();
        }
    }
}
