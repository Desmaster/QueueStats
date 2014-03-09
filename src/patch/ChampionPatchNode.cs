using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Newtonsoft.Json;

using RiotSharp;

using src.api;

namespace src.patch {

    class ChampionPatchNode : PatchNode {

        public ChampionStatic champion { get; set; }

        public ChampionPatchNode(ChampionStatic champion, StaticRiotApi staticAPI)
            : base(staticAPI) {
            this.champion = champion;
            name = Regex.Replace(champion.Name, @"[^0-9a-zA-Z]+", "").Trim();
        }

        override
        public bool patched(String path) {
            return File.Exists(path + @"champions\" + name + ".json");
        }

        override
        public void patch(String path) {
            String fullPath = path + @"champions\";
            String fileName = name + ".json";
            if(!Directory.Exists(fullPath)) {
                Directory.CreateDirectory(fullPath);
            }
            if(!File.Exists(fullPath + fileName)) {
                Console.WriteLine("Downloading " + name + ".json");
                String json = JsonConvert.SerializeObject(champion);
                File.WriteAllText(fullPath + fileName, json);
            }
        }


    }

}
