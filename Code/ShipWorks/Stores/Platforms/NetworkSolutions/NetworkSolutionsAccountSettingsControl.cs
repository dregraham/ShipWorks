using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// Control for connecting
    /// </summary>
    public partial class NetworkSolutionsAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Costructor
        /// </summary>
        public NetworkSolutionsAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the store 
        /// </summary>
        /// <param name="store"></param>
        public override void LoadStore(StoreEntity store)
        {
            NetworkSolutionsStoreEntity nsStore = store as NetworkSolutionsStoreEntity;
            if (nsStore == null)
            {
                throw new InvalidOperationException("A non-NetworkSolutions store entity was passed to the NetworkSolutions account settings control.");
            }

            manageTokenControl.InitializeForStore(nsStore);
        }
    }
}
