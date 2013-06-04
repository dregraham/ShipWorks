using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using ShipWorks.UI;
using Interapptive.Shared.UI;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Delegate for the MenuCommandComplete event
    /// </summary>
    public delegate void MenuCommandCompleteEventHandler(object sender, MenuCommandCompleteEventArgs e);

    /// <summary>
    /// EventArgs for the MenuCommandComplete event
    /// </summary>
    public class MenuCommandCompleteEventArgs : EventArgs
    {
        MenuCommandResult result;
        string message;

        /// <summary>
        /// Success constructor
        /// </summary>
        public MenuCommandCompleteEventArgs()
        {
            result = MenuCommandResult.Success;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MenuCommandCompleteEventArgs(MenuCommandResult result, string message)
        {
            this.result = result;
            this.message = message;
        }

        /// <summary>
        /// The result of executing the menu command against a set of objects
        /// </summary>
        public MenuCommandResult Result
        {
            get { return result; }
        }

        /// <summary>
        /// If result is not success this is the user-displayable text explaining why
        /// </summary>
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// Shows the message if the result is not success
        /// </summary>
        public void ShowMessage(IWin32Window owner)
        {
            if (result == MenuCommandResult.Warning)
            {
                MessageHelper.ShowWarning(owner, message);
            }

            if (result == MenuCommandResult.Error)
            {
                MessageHelper.ShowError(owner, message);
            }
        }
    }
}
