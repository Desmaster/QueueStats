using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Region = RiotSharp.Region;

namespace src {

    class Util {

        public static Image CreateImage(String path) {
            Image Mole = new Image();
            Mole.Width = 24;
            Mole.Height = 24;
            ImageSource MoleImage = new BitmapImage(new Uri(path));
            Mole.Source = MoleImage;
            return Mole;
        }

        public static Region resolveRegion(String name) {
            switch(name) 
            {
                case "br":
                    return Region.br;
                case "eune":
                    return Region.eune;
                case "euw":
                    return Region.euw;
                case "na":
                    return Region.na;
                case "tr":
                    return Region.tr;
                default:
                    return Region.na;
            }
        }
    }
}
