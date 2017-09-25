using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Represents a generic menu command that can be executed
    /// </summary>
    public interface IMenuCommand
    {
        /// <summary>
        /// Raised when the Enabled property changes
        /// </summary>
        event EventHandler EnabledChanged;

        /// <summary>
        /// The Text to display in the menu item
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets if the UI for the command is enabled
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// User data associated with the item
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// Indicates that a separator line will be drawn before this item
        /// </summary>
        bool BreakBefore { get; set; }

        /// <summary>
        /// Indicates that a separator line will be drawn after this item
        /// </summary>
        bool BreakAfter { get; set; }

        /// <summary>
        /// The child MenuCommand objects of this command.  (i.e. submenu items)
        /// </summary>
        List<IMenuCommand> ChildCommands { get; }

        /// <summary>
        /// Execute the command
        /// </summary>
        Task ExecuteAsync(Control owner, IEnumerable<long> selectedKeys, MenuCommandCompleteEventHandler callback);
    }
}