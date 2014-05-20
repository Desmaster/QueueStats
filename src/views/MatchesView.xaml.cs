using System;
using System.Collections.Generic;
using System.Globalization;
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
using src.matches;
using VerticalAlignment = System.Windows.VerticalAlignment;

namespace src.views {
    public partial class MatchesView : Window, SummonerListener {

        private SummonerHandler summonerHandler;
        private Core core;
        private int rowNumber = 0;

        private Dictionary<string, SummonerSpellStatic> summonerSpells;

        private List<Game> games;
        public MatchesView() {
            InitializeComponent();
            summonerHandler = SummonerHandler.getInstance();
            summonerHandler.register(this);

            core = Core.getInstance();
        }

        public async void summonerUpdated(Summoner summoner) {
            if (summonerHandler.isTracked(summoner)) {
                games = await MatchHandler.getInstance().loadTrackedMatches(summoner.Name);
            } else {
                games = new List<Game>();
                loadMatches(summoner);
            }

            updateMatchesList(summoner);
        }

        private void updateMatchesList(Summoner summoner) {
            matchesGrid.RowDefinitions.Clear();
            matchesGrid.Children.Clear();
            rowNumber = 0;

            foreach (Game game in games.OrderBy(x => x.CreateDate).Reverse()) {
                addMatchControl(game);
            }
        }

        private async void loadMatches(Summoner summoner) {
            games = await summoner.GetRecentGamesAsync();
        }

        private void addMatchControl(Game game) {
            //Date
            Label lblDate = new Label();
            lblDate.Content = game.CreateDate.ToString("dd MMMM, yyyy", new CultureInfo("en-US"));
            lblDate.VerticalAlignment = VerticalAlignment.Center;
            lblDate.HorizontalAlignment = HorizontalAlignment.Center;
            lblDate.Foreground = new SolidColorBrush(Colors.White);
            lblDate.FontSize = 10;

            //Gameresult (Win or lose)
            StackPanel spGameResult = new StackPanel();
            spGameResult.VerticalAlignment = VerticalAlignment.Center;

            Label lblGameResult = new Label();
            lblGameResult.Content = game.Statistics.Win ? "Victory" : "Defeat";
            lblGameResult.Foreground = new SolidColorBrush(Colors.White);
            lblGameResult.FontSize = 20;
            lblGameResult.FontWeight = FontWeights.Bold;
            lblGameResult.HorizontalAlignment = HorizontalAlignment.Center;

            Label lblGameType = new Label();
            lblGameType.Content = game.SubType.ToString() == "None" ? "Custom" : game.SubType.ToString();
            lblGameType.Foreground = new SolidColorBrush(Colors.White);
            lblGameType.FontSize = 12;
            lblGameType.HorizontalAlignment = HorizontalAlignment.Center;

            spGameResult.Children.Add(lblGameResult);
            spGameResult.Children.Add(lblGameType);

            //Summonerspells
            Image imgSummonerSpell1 = new Image();
            imgSummonerSpell1.Source =
                Util.CreateImage(core.getAssetsPath() + @"spell\" + core.getSpell(game.Spell1).Image.Full);
            imgSummonerSpell1.Width = 32;
            imgSummonerSpell1.Height = 32;
            imgSummonerSpell1.Margin = new Thickness(2);

            Image imgSummonerSpell2 = new Image();
            imgSummonerSpell2.Source =
                Util.CreateImage(core.getAssetsPath() + @"spell\" + core.getSpell(game.Spell2).Image.Full);
            imgSummonerSpell2.Width = 32;
            imgSummonerSpell2.Height = 32;
            imgSummonerSpell2.Margin = new Thickness(2);

            StackPanel spSummonerSpells = new StackPanel();
            spSummonerSpells.VerticalAlignment = VerticalAlignment.Center;
            spSummonerSpells.Orientation = Orientation.Vertical;
            spSummonerSpells.Children.Add(imgSummonerSpell1);
            spSummonerSpells.Children.Add(imgSummonerSpell2);

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
            Label lblKDA = new Label();
            lblKDA.Content = game.Statistics.ChampionsKilled + "/" + game.Statistics.NumDeaths + "/" + game.Statistics.Assists;
            lblKDA.VerticalAlignment = VerticalAlignment.Center;
            lblKDA.Foreground = new SolidColorBrush(Colors.White);

            Color winColor = Color.FromRgb(40, 200, 10);
            Color lossColor = Color.FromRgb(200, 40, 10);
            Color winColorDark = Color.FromRgb(40, 60, 10);
            Color lossColorDark = Color.FromRgb(60, 10, 10);

            Grid matchRow = new Grid();
            matchRow.Background = new LinearGradientBrush(game.Statistics.Win ? winColor : lossColor, game.Statistics.Win ? winColorDark : lossColorDark, 90.0);
            matchRow.Margin = new Thickness(4);
            
            ColumnDefinition Date = new ColumnDefinition();
            Date.Width = new GridLength(70);

            ColumnDefinition GameResult = new ColumnDefinition();
            GameResult.Width = new GridLength(110);

            ColumnDefinition KDA = new ColumnDefinition();
            KDA.Width = new GridLength(50);

            ColumnDefinition SummonerSpells = new ColumnDefinition();
            SummonerSpells.Width = new GridLength(40);

            ColumnDefinition ChampionIcon = new ColumnDefinition();
            ChampionIcon.Width = new GridLength(64);

            ColumnDefinition ItemsBought = new ColumnDefinition();
            ItemsBought.Width = new GridLength(216);

            matchRow.ColumnDefinitions.Add(Date);
            matchRow.ColumnDefinitions.Add(GameResult);
            matchRow.ColumnDefinitions.Add(KDA);
            matchRow.ColumnDefinitions.Add(SummonerSpells);
            matchRow.ColumnDefinitions.Add(ChampionIcon);
            matchRow.ColumnDefinitions.Add(ItemsBought);
            matchRow.RowDefinitions.Add(new RowDefinition());

            matchRow.Children.Add(lblDate);
            Grid.SetColumn(lblDate, 0);
            Grid.SetRow(lblDate, 0);

            matchRow.Children.Add(spGameResult);
            Grid.SetColumn(spGameResult, 1);
            Grid.SetRow(spGameResult, 0);

            matchRow.Children.Add(lblKDA);
            Grid.SetColumn(lblKDA, 2);
            Grid.SetRow(lblKDA, 0);

            matchRow.Children.Add(spSummonerSpells);
            Grid.SetColumn(spSummonerSpells, 3);
            Grid.SetRow(spSummonerSpells, 0);

            matchRow.Children.Add(imgChampionIcon);
            Grid.SetColumn(imgChampionIcon, 4);
            Grid.SetRow(imgChampionIcon, 0);

            matchRow.Children.Add(spItems);
            Grid.SetColumn(spItems, 5);
            Grid.SetRow(spItems, 0);

            matchesGrid.RowDefinitions.Add(new RowDefinition());
            matchesGrid.Children.Add(matchRow);
            Grid.SetColumn(matchRow, 0);
            Grid.SetRow(matchRow, rowNumber);

            rowNumber++;
        }
    }
}
