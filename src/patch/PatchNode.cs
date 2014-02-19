using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src.patch {

    public abstract class PatchNode {

        public String name { get; set; }
        protected String type { get; set; }

        public PatchNode(String name, String type) {
            this.name = name;
            this.type = type;
        }

        public abstract bool patched(String path); 

        public abstract Task patch(String path);

    }

}
