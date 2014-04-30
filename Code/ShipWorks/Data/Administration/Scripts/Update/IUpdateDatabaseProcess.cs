using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.Scripts.Update
{
    /// <summary>
    /// Process that runs during upgrade to update the database.
    /// </summary>
    public interface IUpdateDatabaseProcess
    {
        /// <summary>
        /// Processes this UpdateDatabaseProcess.
        /// </summary>
        void Process();
    }
}
