using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using System.Runtime.InteropServices;
using ShipWorks.ApplicationCore;
using System.ComponentModel;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Button that shows the Vista shield when ran on vista
    /// </summary>
    public class ShieldButton : Button
    {
        bool showShield = true;

        public bool AllowClick { get; set; } = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShieldButton()
        {
            if (MyComputer.IsWindowsVistaOrHigher)
            {
                FlatStyle = FlatStyle.System;

                HandleCreated += new EventHandler(OnHandleCreated);
            }
        }

        /// <summary>
        /// The window handle of the control has been created
        /// </summary>
        void OnHandleCreated(object sender, EventArgs e)
        {
            UpdateShieldVisibility();
        }

        /// <summary>
        /// Controls if the shield on the button should be shown.
        /// </summary>
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool ShowShield
        {
            get
            {
                return showShield;
            }
            set
            {
                if (showShield == value)
                {
                    return;
                }

                showShield = value;

                if (!IsHandleCreated)
                {
                    return;
                }

                else
                {
                    UpdateShieldVisibility();
                }
            }
        }

        /// <summary>
        /// Update the visibility status of the shield
        /// </summary>
        private void UpdateShieldVisibility()
        {
            if (MyComputer.IsWindowsVistaOrHigher)
            {
                NativeMethods.SendMessage(Handle, NativeMethods.BCM_SETSHIELD, IntPtr.Zero, new IntPtr(ShowShield ? 1 : 0));
            }
        }

        protected override void OnClick(EventArgs e)
        {
            if (AllowClick)
            {
                base.OnClick(e);
            }           
        }
    }
}
