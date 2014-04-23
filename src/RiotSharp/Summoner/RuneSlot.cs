using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RiotSharp
{
    /// <summary>
    /// Slot for a rune (Summoner API).
    /// </summary>
    [Serializable]
    public class RuneSlot
    {
        internal RuneSlot() { }

        /// <summary>
        /// Rune ID associated with the rune slot.
        /// </summary>
        [JsonProperty("runeId")]
        public int RuneId { get; set; }

        /// <summary>
        /// Rune slot ID.
        /// </summary>
        [JsonProperty("runeSlotId")]
        public int RuneSlotId { get; set; }
    }
}
