using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using ShipWorks.UI.Utility;

namespace ShipWorks.ApplicationCore.Options
{
    /// <summary>
    /// Base page for pages that are displayed in the options window
    /// </summary>
    [ToolboxItem(false)]
    public partial class OptionPageBase : UserControl
    {
        ThemedBorderProvider borderProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public OptionPageBase()
        {
            InitializeComponent();

            borderProvider = new ThemedBorderProvider(this);
        }

        /// <summary>
        /// Can be overridden by derived page to save any changes
        /// </summary>
        public virtual void Save()
        {

        }

        /// <summary>
        /// Custom WndPrc handling
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }
    }
}
