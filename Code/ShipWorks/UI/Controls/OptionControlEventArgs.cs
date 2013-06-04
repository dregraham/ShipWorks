using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.UI.Controls
{
    public delegate void OptionControlEventHandler(object sender, OptionControlEventArgs e);

    /// <summary>
    /// Provides data for the Selected and Deselected events of a OptionControl control
    /// </summary>
    public class OptionControlEventArgs : EventArgs
    {
        TabControlAction action;
        OptionPage optionPage;
        int optionPageIndex;

        // Methods
        public OptionControlEventArgs(OptionPage optionPage, int optionPageIndex, TabControlAction action)
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
