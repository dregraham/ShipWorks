using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Class represents possible upgrade paths to the ToVersion.
    /// </summary>
    public class UpgradePath
    {
        public string ToVersion
        {
            get;
            set;
        }

        public List<string> FromVersion
        {
            get;
            set;
        }
    }
}
