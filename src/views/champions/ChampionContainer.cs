using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using RiotSharp;

namespace src.views {
    class ChampionContainer : Image {

        public ChampionStatic champion;

        public ChampionContainer(ChampionStatic champion) {
            this.champion = champion;
        }



    }
}
