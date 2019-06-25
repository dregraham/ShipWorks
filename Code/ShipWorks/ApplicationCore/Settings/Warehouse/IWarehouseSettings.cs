using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.ApplicationCore.Settings.Warehouse
{
    /// <summary>
    /// Warehouse settings
    /// </summary>
    public interface IWarehouseSettings
    {
        /// <summary>
        /// Gets the control associated with the warehouse settings
        /// </summary>
        Control Control { get;  }

        /// <summary>
        /// View model associated with the control
        /// </summary>
        IWarehouseSettingsViewModel ViewModel { get; set; }
    }
}
