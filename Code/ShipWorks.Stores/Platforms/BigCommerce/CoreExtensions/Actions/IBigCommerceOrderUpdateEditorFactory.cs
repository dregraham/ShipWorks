using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Stores.Platforms.BigCommerce.CoreExtensions.Actions
{
    /// <summary>
    /// Create an order update editor for Big Commerce
    /// </summary>
    public interface IBigCommerceOrderUpdateEditorFactory
    {
        /// <summary>
        /// Create an action task editor
        /// </summary>
        ActionTaskEditor Create(BigCommerceOrderUpdateTask bigCommerceOrderUpdateTask);
    }
}
