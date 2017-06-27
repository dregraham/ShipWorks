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
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Content.Controls;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Stores.UI.Content
{
    /// <summary>
    /// Interaction logic for CombineOrdersDialog.xaml
    /// </summary>
    [Component]
    public partial class CombineOrdersDialog : Window, ICombineOrdersDialog
    {
        private readonly Func<IEnumerable<IOrderEntity>, ICombineOrdersViewModel> createViewModel;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public CombineOrdersDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CombineOrdersDialog(IMessageHelper messageHelper, Func<IEnumerable<IOrderEntity>, ICombineOrdersViewModel> createViewModel) : this()
        {
            this.messageHelper = messageHelper;
            this.createViewModel = createViewModel;
        }

        /// <summary>
        /// Get order combination details from user
        /// </summary>
        public GenericResult<Tuple<long, string>> GetCombinationDetailsFromUser(IEnumerable<IOrderEntity> orders)
        {
            ICombineOrdersViewModel viewModel = createViewModel(orders);
            DataContext = viewModel;

            return messageHelper.ShowDialog(this) == true ?
                GenericResult.FromSuccess(viewModel.Details) :
                GenericResult.FromError<Tuple<long, string>>("Canceled");
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
