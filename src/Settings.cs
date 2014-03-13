using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src
{
	class Settings {
		public static void setProperty(String key, String value)
		{
			Properties.Settings.Default.PropertyValues[key].PropertyValue = value;
		}

		public static String getProperty(String key)
		{
			return Properties.Settings.Default.PropertyValues[key].PropertyValue.ToString();
		}
	}
}
