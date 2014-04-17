using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using RiotSharp;

namespace src.views {
    class ItemContainer : Image {

        public ItemStatic item;

        public ItemContainer(ItemStatic item) {
            this.item = item;
        }

    }
}
