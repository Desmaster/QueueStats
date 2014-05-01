using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using RiotSharp;
using VerticalAlignment = System.Windows.VerticalAlignment;

namespace src.views {
    public partial class MatchesView : Window, SummonerListener {

        private SummonerHandler summonerHandler;
        private Core core;
        private int rowNumber = 0;

        private List<Game> games;
        public MatchesView() {
            InitializeComponent();
            summonerHandler = SummonerHandler.getInstance();
            summonerHandler.register(this);

            core = Core.getInstance();
        }

        public void summonerUpdated(Summoner summoner) {
            games = new List<Game>();
            if (summonerHandler.isTracked(summoner)) {
                loadTrackedMatches(summoner);
            } else {
                loadMatches(summoner);
            }

            updateMatchesList(summoner);
        }

        private void updateMatchesList(Summoner summoner) {
            //matchesGrid.ColumnDefinitions.Clear();
            matchesGrid.RowDefinitions.Clear();
            matchesGrid.Children.Clear();
            rowNumber = 0;

            foreach (Game game in games.OrderBy(x => x.CreateDate).Reverse()) {
                addMatchControl(game);
            }
        }

        private void loadTrackedMatches(Summoner summoner) {
            String path = core.getHomePath() + @"matches\" + summoner.Name + @"\";
            foreach (String filePath in Directory.EnumerateFiles(path)) {
                games.Add(JsonConvert.DeserializeObject<Game>(File.ReadAllText(filePath)));
            }
        }

        private async void loadMatches(Summoner summoner) {
            games = summoner.GetRecentGames();
        }

        private void addMatchControl(Game game) {
            //Gameresult (Win or lose)
            Label lblGameResult = new Label();
            lblGameResult.Content = game.Statistics.Win ? "Win" : "Loss";
            lblGameResult.Margin = new Thickness(8);
            lblGameResult.VerticalAlignment = VerticalAlignment.Center;
            lblGameResult.Foreground = new SolidColorBrush(Colors.White);
            lblGameResult.FontSize = 24;
            
            //Champion icon
            Image imgChampionIcon = new Image();
            imgChampionIcon.Source =
                Util.CreateImage(core.getAssetsPath() + @"champion\" + core.getChampion(game.ChampionId).Image.Full);
            imgChampionIcon.Margin = new Thickness(4);


            //Bought items
            StackPanel spItems = new StackPanel();
            spItems.Orientation = Orientation.Horizontal;

            int[] itemsBoughtIds = new int[6];
            itemsBoughtIds[0] = game.Statistics.Item0;
            itemsBoughtIds[1] = game.Statistics.Item1;
            itemsBoughtIds[2] = game.Statistics.Item2;
            itemsBoughtIds[3] = game.Statistics.Item3;
            itemsBoughtIds[4] = game.Statistics.Item4;
            itemsBoughtIds[5] = game.Statistics.Item5;

            for (int i = 0; i < itemsBoughtIds.Length; i++) {
                if (itemsBoughtIds[i] != 0) {
                    Image image = new Image();
                    image.Source = Util.CreateImage(core.getAssetsPath() + @"item\" + itemsBoughtIds[i] + ".png");
                    image.Width = 32;
                    image.Height = 32;
                    image.Margin = new Thickness(2);
                    spItems.Children.Add(image);
                }
            }

            //KDA
            Label lblKills = new Label();
            lblKills.Content = "Kills " + game.Statistics.ChampionsKilled;
            lblKills.VerticalAlignment = VerticalAlignment.Center;
            lblKills.Foreground = new SolidColorBrush(Colors.White);


            Label lblDeaths = new Label();
            lblDeaths.Content = "Deaths " + game.Statistics.NumDeaths;
            lblDeaths.VerticalAlignment = VerticalAlignment.Center;
            lblDeaths.Foreground = new SolidColorBrush(Colors.White);


            Label lblAssists = new Label();
            lblAssists.Content = "Assists " + game.Statistics.Assists;
            lblAssists.VerticalAlignment = VerticalAlignment.Center;
            lblAssists.Foreground = new SolidColorBrush(Colors.White);

            StackPanel spKDA = new StackPanel();
            spKDA.Orientation = Orientation.Vertical;
            spKDA.Children.Add(lblKills);
            spKDA.Children.Add(lblDeaths);
            spKDA.Children.Add(lblAssists);
            
            Color winColor = Color.FromRgb(40, 200, 10);
            Color lossColor = Color.FromRgb(200, 40, 10);
            Color winColorDark = Color.FromRgb(40, 60, 10);
            Color lossColorDark = Color.FromRgb(60, 10, 10);

            Grid matchRow = new Grid();
            matchRow.Background = new LinearGradientBrush(game.Statistics.Win ? winColor : lossColor, game.Statistics.Win ? winColorDark : lossColorDark, 90.0);
            matchRow.Margin = new Thickness(4);

            ColumnDefinition GameResult = new ColumnDefinition();
            GameResult.Width = new GridLength(70);
           
            ColumnDefinition ChampionIcon = new ColumnDefinition();
            ChampionIcon.Width = new GridLength(64);
            
            ColumnDefinition ItemsBought = new ColumnDefinition();
            ItemsBought.Width = new GridLength(216);
            
            ColumnDefinition KDA = new ColumnDefinition();
            KDA.Width = GridLength.Auto;

            matchRow.ColumnDefinitions.Add(GameResult);
            matchRow.ColumnDefinitions.Add(ChampionIcon);
            matchRow.ColumnDefinitions.Add(ItemsBought);
            matchRow.ColumnDefinitions.Add(KDA);
            matchRow.RowDefinitions.Add(new RowDefinition());

            matchRow.Children.Add(lblGameResult);
            Grid.SetColumn(lblGameResult, 0);
            Grid.SetRow(lblGameResult, 0);
            
            matchRow.Children.Add(imgChampionIcon);
            Grid.SetColumn(imgChampionIcon, 1);
            Grid.SetRow(imgChampionIcon, 0);

            matchRow.Children.Add(spItems);
            Grid.SetColumn(spItems, 2);
            Grid.SetRow(spItems, 0);

            matchRow.Children.Add(spKDA);
            Grid.SetColumn(spKDA, 3);
            Grid.SetRow(spKDA, 0);

            matchesGrid.RowDefinitions.Add(new RowDefinition());
            matchesGrid.Children.Add(matchRow);
            Grid.SetColumn(matchRow, 0);
            Grid.SetRow(matchRow, rowNumber);

            rowNumber++;
        }
    }
}
