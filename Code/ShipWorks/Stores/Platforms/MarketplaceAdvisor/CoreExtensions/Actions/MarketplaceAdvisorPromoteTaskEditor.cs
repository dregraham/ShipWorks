using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.CoreExtensions.Actions
{
    /// <summary>
    /// Task editor for MarketplaceAdvisor promote orders and parcels tasks
    /// </summary>
    public partial class MarketplaceAdvisorPromoteTaskEditor : ActionTaskEditor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorPromoteTaskEditor(string orderOrParcel)
        {
            InitializeComponent();

            labelInfo.Text = string.Format(labelInfo.Text, orderOrParcel);
        }
    }
}
