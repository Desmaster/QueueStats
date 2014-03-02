using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using src.api;

namespace src.patch {

    class ChampionPatchNode : PatchNode {

        public int id { get; set; }

        public ChampionPatchNode(int id, String name, String type) : base(name, type) {
            this.id = id;
        }

        override
        public bool patched(String path) {
            if(File.Exists(path + @"champions\" + name + ".json")) {
                return true;
            }
            return false;
        }

        override
        public async Task patch(String path) {
            String fullPath = path + @"champions\";
            String fileName = name + ".json";
            if(!Directory.Exists(fullPath)) {
                Directory.CreateDirectory(fullPath);
            }
            String result = "";
            if(!File.Exists(fullPath + fileName)) {
                result = await API.loadAsync(type, new { region = "euw", id = id }, "{\"champData\":\"all\"}");
                Console.WriteLine("Downloading " + name + ".json");
                File.WriteAllText(fullPath + fileName, result);
            }
        }


    }

}
