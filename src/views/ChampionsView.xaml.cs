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

    public partial class ChampionsView : Window {

        private int championsHorizontal;
        private int imageWidth;
        private ChampionListStatic championList;
        private MainWindow mainWindow;

        public ChampionsView(MainWindow mainWindow) {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void init() {
            championList = Core.getInstance().getChampionList();
            championsHorizontal = 10;
            imageWidth = 64;
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
                Grid.SetColumn(championContainer, (int) c % championsHorizontal);
                Grid.SetRow(championContainer, (int) c / championsHorizontal);
                
                c++;
            }
            c += championsHorizontal;
            grdChampions.Height = imageWidth*(c/championsHorizontal);
        }

        void championContainer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            ChampionContainer championContainer = (ChampionContainer) sender;
            ChampionStatic champion = championContainer.champion;
            ChampionView championView = new ChampionView(mainWindow, champion);
            mainWindow.setMenu(championView);       
        }
    }
}
