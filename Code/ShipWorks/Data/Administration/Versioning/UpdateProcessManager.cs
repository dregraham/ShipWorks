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
        /// Enqueues the update process.
        /// </summary>
        /// <param name="processName">Name of the process.</param>
        public static void EnqueueUpdateProcess(string processName)
        {
            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                sqlAdapter.SaveEntity(new UpdateQueueEntity() { UpdateDatabaseProcessType = processName });
            }
        }

        /// <summary>
        /// Deletes the update process from queue.
        /// </summary>
        public static void DeleteUpdateProcessFromQueue(int updateQueueID)
        {
            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                UpdateQueueEntity updateQueueEntity = ((new LinqMetaData(sqlAdapter))).UpdateQueue.FirstOrDefault(q => q.UpdateQueueID == updateQueueID);
                if (updateQueueEntity == null)
                {
                    throw new InvalidOperationException(string.Format("Update Process with ID '{0}' not found.", updateQueueID));
                }
                sqlAdapter.DeleteEntity(updateQueueEntity);
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
