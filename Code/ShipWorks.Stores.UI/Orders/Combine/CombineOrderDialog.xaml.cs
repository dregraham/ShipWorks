﻿using System.Diagnostics.CodeAnalysis;
using System.Windows.Interop;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Orders.Combine;

namespace ShipWorks.Stores.UI.Orders.Combine
{
    /// <summary>
    /// Interaction logic for CombineOrdersDialog.xaml
    /// </summary>
    [Component]
    public partial class CombineOrdersDialog : ICombineOrderDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CombineOrdersDialog()
        {
            InitializeComponent();

            WindowStateSaver.Manage(this);
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
