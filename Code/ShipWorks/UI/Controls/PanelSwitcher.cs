using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Provides TabControl experience at design time, with hidden
    /// tabs at runtime.
    /// </summary>
    public class PanelSwitcher : TabControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PanelSwitcher()
        {
            Appearance = TabAppearance.Buttons;

            // Makes it so the tabs don't recieve focus
            SetStyle(ControlStyles.Selectable, false);
        }

        [DefaultValue(TabAppearance.Buttons)]
        public new TabAppearance Appearance
        {
            get { return base.Appearance; }
            set { base.Appearance = value; }
        }

        /// <summary>
        /// Control is being created
        /// </summary>
        protected override void OnCreateControl()
        {
            if (!DesignMode)
            {
                ItemSize = new Size(0, 1);
                SizeMode = TabSizeMode.Fixed;

                Location = new Point(Location.X - 2, Location.Y - 3);
                Size = new Size(Size.Width + 4, Size.Height + 5);

                foreach (TabPage page in TabPages)
                {
                    page.Padding = new Padding(0);
                }
            }

            base.OnCreateControl();
        }
    }
}
