using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Miva.WizardPages
{
    /// <summary>
    /// Wizard page for determining if a user already has the miva module installed
    /// </summary>
    public partial class MivaModuleQuestionPage : WizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MivaModuleQuestionPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Indicates if the module is installed
        /// </summary>
        public bool IsModuleInstalled
        {
            get { return radioInstalled.Checked; }
        }

    }
}
