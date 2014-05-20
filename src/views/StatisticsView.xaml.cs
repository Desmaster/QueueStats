using System;
using System.Collections.Generic;
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
using src.matches;
using src.stats;
using RiotSharp;

namespace src.views {
    /// <summary>
    /// Interaction logic for StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : Window, SummonerListener
    {

        private Core core;
        private AverageStats stats;
        private MainWindow mainWindow;

        private Random random;

        public StatisticsView(MainWindow mainWindow) {
            InitializeComponent();

            core = Core.getInstance();
            SummonerHandler.getInstance().register(this);

            this.mainWindow = mainWindow;
            random = new Random();
        }

        public async void summonerUpdated(Summoner summoner) {
            stats = await MatchHandler.getInstance().getAverageStats(summoner);

            ImageBrush splashImage = new ImageBrush();
            splashImage.Opacity = 0.5;

            ImageSource imageSource = Util.CreateImage("http://ddragon.leagueoflegends.com/cdn/img/champion/splash/" +
                                 Util.resolveChampionId(stats.champions.First().championId) + "_" + random.Next(0, core.getChampion(stats.champions.First().championId).Skins.Count) + ".jpg");

            imageSource.Changed += (sender, args) =>
            {
                splashImage.ImageSource = imageSource;
            };

            splashImage.ImageSource = imageSource;
            

            mainWindow.setBackground(splashImage);


            lblAvgKills.Content = Math.Round(stats.averageKills, 2);
            lblAvgDeaths.Content = Math.Round(stats.averageDeaths, 2);
            lblAvgAssists.Content = Math.Round(stats.averageAssists, 2);

            lblTotalKills.Content = stats.totalKills;
            lblTotalDeaths.Content = stats.totalDeaths;
            lblTotalAssists.Content = stats.totalAssists;

            spPlayedChamps.Children.Clear();
            foreach (ChampionPlayed champion in stats.champions)
            {
                Image image = new Image();
                image.Source = Util.CreateImage(core.getAssetsPath() + @"champion\" + core.getChampion(champion.championId).Image.Full);

                spPlayedChamps.Children.Add(image);
            }
        }
    }
}
