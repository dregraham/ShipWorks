﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Represents a generic menu command that can be executed
    /// </summary>
    public class MenuCommand : IMenuCommand
    {
        // Controls if the UI for the command is enabled
        bool enabled = true;

        // The event handler that will be called when the item is invoked
        protected MenuCommandExecutor ExecuteEvent;

        /// <summary>
        /// Raised when the Enabled property changes
        /// </summary>
        public event EventHandler EnabledChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public MenuCommand(MenuCommandExecutor executeEvent)
        {
            ExecuteEvent = executeEvent;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MenuCommand(string text, MenuCommandExecutor executeEvent) :
            this(executeEvent)
        {
            Text = text;
        }

        /// <summary>
        /// Create a MenuCommand with no event handler
        /// </summary>
        public MenuCommand(string text)
        {
            Text = text;
        }

        /// <summary>
        /// The Text to display in the menu item
        /// </summary>
        public string Text { get; set; }

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

                EnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// User data associated with the item
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Indicates that a separator line will be drawn before this item
        /// </summary>
        public bool BreakBefore { get; set; }

        /// <summary>
        /// Indicates that a separator line will be drawn after this item
        /// </summary>
        public bool BreakAfter { get; set; }

        /// <summary>
        /// The child MenuCommand objects of this command.  (i.e. submenu items)
        /// </summary>
        public List<IMenuCommand> ChildCommands { get; } = new List<IMenuCommand>();

        /// <summary>
        /// Execute the command
        /// </summary>
        public Task ExecuteAsync(Control owner, IEnumerable<long> selectedKeys, MenuCommandCompleteEventHandler callback)
        {
            var context = new MenuCommandExecutionContext(this, owner, selectedKeys, callback);

            ExecuteEvent?.Invoke(context);

            return Task.CompletedTask;
        }
    }
}
