using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// A page for use in the AddStoreWizard
    /// </summary>
    public partial class AddStoreWizardPage : WizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AddStoreWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get the store associated
        /// </summary>
        public T GetStore<T>() where T: StoreEntity
        {
            return (T) ((AddStoreWizard) Wizard).Store;
        }
    }
}
