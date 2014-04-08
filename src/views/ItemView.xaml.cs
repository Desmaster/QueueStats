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
    /// Interaction logic for ItemView.xaml
    /// </summary>
    public partial class ItemView : Window {

        private MainWindow mainWindow;
        private ItemStatic item;

        public ItemView(MainWindow mainWindowm, ItemStatic item) {
            InitializeComponent();
            this.mainWindow = mainWindowm;
            this.item = item;

            lblName.Content = item.Name;
            imgItem.Source = Util.CreateImage(Core.getInstance().getAssetsPath() + @"item\" + item.Image.Full);
        }

    }
}
