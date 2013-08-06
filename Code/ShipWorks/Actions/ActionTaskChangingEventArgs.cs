using System.ComponentModel;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Delegate for the TaskChanging event
    /// </summary>
    public delegate void ActionTaskChangingEventHandler(object sender, ActionTaskChangingEventArgs e);

    /// <summary>
    /// Event data for the TaskChanging event
    /// </summary>
    public class ActionTaskChangingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Creates a new instance of the class
        /// </summary>
        /// <param name="originalTask">Task that was selected before it was changed</param>
        /// <param name="newTask">Task that is currently selected</param>
        public ActionTaskChangingEventArgs(ActionTask originalTask, ActionTask newTask)
        {
            OriginalTask = originalTask;
            NewTask = newTask;
        }

        /// <summary>
        /// The task that was selcted before it was changed
        /// </summary>
        public ActionTask OriginalTask { get; private set; }

        /// <summary>
        /// The task that is currently selected
        /// </summary>
        public ActionTask NewTask { get; private set; }
    }
}
