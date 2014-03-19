using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RiotSharp;

namespace src.patch {

    public abstract class PatchNode {

        public String name;
        protected StaticRiotApi staticAPI;

        public PatchNode(StaticRiotApi staticAPI) {
            this.staticAPI = staticAPI;
        }

        public abstract bool patched(String path); 

        public abstract int patch(String path);

    }

}
