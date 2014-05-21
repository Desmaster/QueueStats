using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
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
            switch (name) {
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

        public static String resolveChampionId(int id)
        {
            String name = Core.getInstance().getChampion(id).Name;
            Log.info(name);
            switch (name) {
                case "Fiddlesticks":
                    return "FiddleSticks";
                default:
                    return name.Replace(" ", "");
            }
        }

        public static String resolveItemId(int id) {
            return Core.getInstance().getItem(id).Name;
        }

        public static Image cropImage(Image image, Rect rect) {
            
            return image;

        }
    }
}
