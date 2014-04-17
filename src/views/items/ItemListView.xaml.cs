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
    /// <summary>
    /// Interaction logic for ItemListView.xaml
    /// </summary>
    public partial class ItemListView : View {

        private MainWindow mainWindow;
        private ItemListStatic itemList;
        private int width;
        private int imageWidth;

        public ItemListView(MainWindow mainWindow) {
            InitializeComponent();
            this.mainWindow = mainWindow;
            itemList = Core.getInstance().getItemList();
            imageWidth = 64;
            width = 11;
            init();
        }

        private void init() {            
            int i = 0;
            foreach (var pair in itemList.Items) {
                ColumnDefinition columnDefinition = new ColumnDefinition {Width = new GridLength(imageWidth)};
                grdItems.ColumnDefinitions.Add(columnDefinition);
                RowDefinition rowDefinition = new RowDefinition {Height = new GridLength(imageWidth)};
                grdItems.RowDefinitions.Add(rowDefinition);

                ItemContainer itemContainer = new ItemContainer(pair.Value);
                itemContainer.Source = Util.CreateImage(Core.getInstance().getAssetsPath() + @"item\" + pair.Value.Image.Full);
                itemContainer.MouseLeftButtonDown += itemContainer_MouseLeftButtonDown;
                grdItems.Children.Add(itemContainer);
                Grid.SetColumn(itemContainer, (int) i % width);
                Grid.SetRow(itemContainer, (int) i / width);
                i++;
            }
            i += width;
            grdItems.Height = imageWidth*(i/width);
        }

        void itemContainer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            ItemContainer itemContainer = (ItemContainer) sender;
            ItemStatic item = itemContainer.item;
            ItemView itemView = new ItemView(mainWindow, item);
            mainWindow.setMenu(itemView);
        }

    }
}
