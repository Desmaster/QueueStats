﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RiotSharp
{
    /// <summary>
    /// Class representing metadata on runes and items (Static API).
    /// </summary>
    [Serializable]
    public class MetadataStatic
    {
        internal MetadataStatic() { }

        /// <summary>
        /// Whether this item is a rune or not.
        /// </summary>
        [JsonProperty("isRune")]
        public bool IsRune { get; set; }

        /// <summary>
        /// Tier of the rune.
        /// </summary>
        [JsonProperty("tier")]
        public int Tier { get; set; }

        /// <summary>
        /// Type of the rune.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
