using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShipWorks.UI
{
    /// <summary>
    /// Proxy for binding
    /// </summary>
    /// <remarks>
    /// This is to make binding work with obfuscation in places where the element to be bound
    /// is not in the normal visual tree, like a DataGridColumn's visibility property.
    /// </remarks>
    public class BindingProxy : Freezable
    {
        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        /// <summary>
        /// Create an instance of the proxy
        /// </summary>
        protected override Freezable CreateInstanceCore() => new BindingProxy();

        /// <summary>
        /// Data to be used as the binding proxy
        /// </summary>
        public object Data
        {
            get { return (object) GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
    }
}
