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
using System.Windows.Navigation;
using System.Windows.Shapes;

using src.api;
using src.api.champion;

namespace src {

    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            API.init("na");
            //API.load(API.STATIC_CHAMPIONS, new { region = "euw" }, "{\"champData\" : \"all\"}");
            Console.WriteLine(new { region = "region", version = "1.2.3" });
            Console.WriteLine("Version: " + API.getVersion());
        }

        bool mouseDown = false;
        int lastX, lastY;

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            mouseDown = true;
            lastX = (int) e.GetPosition(this).X;
            lastY = (int) e.GetPosition(this).Y;
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e) {
            mouseDown = false;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e) {
            if (mouseDown) {
                int xDiff = (int)e.GetPosition(this).X - lastX;
                int yDiff = (int)e.GetPosition(this).Y - lastY;
                this.Left += xDiff;
                this.Top += yDiff;
            }
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e) {
            mouseDown = false;
        }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("GODVERDOME");
		}

        
    }
}
