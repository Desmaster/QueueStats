using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace src
{
	class DataHandler
	{
		private String HOME_PATH;

		public DataHandler(String apppath)
		{
			if (Directory.Exists(apppath))
			{
			    HOME_PATH = apppath;
			}
		}

		public void addData(String folderpath)
		{
		    if (Directory.Exists(HOME_PATH + folderpath))
		    {
		        
		    }
		}
		
		public String loadData(String key)
		{
			return File.ReadAllText(HOME_PATH);
		}
	}
}
