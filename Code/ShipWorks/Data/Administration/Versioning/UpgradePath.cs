using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Data.Administration.Versioning
{
    /// <summary>
    /// Class represents possible upgrade paths to the ToVersion.
    /// </summary>
    public class UpgradePath
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradePath"/> class.
        /// </summary>
        public UpgradePath()
        {
            FromVersion=new List<string>();    
        }

        /// <summary>
        /// The version to Upgrade To.
        /// </summary>
        public string ToVersion
        {
            get;
            set;
        }

        /// <summary>
        /// The version we will upgrade From.
        /// </summary>
        public List<string> FromVersion
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
    }
}
