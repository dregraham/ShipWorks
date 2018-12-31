using System.Windows.Controls;
using ShipWorks.Products.Import;
using ShipWorks.UI;

namespace ShipWorks.Products.UI.Import
{
    /// <summary>
    /// Interaction logic for SetupImportStateView.xaml
    /// </summary>
    [WpfViewFor(typeof(SetupImportState))]
    public partial class SetupImportStateView : UserControl
    {
        public SetupImportStateView()
        {
            InitializeComponent();
        }
    }
}
