using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using RiotSharp;
using src;

namespace src.views {
    public partial class SummonerView : Window, SummonerListener {

        private Summoner summoner;
        private List<LeagueItem> leagues; 
        private Region region;
        private String summonerName;
        private RiotApi riotApi;

        public SummonerView() {
            InitializeComponent();
            SummonerHandler.getInstance().register(this);
            riotApi = Core.getInstance().getRiotApi();
        }

        private delegate void ObjectDelegate(object obj);

        private async Task loadSummoner() {
            leagues = await summoner.GetLeaguesAsync();
        }

        public void summonerUpdated(Summoner summoner) {
            this.summoner = summoner;
            lblSummonerName.Content = summoner.Name;
            Log.info("Summoner Updated");
        }
    }
}
