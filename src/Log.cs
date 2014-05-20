using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src {

    class Log {
        private static void log(String message) {
            Console.WriteLine(DateTime.Now + " " + message);
        }

        public static void info(String message) {
            log("[INFO]: " + message);
        }

    }

}
