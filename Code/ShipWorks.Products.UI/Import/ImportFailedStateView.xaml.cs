using System.Windows.Controls;
using ShipWorks.Products.Import;
using ShipWorks.UI;

namespace ShipWorks.Products.UI.Import
{
    /// <summary>
    /// Interaction logic for ImportFailedStateView.xaml
    /// </summary>
    [WpfViewFor(typeof(ImportFailedState))]
    public partial class ImportFailedStateView : UserControl
    {
        public ImportFailedStateView()
        {
            InitializeComponent();
        }
    }
}
