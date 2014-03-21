using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp;

namespace src {

    class Util {

        public static Region? resolveRegion(String name) {
            switch(name) 
            {
                case "br":
                    return Region.br;
                case "eune":
                    return Region.eune;
                case "euw":
                    return Region.euw;
                case "na":
                    return Region.na;
                case "tr":
                    return Region.tr;
                default:
                    return null;
            }
        }

    }
}
