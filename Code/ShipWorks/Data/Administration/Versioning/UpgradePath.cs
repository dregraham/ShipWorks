using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ShipWorks.Data.Administration.Versioning
{
    /// <summary>
    /// Class represents possible upgrade paths to the To.
    /// </summary>
    public class UpgradePath
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradePath"/> class.
        /// </summary>
        public UpgradePath()
        {
            From = new List<VersionAndScriptName>();    
        }

        /// <summary>
        /// The version to Upgrade To.
        /// </summary>
        public string To
        {
            get;
            set;
        }

        /// <summary>
        /// The version we will upgrade From.
        /// </summary>
        public List<VersionAndScriptName> From
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Gets the name of the script.
        /// </summary>
        /// <param name="FromVersion">From version.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        internal string GetScriptName(string FromVersion)
        {
            VersionAndScriptName selectedFromVersion = From.SingleOrDefault(f => f.Version == FromVersion);

            return String.IsNullOrEmpty(selectedFromVersion.Script) ? 
                String.Format("{0}To{1}", selectedFromVersion.Version, To) : 
                selectedFromVersion.Script;
        }
    }
}
