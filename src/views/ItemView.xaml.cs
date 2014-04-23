using System;
using System.Text.RegularExpressions;
using System.Windows;
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
            String description = item.Description;
            description = description.Replace("<br>", "\r\n");
            description = Regex.Replace(description, "<[^>]+>", "");
            tbDescription.Text = description;
        }

    }
}
