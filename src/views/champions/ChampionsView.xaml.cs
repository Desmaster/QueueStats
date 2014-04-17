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
using RiotSharp;

namespace src.views {

    public partial class ChampionsView : View {

        private int width, height;
        private int imageWidth;

        private Window mainWindow;
        private ChampionListStatic championList;

        public ChampionsView(Window window) {
            InitializeComponent();
            mainWindow = window;
            championList = Core.getInstance().getChampionList();
            width = 10;
            imageWidth = 64;
            height = championList.Champions.Count / width;
            init();
        }

        private void init() {
            var champions = from pair in championList.Champions orderby pair.Key ascending select pair;
            int c = 0;
            foreach (KeyValuePair<String, ChampionStatic> pair in championList.Champions.OrderBy(p => p.Key)) {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(imageWidth);
                grdChampions.ColumnDefinitions.Add(columnDefinition);

                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(imageWidth);
                grdChampions.RowDefinitions.Add(rowDefinition);

                ChampionContainer championContainer = new ChampionContainer(pair.Value);
                championContainer.Source = Util.CreateImage(Core.getInstance().getAssetsPath() + @"champion\" + pair.Value.Image.Full);
                championContainer.MouseLeftButtonDown += championContainer_MouseLeftButtonDown;
                grdChampions.Children.Add(championContainer);    
                Grid.SetColumn(championContainer, (int) c % width);
                Grid.SetRow(championContainer, (int) c / width);
                
                c++;
            }
            c += width;
            grdChampions.Height = imageWidth*(c/width);
        }

        void championContainer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            ChampionContainer championContainer = (ChampionContainer) sender;
            ChampionStatic champion = championContainer.champion;
            ChampionView championView = new ChampionView(mainWindow, champion);
        }
    }
}
