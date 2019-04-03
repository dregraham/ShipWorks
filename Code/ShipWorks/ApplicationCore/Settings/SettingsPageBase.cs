using System.ComponentModel;
using System.Windows.Forms;
using ShipWorks.UI.Utility;

namespace ShipWorks.ApplicationCore.Settings
{
    /// <summary>
    /// Base page for pages that are displayed in the options window
    /// </summary>
    [ToolboxItem(false)]
    public partial class SettingsPageBase : UserControl
    {
        private ThemedBorderProvider borderProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsPageBase()
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
