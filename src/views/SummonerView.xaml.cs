using System;
using System.Windows;

using RiotSharp;
using src;

namespace src.views {
    public partial class SummonerView : Window {

        private Summoner summoner;

        public SummonerView() {
            InitializeComponent();
            Console.WriteLine("Pls no Summonerino");
            Console.WriteLine(SummonerHandler.getInstance().getSummoner());
         }
    }
}
