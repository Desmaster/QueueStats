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
using System.Windows.Threading;
using src.patch;

namespace src.views {

    public partial class SettingsView : Window {

        private PatchClient patchClient;

        public SettingsView() {
            InitializeComponent();
            patchClient = new PatchClient(this);
            if (patchClient.shouldPatch()) {
                patch();
            } else {
                
            }
            
        }

        private async void patch() {
            await patchClient.patch();
        }

        public void setProgress(double progress) {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (MyDelegate)
                delegate() {
                    pbProgress.Value= progress;
                }
           );
        }

        private delegate void MyDelegate();

        public void setStatus(String status) {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (MyDelegate)
               delegate() {
                   lblStatus.Content = status;
               }
            );
        }



    }
}
