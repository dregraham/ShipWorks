using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Templates.Controls.DefaultPickListTemplate;

namespace ShipWorks.UI.Dialogs.DefaultPickListTemplate
{
    /// <summary>
    /// Interaction logic for DefaultPickListTemplateDialog.xaml
    /// </summary>
    [Component]
    public partial class DefaultPickListTemplateDialog : IDefaultPickListTemplateDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultPickListTemplateDialog(IWin32Window owner, IDefaultPickListTemplateDialogViewModel viewModel) :
            base(owner, viewModel, false)
        {
            InitializeComponent();
        }
    }
}
