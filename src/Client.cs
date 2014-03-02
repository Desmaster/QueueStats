using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using src.api;
using src.api.champion.list;
using src.patch;

namespace src {
    class Client {

        public Client() {
            API.init("na");
            API.load(API.STATIC_CHAMPIONS, new { region = "euw" }, "{\"champData\" : \"all\"}");
            Console.WriteLine(new { region = "region", version = "1.2.3" });
            Console.WriteLine("Version: " + API.getVersion());
            Patcher patcher = new Patcher();            
        }

    }
}
