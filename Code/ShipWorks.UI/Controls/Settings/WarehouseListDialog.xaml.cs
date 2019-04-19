using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Settings.Warehouse;

namespace ShipWorks.UI.Controls.Settings
{
    /// <summary>
    /// Interaction logic for WarehouseListDialog.xaml
    /// </summary>
    [Component(RegisterAs = RegistrationType.SpecificService, Service = typeof(IWarehouseListDialog))]
    public partial class WarehouseListDialog : Window, IWarehouseListDialog
    {
        public WarehouseListDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set the owner of this window
        /// </summary>
        [SuppressMessage("SonarQube", "S1848:Objects should not be created to be dropped immediately without being used",
            Justification = "The interop helper is only used temporarily to set this window's owner")]
        [SuppressMessage("Recommendations", "RECS0026:Objects should not be created to be dropped immediately without being used",
            Justification = "The interop helper is only used temporarily to set this window's owner")]
        public void LoadOwner(System.Windows.Forms.IWin32Window owner)
        {
            new WindowInteropHelper(this)
            {
                Owner = owner.Handle
            };
        }
    }
}
