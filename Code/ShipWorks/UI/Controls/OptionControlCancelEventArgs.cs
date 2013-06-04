using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace ShipWorks.UI.Controls
{
    public delegate void OptionControlCancelEventHandler(object sender, OptionControlCancelEventArgs e);

    /// <summary>
    /// Provides data for the Selecting and Deselecting events of a OptionControl control. 
    /// </summary>
    public class OptionControlCancelEventArgs : CancelEventArgs
    {
        private TabControlAction action;
        private OptionPage optionPage;
        private int optionPageIndex;

        /// <summary>
        /// Constructor
        /// </summary>
        public OptionControlCancelEventArgs(OptionPage optionPage, int optionPageIndex, bool cancel, TabControlAction action)
            : base(cancel)
        {
            this.optionPage = optionPage;
            this.optionPageIndex = optionPageIndex;
            this.action = action;
        }

        public TabControlAction Action
        {
            get
            {
                return action;
            }
        }

        public OptionPage OptionPage
        {
            get
            {
                return optionPage;
            }
        }

        public int OptionPageIndex
        {
            get
            {
                return optionPageIndex;
            }
        }
    }
}
