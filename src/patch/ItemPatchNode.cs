using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Newtonsoft.Json;

using RiotSharp;

using src.api;

namespace src.patch {
    class ItemPatchNode : PatchNode {

        private ItemStatic item { get; set; }
        private String regex = "[^0-9a-zA-Z]+";

        public ItemPatchNode(ItemStatic item, StaticRiotApi staticAPI) : base(staticAPI) {
            this.item = item;
            name = Regex.Replace(item.Name, regex, "");
        }

        override
        public bool patched(String path) {
            return File.Exists(path + @"items\" + name + ".json");
        }

        override
        public int patch(String path) {
            String fullPath = path + @"items\";
            String fileName = name + ".json";
            if(!Directory.Exists(fullPath)) {
                Directory.CreateDirectory(fullPath);
            }
            if(!File.Exists(fullPath + fileName)) {
                Console.WriteLine("Downloading " + item.Name + ".json");
                String json = JsonConvert.SerializeObject(item);
                File.WriteAllText(fullPath + fileName, json);
            }
            return 1;
        }

    }
}
