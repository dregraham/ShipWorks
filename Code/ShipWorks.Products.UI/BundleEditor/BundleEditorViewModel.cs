using System.Collections.Generic;
using System.Windows.Input;

namespace ShipWorks.Products.UI.BundleEditor
{
    public class BundleEditorViewModel
    {
        public string Sku { get; set; }
        
        public int Quantity { get; set; }
        
        public IEnumerable<string> BundledSkus { get; set; }
        
        public string SelectedSku { get; set; }
        
        public ICommand AddCommand { get; }
        
        public ICommand RemoveCommand { get; }
    }
}
