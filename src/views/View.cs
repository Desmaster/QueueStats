using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace src.views {

    public class View : Window {

        protected static View instance;

        protected View(Window window = null) {
        }

        public static View getInstance(Window window) {
            if (instance == null) {
                instance = (window);
            }

            return instance;
        }
    }
}
