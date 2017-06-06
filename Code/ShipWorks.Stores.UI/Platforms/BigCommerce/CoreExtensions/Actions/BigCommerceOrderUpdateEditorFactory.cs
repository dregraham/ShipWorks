using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Actions.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Stores.Platforms.BigCommerce.CoreExtensions.Actions;

namespace ShipWorks.Stores.UI.Platforms.BigCommerce.CoreExtensions.Actions
{
    /// <summary>
    /// Create an order update editor for Big Commerce
    /// </summary>
    [Component]
    public class BigCommerceOrderUpdateEditorFactory : IBigCommerceOrderUpdateEditorFactory
    {
        /// <summary>
        /// Create an action task editor
        /// </summary>
        public ActionTaskEditor Create(BigCommerceOrderUpdateTask task) =>
            new BigCommerceOrderUpdateTaskEditor(task);
    }
}
