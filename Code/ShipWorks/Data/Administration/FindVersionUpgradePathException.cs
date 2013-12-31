using System;
using System.Collections.Generic;

namespace ShipWorks.Data.Administration
{
    [Serializable]
    public class FindVersionUpgradePathException:Exception
    {
        public FindVersionUpgradePathException(string message) : base(message)
        {
        }

        public FindVersionUpgradePathException(string message, Exception ex):base(message, ex)
        {
        }

        
    }
}
