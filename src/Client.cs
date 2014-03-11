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
		Core core;

        public Client() {
            String key = Properties.Settings.Default.api_key;
            core = Core.getInstance(Region.euw, key, false);
            ChampionStatic champ = core.getChampion("VelKoz");
            Patcher patcher = new Patcher();
		}


		public Core getCore() {
			return core;
		}

		public bool summonerSet() {
			return (Core.getPropertyString("summonername") != "" && Core.getPropertyString("region") != "");
		}
	}
}
