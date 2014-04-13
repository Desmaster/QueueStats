using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
            String description = item.Description;
            description = description.Replace("<br>", "\r\n");
            String[] descriptions = Regex.Split(description, @"<.*?>.*?</.*?>|<.*?/>");    
            Log.info("Stats: " + descriptions[0]);
            foreach (Match match in Regex.Matches(description, @"<.*?>.*?</.*?>|<.*?/>")) {
                Log.info(match.Value);
                String value = match.Value;
                if (value.StartsWith("<stats>")) {
                    value = value.Replace("<stats>", "").Replace("</stats>", "");
                    tbDescription.Inlines.Add(new Bold(new Run("Stats \r\n")));
                    tbDescription.Inlines.Add(new Run(value));
                }
            }

            foreach (string desc in descriptions) {
                if (desc.StartsWith("<stats>")) {
                    string stats = desc.Substring(7, desc.IndexOf("</stats>", System.StringComparison.Ordinal) - 7);
                    tbDescription.Inlines.Add(new Run(stats + "\r\n\r\n"));
                } else {
                    tbDescription.Inlines.Add(new Run(desc));
                }
            }
        }

    }
}
