using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace src
{

	class SummonerHandler
	{
		object[] trackedSummoners;

		public SummonerHandler(){
			Log.info(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\summonerData\"));
			
		}
	}
}
