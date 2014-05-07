using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RiotSharp;
using src;

namespace src.views {
    public partial class SummonerView : Window, SummonerListener {

        private Summoner summoner, previous;
        private List<LeagueItem> leagues;
        private List<PlayerStatsSummary> summaries; 
        private Region region;
        private String summonerName;
        private RiotApi riotApi;
        private object defaultContent;

        public SummonerView() {
            InitializeComponent();
            SummonerHandler.getInstance().register(this);
            riotApi = Core.getInstance().getRiotApi();
            if (SummonerHandler.getInstance().getSummoner() != null) {
                if (summoner == null)
                    summonerUpdated(SummonerHandler.getInstance().getSummoner());
            } else {
                hideControls();
                lblPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private delegate void ObjectDelegate(object obj);

        private async Task loadSummoner() {
            leagues = await summoner.GetLeaguesAsync();
            summaries = await summoner.GetStatsSummariesAsync();
        }

        private void init(Task task) {
            if(summoner == null || summoner.Id == 0) return;
            resetControls();
            initRanked();
            initNormal();
        }

        private void initRanked() {
            if (leagues == null) return;
            lblSummonerName.Content = summoner.Name;
            lblLevel.Content = "level " + summoner.Level;
            if (leagues == null) return;
            for (int i = 0; i < leagues.Count; i++) {
                LeagueItem league = leagues[i];
                if (league.PlayerOrTeamName != summoner.Name) return;
                Log.info("League Type: " + league.QueueType);
                lblLeagueName.Content = league.LeagueName;
                lblDivision.Content = league.Tier + " " + league.Rank;
                lblWins.Content = league.Wins;
                lblLeaguePoints.Content = league.LeaguePoints;
                int medals = 0;
                if (league.IsHotStreak) {
                    Image imgHot = new Image();
                    imgHot.Source = Util.CreateImage(Core.getInstance().getAssetsPath() + @"profile\league_hot.png");
                    Grid.SetColumn(imgHot, medals);
                    Grid.SetRow(imgHot, 0);
                    grdMedals.Children.Add(imgHot);
                    medals++;
                }
                if (league.IsFreshBlood) {
                    Image imgNew = new Image();
                    imgNew.Source = Util.CreateImage(Core.getInstance().getAssetsPath() + @"profile\league_new.png");
                    Grid.SetColumn(imgNew, medals);
                    Grid.SetRow(imgNew, 0);
                    grdMedals.Children.Add(imgNew);
                    medals++;
                }
                MiniSeries series = league.MiniSeries;
                if (series != null) {
                if (!(series.Losses == 0 && series.Wins == 0)) {
                    lblSeriesTag.Visibility = Visibility.Visible;
                    grdSeries.Visibility = Visibility.Visible;
                    grdSeries.Children.Clear();
                        for (int j = 0; j < series.Progress.Length; j++) {
                        char c = series.Progress[j];
                        Log.info("" + c);
                        switch (c) {
                            case 'W':
                            Image img1 = new Image();
                            img1.Source = Util.CreateImage(Core.getInstance().getCurrentPath() + @"\img\profile\series_win.png");
                            Grid.SetColumn(img1, j);
                            Grid.SetRow(img1, 0);
                            grdSeries.Children.Add(img1);
                            break;
                            case 'L':
                            Image img2 = new Image();
                            img2.Source = Util.CreateImage(Core.getInstance().getCurrentPath() + @"\img\profile\series_lose.png");
                            Grid.SetColumn(img2, j);
                            Grid.SetRow(img2, 0);
                            grdSeries.Children.Add(img2);
                            break;
                            case 'N':
                            Image img3 = new Image();
                            img3.Source = Util.CreateImage(Core.getInstance().getCurrentPath() + @"\img\profile\series_none.png");
                            Grid.SetColumn(img3, j);
                            Grid.SetRow(img3, 0);
                            grdSeries.Children.Add(img3);
                            break;
                        }
                    }
                } else {
                    lblSeriesTag.Visibility = Visibility.Hidden;
                    grdSeries.Visibility = Visibility.Hidden;
                }
                } else {
                    lblSeriesTag.Visibility = Visibility.Hidden;
                    grdSeries.Visibility = Visibility.Hidden;
                }
            }
        }

        private void initNormal() {
            if (summaries == null) return;
            lblAramWins.Content = summaries[0].Wins;
            lblAramTakedowns.Content = summaries[0].AggregatedStats.TotalChampionKills;
            lblAramTurrets.Content = summaries[0].AggregatedStats.TotalTurretsKilled;
            lblDominionWins.Content = summaries[3].Wins;
            lblDominionNodes.Content = summaries[3].AggregatedStats.TotalNodeCapture;
            lblDominionKills.Content = summaries[3].AggregatedStats.TotalChampionKills;
            lbl3v3Wins.Content = summaries[9].Wins;
            lbl3v3Kills.Content = summaries[9].AggregatedStats.TotalChampionKills;
            lbl3v3Turrets.Content = summaries[9].AggregatedStats.TotalTurretsKilled;
        }

        private void resetControls() {
            lblSummonerName.Content = "";
            lblLevel.Content = "";
            lblLeagueName.Content = "";
            lblDivision.Content = "";
            lblLeaguePoints.Content = "";
            lblWins.Content = "0";
            lblSeriesTag.Visibility = Visibility.Hidden;
            grdSeries.Children.Clear();
            grdMedals.Children.Clear();
        }

        private void hideControls() {
            resetControls();
//            lblSummonerName.Visibility = Visibility.Hidden;
//            lblLevel.Visibility = Visibility.Hidden;
//            lblTitleRanked.Visibility = Visibility.Hidden;
//            lblTitleLeagueName.Visibility = Visibility.Hidden;
//            lblTitleLeaguePoints.Visibility = Visibility.Hidden;
//            lblTitleWins.Visibility = Visibility.Hidden;
//            lblTitleDivision.Visibility = Visibility.Hidden;
//            lblTitleMedals.Visibility = Visibility.Hidden;
//            lblWins.Visibility = Visibility.Hidden;
            grdRanked.Visibility = Visibility.Hidden;
        }

        private void showControls() {
//            lblWins.Visibility = Visibility.Visible;
//            lblSummonerName.Visibility = Visibility.Visible;
//            lblLevel.Visibility = Visibility.Visible;
//            lblTitleRanked.Visibility = Visibility.Visible;
//            lblTitleLeagueName.Visibility = Visibility.Visible;
//            lblTitleLeaguePoints.Visibility = Visibility.Visible;
//            lblTitleWins.Visibility = Visibility.Visible;
//            lblTitleDivision.Visibility = Visibility.Visible;
//            lblTitleMedals.Visibility = Visibility.Visible;
//            lblWins.Visibility = Visibility.Visible;
            grdRanked.Visibility = Visibility.Visible;
        }

        public void summonerUpdated(Summoner summoner) {
            previous = this.summoner;
            this.summoner = summoner;
            showControls();
            lblPlaceholder.Visibility = Visibility.Hidden;
            loadSummoner().ContinueWith(init, TaskContinuationOptions.ExecuteSynchronously);
            Log.info("Summoner Updated");
        }

    }
}
