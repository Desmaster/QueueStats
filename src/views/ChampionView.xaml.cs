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

    public partial class ChampionView : Window {

        private MainWindow mainWindow;
        private ChampionStatic champion;

        public ChampionView(MainWindow mainWindow, ChampionStatic champion) {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.champion = champion;
            lblName.Content = champion.Name;
            lblTitle.Content = champion.Title;
            int spells = 0;
            foreach (var spell in champion.Spells) {
                ColumnDefinition columnDefinition = new ColumnDefinition {Width = new GridLength(512)};
                grdMain.ColumnDefinitions.Add(columnDefinition);
                RowDefinition rowDefinition = new RowDefinition {Height = new GridLength(48)};
                grdMain.RowDefinitions.Add(rowDefinition);
                Grid tempGrid = new Grid();
                columnDefinition = new ColumnDefinition {Width = new GridLength(48)};
                tempGrid.ColumnDefinitions.Add(columnDefinition);
                columnDefinition = new ColumnDefinition {Width = new GridLength(256)};
                tempGrid.ColumnDefinitions.Add(columnDefinition);

                rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(48);
                tempGrid.RowDefinitions.Add(rowDefinition);

                Image image = new Image();
                image.Width = 48;
                image.Height = 48;
                image.Source = Util.CreateImage(Core.getInstance().getAssetsPath() + @"spell\" + spell.Image.Full);

                Label label = new Label();
                label.Content = spell.Description;

                Grid.SetColumn(image, 0);
                Grid.SetRow(image, 0);
                Grid.SetColumn(label, 1);
                Grid.SetRow(label, 0);
                tempGrid.Children.Add(image);
                tempGrid.Children.Add(label);
                Grid.SetColumn(tempGrid, 0);
                Grid.SetRow(tempGrid, spells);
                grdMain.Children.Add(tempGrid);
                spells++;
            }
            imgChampion.Source =
                Util.CreateImage(Core.getInstance().getAssetsPath() + @"champion\" + champion.Image.Full);
        }

    }

}
