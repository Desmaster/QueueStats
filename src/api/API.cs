using System;
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

using RiotSharp;

namespace src.api {

    class API {

        private static String storagePath;

        private static String version;

        private const String HOST = "http://tdegroot.nl/api/qstats/";

        private const String STATIC_REALM = "/api/lol/static-data/{region}/v1/realm";

        private static HttpWebRequest request;

        public static void init(Region region) {
            version = loadVersion(region);
        }

        private static String loadVersion(Region region) {
            String json = load(STATIC_REALM, new { region = region.ToString() }, null);
            Realm realm = JsonConvert.DeserializeObject<Realm>(json);
            return realm.v;
        }

        public static String load(String type, object data, object args) {
            String url = HOST + "index.php?url=" + ReplaceArguments(type, data);
            if(args != null) {
                url += "&args=" + args;
            }
            return webGet(url);
        }

        private static string webGet(String url) {
            request = (HttpWebRequest)WebRequest.Create(url);
            request.Proxy = null;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamData = response.GetResponseStream();
            StreamReader reader = new StreamReader(streamData, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        public static String getVersion() {
            return version;
        }

        /**
         * Author: Lasse V. Karlsen
         **/
        private static string ReplaceArguments(string input, object arguments) {
            if(arguments == null || input == null)
                return input;

            var argumentsType = arguments.GetType();
            var re = new Regex(@"\{(?<name>[^}]+)\}");
            return re.Replace(input, match => {
                var pi = argumentsType.GetProperty(match.Groups["name"].Value);
                if(pi == null)
                    return match.Value;

                return (pi.GetValue(arguments) ?? string.Empty).ToString();
            });
        }

    }
}
