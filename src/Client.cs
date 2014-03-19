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
    class Client
    {
	    private String HOME_PATH;
		Core core;
        private SummonerHandler summonerHandler;

		public Client() {
			HOME_PATH = HOME_PATH = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\QueueStats\";

		    summonerHandler = SummonerHandler.getInstance(HOME_PATH);
			core = Core.getInstance(Region.euw, Settings.getProperty("api_key"), false);
            Log.info("Mastery Page: " + core.getRiotApi().GetSummoner(Region.euw, "Krindle").GetMasteryPages()[0].Name);
		}
	}
}
