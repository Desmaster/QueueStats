using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src.api.item {

    public class Item {
        public int id { get; set; }
        public string name { get; set; }
        public string plaintext { get; set; }
        public string description { get; set; }
        public string colloq { get; set; }
        public string[] into { get; set; }
        public Image image { get; set; }
        public Gold gold { get; set; }
        public string[] tags { get; set; }
        public Stats stats { get; set; }
    }

    public class Image {
        public string full { get; set; }
        public string sprite { get; set; }
        public string group { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
    }

    public class Gold {
        public int _base { get; set; }
        public int total { get; set; }
        public int sell { get; set; }
        public bool purchasable { get; set; }
    }

    public class Stats {
        public float FlatHPPoolMod { get; set; }
    }

}

