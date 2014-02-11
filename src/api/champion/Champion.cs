using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src.api.champion {

    public class Champion {
        public string[] tags { get; set; }
        public Stats stats { get; set; }
        public string[] enemytips { get; set; }
        public Recommended[] recommended { get; set; }
        public Image image { get; set; }
        public Spell[] spells { get; set; }
        public string blurb { get; set; }
        public string[] allytips { get; set; }
        public string lore { get; set; }
        public Info info { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public Passive passive { get; set; }
        public string partype { get; set; }
        public string key { get; set; }
        public Skin[] skins { get; set; }
    }

    public class Stats {
        public int attackrange { get; set; }
        public int mpperlevel { get; set; }
        public int mp { get; set; }
        public float attackdamage { get; set; }
        public int hp { get; set; }
        public int hpperlevel { get; set; }
        public float attackdamageperlevel { get; set; }
        public float armor { get; set; }
        public float mpregenperlevel { get; set; }
        public float hpregen { get; set; }
        public int critperlevel { get; set; }
        public float spellblockperlevel { get; set; }
        public float mpregen { get; set; }
        public float attackspeedperlevel { get; set; }
        public int spellblock { get; set; }
        public int movespeed { get; set; }
        public int attackspeedoffset { get; set; }
        public int crit { get; set; }
        public float hpregenperlevel { get; set; }
        public float armorperlevel { get; set; }
    }

    public class Image {
        public int w { get; set; }
        public string full { get; set; }
        public string sprite { get; set; }
        public string group { get; set; }
        public int h { get; set; }
        public int y { get; set; }
        public int x { get; set; }
    }

    public class Info {
        public int defense { get; set; }
        public int magic { get; set; }
        public int difficulty { get; set; }
        public int attack { get; set; }
    }

    public class Passive {
        public string description { get; set; }
        public string name { get; set; }
        public Image1 image { get; set; }
    }

    public class Image1 {
        public int w { get; set; }
        public string full { get; set; }
        public string sprite { get; set; }
        public string group { get; set; }
        public int h { get; set; }
        public int y { get; set; }
        public int x { get; set; }
    }

    public class Recommended {
        public string champion { get; set; }
        public string title { get; set; }
        public bool priority { get; set; }
        public string map { get; set; }
        public Block[] blocks { get; set; }
        public string type { get; set; }
        public string mode { get; set; }
    }

    public class Block {
        public Item[] items { get; set; }
        public string type { get; set; }
    }

    public class Item {
        public string id { get; set; }
        public int count { get; set; }
    }

    public class Spell {
        public object range { get; set; }
        public Leveltip leveltip { get; set; }
        public string resource { get; set; }
        public int maxrank { get; set; }
        public Image2 image { get; set; }
        public string[] effectBurn { get; set; }
        public int[] cooldown { get; set; }
        public int[] cost { get; set; }
        public string id { get; set; }
        public Var[] vars { get; set; }
        public string rangeBurn { get; set; }
        public string costType { get; set; }
        public string cooldownBurn { get; set; }
        public int[][] effect { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string costBurn { get; set; }
        public string tooltip { get; set; }
    }

    public class Leveltip {
        public string[] effect { get; set; }
        public string[] label { get; set; }
    }

    public class Image2 {
        public int w { get; set; }
        public string full { get; set; }
        public string sprite { get; set; }
        public string group { get; set; }
        public int h { get; set; }
        public int y { get; set; }
        public int x { get; set; }
    }

    public class Var {
        public string link { get; set; }
        public float coeff { get; set; }
        public string key { get; set; }
    }

    public class Skin {
        public string id { get; set; }
        public int num { get; set; }
        public string name { get; set; }
    }


}
