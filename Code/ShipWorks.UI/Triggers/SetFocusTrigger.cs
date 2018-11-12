using System.Windows.Controls;
using System.Windows.Interactivity;

namespace ShipWorks.UI.Triggers
{
    /// <summary>
    /// Trigger that will set focus on the target control
    /// </summary>
    public class SetFocusTrigger : TargetedTriggerAction<Control>
    {
        /// <summary>
        /// Invoke the trigger
        /// </summary>
        protected override void Invoke(object parameter) =>
            Target?.Focus();
    }
}
