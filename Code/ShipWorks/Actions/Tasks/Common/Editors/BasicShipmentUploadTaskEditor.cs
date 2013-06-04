using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    /// <summary>
    /// Task editor that can be used for any task that uploads shipment details without requiring any input
    /// </summary>
    public partial class BasicShipmentUploadTaskEditor : ActionTaskEditor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BasicShipmentUploadTaskEditor()
        {
            InitializeComponent();
        }
    }
}
