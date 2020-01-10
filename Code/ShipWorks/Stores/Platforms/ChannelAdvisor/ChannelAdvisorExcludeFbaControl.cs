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

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Control to update FBA control
    /// </summary>
    public partial class ChannelAdvisorExcludeFbaControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorExcludeFbaControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the store
        /// </summary>
        public void LoadStore(ChannelAdvisorStoreEntity store)
        {
            excludeFba.Checked = store.ExcludeFBA;
        }

        /// <summary>
        /// Save the store
        /// </summary>
        public void SaveToEntity(ChannelAdvisorStoreEntity store)
        {
            store.ExcludeFBA = excludeFba.Checked;
        }
    }
}
