using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Volusion.WizardPages
{
    /// <summary>
    /// Base class for Volusion Add Store pages
    /// </summary>
    public partial class VolusionAddStoreWizardPage : AddStoreWizardPage
    {
        /// <summary>
        /// Volusion setup flow information, stored in wizard state
        /// </summary>
        protected VolusionSetupState SetupState
        {
            get
            {
                VolusionSetupState setupState = ((AddStoreWizard)Wizard).GetStateValue("VolusionSetup") as VolusionSetupState;
                if (setupState == null)
                {
                    setupState = new VolusionSetupState();
                    ((AddStoreWizard)Wizard).SetStateValue("VolusionSetup", setupState);
                }

                return setupState;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionAddStoreWizardPage()
        {
            InitializeComponent();
        }
    }
}
