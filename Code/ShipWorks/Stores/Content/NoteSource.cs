using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Type\classification of notes.
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum NoteSource
    {
        /// <summary>
        /// A note entered by a ShipWorks user
        /// </summary>
        [Description("Manual")]
        ShipWorksUser = 0,

        /// <summary>
        /// A note that was entered b\c it was downloaded from an online store
        /// </summary>
        [Description("Downloaded")]
        Downloaded = 1,

        /// <summary>
        /// A note that was combined from other order(s)
        /// </summary>
        [Description("Combined Order")]
        CombinedOrder = 2
    }
}
