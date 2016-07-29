using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    ///
    /// </summary>
    public partial class OdbcImportSettingsWizard : WizardForm, IStoreWizard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportSettingsWizard"/> class.
        /// </summary>
        /// <param name="odbcStore">The ODBC store.</param>
        /// <param name="wizardPages"></param>
        public OdbcImportSettingsWizard(OdbcStoreEntity odbcStore, IEnumerable<IOdbcWizardPage> wizardPages)
        {
            Store = odbcStore;

            Pages.AddRange(wizardPages.Cast<WizardPage>().ToArray());

            InitializeComponent();
        }

        /// <summary>
        /// The store currently being configured by the wizard
        /// </summary>
        public StoreEntity Store { get; }
    }
}
