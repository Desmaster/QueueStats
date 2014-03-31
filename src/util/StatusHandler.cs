using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace src.util {
    class StatusHandler {
        public static MainWindow window;

        private static void status(String statusString) {
            window.lblStatus.Content = statusString;
        }

        public static void info(String statusString) {
            window.lblStatus.Foreground = new SolidColorBrush(Colors.DarkGray);
            status(statusString);
        }

        public static void error(String statusString) {
            window.lblStatus.Foreground = new SolidColorBrush(Colors.DarkRed);
            status(statusString);
        }

        public static void reset() {
            window.lblStatus.Content = "";
        }
    }
}
