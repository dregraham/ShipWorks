using System.Windows.Controls;
using ShipWorks.Products.Import;
using ShipWorks.UI;

namespace ShipWorks.Products.UI.Import
{
    /// <summary>
    /// Interaction logic for ImportSucceededStateView.xaml
    /// </summary>
    [WpfViewFor(typeof(ImportSucceededState))]
    public partial class ImportSucceededStateView : UserControl
    {
        public ImportSucceededStateView()
        {
            InitializeComponent();
        }
    }
}
