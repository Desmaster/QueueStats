﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RiotSharp
{
    /// <summary>
    /// Class representing a mastery tree item or talent (Static API).
    /// </summary>
    [Serializable]
    public class MasteryTreeItemStatic
    {
        internal MasteryTreeItemStatic() { }

        /// <summary>
        /// Id of the mastery.
        /// </summary>
        [JsonProperty("masteryId")]
        public int MasteryId { get; set; }

        /// <summary>
        /// Prerequisite.
        /// </summary>
        [JsonProperty("prereq")]
        public string Prerequisite { get; set; }
    }
}
