using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Miva
{
    /// <summary>
    /// The name and code of a Miva store that is available for use with an installed module.
    /// </summary>
    public class MivaStoreHeader
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
