using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Region = RiotSharp.Region;

namespace src {

    class Util {

        public static ImageSource CreateImage(String path) {
            try {
                return new BitmapImage(new Uri(path));
            } catch (Exception e) {
                return null;
            }
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
