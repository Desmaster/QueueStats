using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using src.patch;

namespace src.views {

    public partial class SettingsView : Window {

        private PatchClient patchClient;
        private MainWindow mainWindow;

        public SettingsView(MainWindow mainWindow) {
            this.mainWindow = mainWindow;
            InitializeComponent();
            patchClient = new PatchClient(this);
            if (patchClient.shouldPatch()) {
                patch();
            } else {
                lblStatus.Content = "Nothing to download right now!";
            }
        }

        private async void patch() {
            await patchClient.patch();
        }

        public void setProgress(double progress) {
            Dispatcher.Invoke(DispatcherPriority.Normal, (MyDelegate)
                delegate() {
                    pbProgress.Value = progress;
                }
           );
        }

        private delegate void MyDelegate();

        public void setStatus(String status) {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (MyDelegate)
               (() => lblStatus.Content = status)
            );
        }

        public void completion() {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (MyDelegate)
               (() => mainWindow.initViews())
            );
        }

        private void btnClearDownloadedData_Click(object sender, RoutedEventArgs e) {
            foreach (var directory in Directory.EnumerateDirectories(Core.getInstance().getHomePath())) {
                if (directory.EndsWith("matches") || directory.EndsWith("summoners")) continue;
                Directory.Delete(directory);
            }
        }

        private void btnClearSummonerData_Click(object sender, RoutedEventArgs e) {
            try {
                Directory.Delete(Core.getInstance().getHomePath() + "matches", true);
                Directory.Delete(Core.getInstance().getHomePath() + "summoners", true);
                File.Delete(Core.getInstance().getHomePath() + "trackedSummoners.json");
            } catch {
                
            }
        }

    }
}
