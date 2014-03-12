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
			core = Core.getInstance(Region.euw, Core.getPropertyString("api_key"), false);
            Log.info("Mastery Page: " + core.getRiotApi().GetSummoner(Region.euw, "Krindle").GetMasteryPages()[0].Name);
		}

		public bool isSummonerSet() {
			return (Core.getPropertyString("summonername") != "" && Core.getPropertyString("region") != "");
		}
	}
}
