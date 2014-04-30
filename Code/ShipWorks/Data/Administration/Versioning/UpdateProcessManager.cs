using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration.Scripts.Update;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;

namespace ShipWorks.Data.Administration.Versioning
{
    /// <summary>
    /// Manages Update process queue
    /// </summary>
    public class UpdateProcessManager
    {
        /// <summary>
        /// Dequeue update process.
        /// </summary>
        /// <returns></returns>
        public static UpdateQueueEntity DequeueUpdateProcess()
        {
            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                return ((new LinqMetaData(sqlAdapter))).UpdateQueue.FirstOrDefault();
            }
        }

        /// <summary>
        /// Deletes the update process from queue.
        /// </summary>
        public static void DeleteUpdateProcessFromQueue(UpdateQueueEntity updateQueueEntity)
        {
            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
               sqlAdapter.DeleteEntity(updateQueueEntity);
            }
        }

        /// <summary>
        /// Gets the update process count.
        /// </summary>
        public static int GetUpdateProcessCount()
        {
            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                return ((new LinqMetaData(sqlAdapter))).UpdateQueue.Count();
            }
        }

        /// <summary>
        /// Gets the update process.
        /// </summary>
        public static IUpdateDatabaseProcess GetUpdateProcess(UpdateQueueEntity updateQueueEntity)
        {
            switch (updateQueueEntity.UpdateDatabaseProcessType)
            {
                case "BestRateToAddressValidation": return new BestRateToAddressValidation();
            }

            throw new InvalidOperationException(string.Format("Unknown ProcessType {0}.", updateQueueEntity.UpdateDatabaseProcessType));
        }
    }
}
