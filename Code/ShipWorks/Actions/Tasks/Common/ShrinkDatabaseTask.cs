using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration;
using log4net;
using ShipWorks.SqlServer.Common.Data;
using ShipWorks.Templates;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Emailing;
using ShipWorks.Email;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Templates.Processing;
using ShipWorks.Data;
using ShipWorks.Data.Model;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for shrinking a database
    /// </summary>
    [ActionTask("Shrink the database", "ShrinkDatabase", ActionTriggerClassifications.Scheduled)]
    public class ShrinkDatabaseTask : ActionTask
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ShrinkDatabaseTask));

        /// <summary>
        /// Creates the editor that is used to edit the task.
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new ShrinkDatabaseTaskEditor(this);
        }

        /// <summary>
        /// Run the task 
        /// </summary>
        protected override void Run()
        {
            log.Info("Starting to shrink database.");
            try
            {
                SqlShrinkDatabase.ShrinkDatabase();
            }
            catch (SqlLockException ex)
            {
                throw new ActionTaskRunException("Another computer is currently running a shrink task.", ex);
            }
            catch (SqlException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
            log.Info("Shrink database completed.");
        }

        /// <summary>
        /// Indicates if the task requires input to function.  Such as the contents of a filter, or the item that caused the action.
        /// </summary>
        public override bool RequiresInput
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// The label that goes before what the data source for the task should be.
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Shrink database";
            }
        }
    }
}
