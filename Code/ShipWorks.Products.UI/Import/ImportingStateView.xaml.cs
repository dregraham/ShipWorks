using System.Windows.Controls;
using ShipWorks.Products.Import;
using ShipWorks.UI;

namespace ShipWorks.Products.UI.Import
{
    /// <summary>
    /// Interaction logic for ImportingStateView.xaml
    /// </summary>
    [WpfViewFor(typeof(ImportingState))]
    public partial class ImportingStateView : UserControl
    {
        public ImportingStateView()
        {
            InitializeComponent();
        }
    }
}
