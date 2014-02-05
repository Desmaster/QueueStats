using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src {

    public class Champion {
        public string id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public Image image { get; set; }
        public Skin[] skins { get; set; }
        public string lore { get; set; }
        public string blurb { get; set; }
        public string[] allytips { get; set; }
        public string[] enemytips { get; set; }
        public string[] tags { get; set; }
        public string partype { get; set; }
        public Info info { get; set; }
        public Stats stats { get; set; }
        public Spell[] spells { get; set; }
        public Passive passive { get; set; }
        public Recommended[] recommended { get; set; }
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

    public class Info {
        public int attack { get; set; }
        public int defense { get; set; }
        public int magic { get; set; }
        public int difficulty { get; set; }
    }

    public class Stats {
        public float armor { get; set; }
        public float armorperlevel { get; set; }
        public float attackdamage { get; set; }
        public float attackdamageperlevel { get; set; }
        public float attackrange { get; set; }
        public float attackspeedoffset { get; set; }
        public float attackspeedperlevel { get; set; }
        public float crit { get; set; }
        public float critperlevel { get; set; }
        public float hp { get; set; }
        public float hpperlevel { get; set; }
        public float hpregen { get; set; }
        public float hpregenperlevel { get; set; }
        public float movespeed { get; set; }
        public float mp { get; set; }
        public float mpperlevel { get; set; }
        public float mpregen { get; set; }
        public float mpregenperlevel { get; set; }
        public float spellblock { get; set; }
        public float spellblockperlevel { get; set; }
    }

    public class Passive {
        public string name { get; set; }
        public string description { get; set; }
        public Image1 image { get; set; }
    }

    public class Image1 {
        public string full { get; set; }
        public string sprite { get; set; }
        public string group { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
    }

    public class Skin {
        public string id { get; set; }
        public string name { get; set; }
        public int num { get; set; }
    }

    public class Spell {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string tooltip { get; set; }
        public Leveltip leveltip { get; set; }
        public int maxrank { get; set; }
        public int[] cooldown { get; set; }
        public string cooldownBurn { get; set; }
        public int[] cost { get; set; }
        public string costBurn { get; set; }
        public int[][] effect { get; set; }
        public string[] effectBurn { get; set; }
        public Var[] vars { get; set; }
        public string costType { get; set; }
        public object range { get; set; }
        public string rangeBurn { get; set; }
        public Image2 image { get; set; }
        public string resource { get; set; }
    }

    public class Leveltip {
        public string[] label { get; set; }
        public string[] effect { get; set; }
    }

    public class Image2 {
        public string full { get; set; }
        public string sprite { get; set; }
        public string group { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
    }

    public class Var {
        public string link { get; set; }
        public float coeff { get; set; }
        public string key { get; set; }
    }

    public class Recommended {
        public string champion { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string map { get; set; }
        public string mode { get; set; }
        public bool priority { get; set; }
        public Block[] blocks { get; set; }
    }

    public class Block {
        public string type { get; set; }
        public Item[] items { get; set; }
    }

    public class Item {
        public string id { get; set; }
        public int count { get; set; }
    }

}
