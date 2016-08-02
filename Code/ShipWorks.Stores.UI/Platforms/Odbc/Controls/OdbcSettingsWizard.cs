using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.UI.Wizard;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls
{
    /// <summary>
    ///
    /// </summary>
    public partial class OdbcSettingsWizard : WizardForm, IStoreWizard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcSettingsWizard"/> class.
        /// </summary>
        public OdbcSettingsWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the pages.
        /// </summary>
        /// <param name="odbcStore">The ODBC store.</param>
        /// <param name="wizardPages">The wizard pages.</param>
        public void LoadPages(OdbcStoreEntity odbcStore, IEnumerable<IOdbcWizardPage> wizardPages)
        {
            Store = odbcStore;

            Pages.AddRange(wizardPages.Cast<WizardPage>().ToArray());
        }

        /// <summary>
        /// The store currently being configured by the wizard
        /// </summary>
        public StoreEntity Store { get; private set; }
    }
}
