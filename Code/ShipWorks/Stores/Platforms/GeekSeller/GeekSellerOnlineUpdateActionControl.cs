using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule.WizardPages;

namespace ShipWorks.Stores.Platforms.GeekSeller
{
    /// <summary>
    /// User control for configuring online update actions for generic stores
    /// </summary>
    public partial class GeekSellerOnlineUpdateActionControl : GenericStoreModuleActionControl
    {
        private IGenericModuleStoreEntity geekSellerStore;
        private const int StatusNotFound = -2;
        private const string JetUrlPart = "/jet/";
        private const string JetShippedStatusName = "Complete";
        private const string WalmartUrlPart = "/walmart/";
        private const string WalmartShippedStatusName = "Shipped";

        /// <summary>
        /// Constructor
        /// </summary>
        public GeekSellerOnlineUpdateActionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the UI for the given store
        /// </summary>
        public override void UpdateForStore(StoreEntity store)
        {
            base.UpdateForStore(store);

            geekSellerStore = store as IGenericModuleStoreEntity;
            List<KeyValuePair<string, object>> dataSource = (List<KeyValuePair<string, object>>) comboStatus.DataSource;

            if (geekSellerStore != null && dataSource?.Any() == true)
            {
                int index = StatusNotFound;

                // If neither Jet or Walmart is found in the url, just use the default status from the base class.
                // If we are Jet, try to find it's status
                if (geekSellerStore.ModuleUrl.IndexOf(JetUrlPart, StringComparison.InvariantCultureIgnoreCase) > 0)
                {
                    index = FindIndex(dataSource, JetShippedStatusName);
                }
                // If we are Walmart, try to find it's status.
                else if (geekSellerStore.ModuleUrl.IndexOf(WalmartUrlPart, StringComparison.InvariantCultureIgnoreCase) > 0)
                {
                    index = FindIndex(dataSource, WalmartShippedStatusName);
                }

                if (index != StatusNotFound)
                {
                    comboStatus.SelectedIndex = index;
                }
            }
        }

        /// <summary>
        /// Return the index of the given status name in the data source
        /// </summary>
        private static int FindIndex(IList<KeyValuePair<string, object>> dataSource, string statusNameToFind)
        {
            KeyValuePair<string, object> kvp;
            int index = StatusNotFound;

            kvp = dataSource.FirstOrDefault(kv => kv.Key.Equals(statusNameToFind, StringComparison.InvariantCultureIgnoreCase));

            if (!kvp.Equals(default(KeyValuePair<string, object>)))
            {
                index = dataSource.IndexOf(kvp);
            }

            return index;
        }
    }
}
