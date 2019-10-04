using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericFile.Sources;
using ShipWorks.Users;

namespace ShipWorks.Stores.Platforms.GenericFile.WizardPages
{
    /// <summary>
    /// Select the location of the data to be imported
    /// </summary>
    public partial class GenericStoreFileSourcePage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStoreFileSourcePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            GenericFileStoreEntity store = GetStore<GenericFileStoreEntity>();

            if (store.FileSource == (int) GenericFileSourceTypeCode.Warehouse)
            {
                e.Skip = true;
                return;
            }

            if (e.FirstTime)
            {
                fileSourceMasterControl.LoadStore(GetStore<GenericFileStoreEntity>(), true);
            }
        }

        /// <summary>
        /// Stepping next from the page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            GenericFileStoreEntity store = GetStore<GenericFileStoreEntity>();

            if (!fileSourceMasterControl.SaveToEntity(store))
            {
                e.NextPage = this;
            }
            else
            {
                GenericFileSourceTypeCode sourceType = (GenericFileSourceTypeCode) store.FileSource;

                // If using the local disk, this is the only computer that should be allowed to download
                if (sourceType == GenericFileSourceTypeCode.Disk)
                {
                    ComputerDownloadPolicy policy = ComputerDownloadPolicy.Load(store);

                    policy.DefaultToYes = false;
                    policy.SetComputerAllowed(UserSession.Computer.ComputerID, ComputerDownloadAllowed.Yes);

                    store.ComputerDownloadPolicy = policy.SerializeToXml();
                }
                else
                {
                    store.ComputerDownloadPolicy = "";
                }
            }
        }
    }
}
