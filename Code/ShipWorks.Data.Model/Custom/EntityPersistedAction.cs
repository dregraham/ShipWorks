using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Model.Custom
{
    /// <summary>
    /// Action taken during an entity persist
    /// </summary>
    public enum EntityPersistedAction
    {
        Insert,
        Update,
        Delete
    }
}
