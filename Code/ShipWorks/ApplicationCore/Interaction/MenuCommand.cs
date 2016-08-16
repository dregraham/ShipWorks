using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters;
using System.ComponentModel;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Grid;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Represents a generic menu command that can be executed
    /// </summary>
    public class MenuCommand
    {
        // The text to display for the item
        string text;

        // Controls if the UI for the command is enabled
        bool enabled = true;

        // The event handler that will be called when the item is invoked
        protected MenuCommandExecutor ExecuteEvent;

        // User data
        object tag;

        // Indicates that a seperator break will be drawn before and\or after this item
        bool breakBefore = false;
        bool breakAfter = false;

        // Child menu commands (i.e. submenu)
        List<MenuCommand> childCommands = new List<MenuCommand>();

        // Raised when the Enabled property changes
        public event EventHandler EnabledChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public MenuCommand(MenuCommandExecutor executeEvent)
        {
            this.ExecuteEvent = executeEvent;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MenuCommand(string text, MenuCommandExecutor executeEvent)
        {
            this.text = text;
            this.ExecuteEvent = executeEvent;
        }

        /// <summary>
        /// Create a MenuCommand with no event handler
        /// </summary>
        public MenuCommand(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// The Text to display in the menu item
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Gets or sets if the UI for the command is enabled
        /// </summary>
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                if (enabled == value)
                {
                    return;
                }

                enabled = value;

                if (EnabledChanged != null)
                {
                    EnabledChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// User data associated with the item
        /// </summary>
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        /// <summary>
        /// Indicates that a seperator line will be drawn before this item
        /// </summary>
        public bool BreakBefore
        {
            get { return breakBefore; }
            set { breakBefore = value; }
        }

        /// <summary>
        /// Indicates that a seperator line will be drawn after this item
        /// </summary>
        public bool BreakAfter
        {
            get { return breakAfter; }
            set { breakAfter = value; }
        }

        /// <summary>
        /// The child MenuCommand objects of this command.  (i.e. submenu items)
        /// </summary>
        public List<MenuCommand> ChildCommands
        {
            get { return childCommands; }
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void ExecuteAsync(Control owner, IEnumerable<long> selectedKeys, MenuCommandCompleteEventHandler callback)
        {
            if (ExecuteEvent != null)
            {
                ExecuteEvent(new MenuCommandExecutionContext(this, owner, selectedKeys, callback));
            }
        }
    }
}
