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
using ShipWorks.Settings;

namespace ShipWorks.Users
{
    /// <summary>
    /// User control to select a UI Mode
    /// </summary>
    public partial class UIModeSelectionControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UIModeSelectionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads UI mode from user settings
        /// </summary>
        public void LoadFrom(UserSettingsEntity settings)
        {
            batch.Checked = settings.UIMode == UIMode.Batch;
            orderLookup.Checked = settings.UIMode == UIMode.OrderLookup;
        }

        /// <summary>
        /// Saves user input to settings entity
        /// </summary>
        public void SaveTo(UserSettingsEntity settings)
        {
            settings.UIMode = batch.Checked ? UIMode.Batch : UIMode.OrderLookup;
        }
    }
}
