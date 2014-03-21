using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using RiotSharp;
using src;

namespace src.views {
    public partial class SummonerView : Window {

        private Summoner summoner;
        private List<LeagueItem> leagues; 
        private Region region;
        private String summonerName;
        private RiotApi riotApi;

        public SummonerView() {
            InitializeComponent();
            riotApi = Core.getInstance().getRiotApi();
            region = SummonerHandler.getInstance().getSummoner().Region;
            summonerName = SummonerHandler.getInstance().getSummoner().Name;
            summoner = SummonerHandler.getInstance().getCurrentSummoner();
            loadSummoner().ContinueWith(init, TaskContinuationOptions.ExecuteSynchronously);
        }

        private delegate void ObjectDelegate(object obj);

        private async Task loadSummoner() {
//            summoner = await riotApi.GetSummonerAsync(region, summonerName);
            leagues = await summoner.GetLeaguesAsync();
        }

        private void init(Task task) {
            lblSummonerName.Content = summoner.Name;
        }
        
    }
}
