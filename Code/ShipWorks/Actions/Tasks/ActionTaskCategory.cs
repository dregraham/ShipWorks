using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// Represents categories of action tasks, for use with presentation in the UI
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ActionTaskCategory
    {
        /// <summary>
        /// For tasks that generate output, like printing and saving
        /// </summary>
        [Description("Output")]
        Output,

        /// <summary>
        /// For tasks that mess with ShipWorks data, like downloading or setting status
        /// </summary>
        [Description("Update Locally")]
        UpdateLocally,

        /// <summary>
        /// For tasks that push things back to online stores
        /// </summary>
        [Description("Update Store")]
        UpdateOnline,

        /// <summary>
        /// For tasks that do something externally, like playing a sound or running a program
        /// </summary>
        [Description("External")]
        External,

        /// <summary>
        /// For admin type tasks, like backuping up the database
        /// </summary>
        [Description("Administration")]
        Administration

    }
}
