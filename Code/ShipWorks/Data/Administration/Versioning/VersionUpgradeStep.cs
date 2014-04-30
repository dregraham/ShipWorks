using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.Versioning
{
    public class VersionUpgradeStep
    {
        public string Version { get; set; }
        public string Script { get; set; }
        public string Process { get; set; }
    }
}
