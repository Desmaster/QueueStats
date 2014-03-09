using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using src.api;
using RiotSharp;
using src.patch;

namespace src {
    class Client {

        public Client() {
            String key = Properties.Settings.Default.api_key;
            var api = RiotApi.GetInstance(key, false);
            API.init("euw");
            Patcher patcher = new Patcher();            
        }

    }
}
