using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks.Common.Editors;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for running a command
    /// </summary>
    [ActionTask("Run a command", "RunCommand", ActionTriggerClassifications.Scheduled)]
    public class RunCommandTask : ActionTask
    {
        /// <summary>
        /// Creates the editor that will be used to modify the task
        /// </summary>
        /// <returns></returns>
        public override ActionTaskEditor CreateEditor()
        {
            return new RunCommandTaskEditor(this);
        }

        /// <summary>
        /// This task requires input
        /// </summary>
        public override bool RequiresInput
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets the command that should be executed
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets whether the command should be stopped after the timeout has elapsed
        /// </summary>
        public bool ShouldStopCommandOnTimeout { get; set; }

        /// <summary>
        /// Gets or sets how long to wait before timing out the command
        /// </summary>
        public int CommandTimeoutInMinutes { get; set; }
    }
}
