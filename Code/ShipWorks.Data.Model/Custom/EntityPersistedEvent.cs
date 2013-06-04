using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;

namespace ShipWorks.Data.Model.Custom
{
    /// <summary>
    /// EventHandler for the EntityPersisted event
    /// </summary>
    public delegate void EntityPersistedEventHandler(object sender, EntityPersistedEventArgs e);

    /// <summary>
    /// EventArgs for the EntityPersisted event
    /// </summary>
    public class EntityPersistedEventArgs: EventArgs
    {
        EntityPersistedAction action;

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityPersistedEventArgs(EntityPersistedAction action)
        {
            this.action = action;
        }

        /// <summary>
        /// The action of the persistence
        /// </summary>
        public EntityPersistedAction Action
        {
            get { return action; }
        }
    }
}
