using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.XPath;

namespace ShipWorks.Data.Import.Xml
{
    /// <summary>
    /// Interface for receiving callbacks during the GenericXmlOrderLoader operations
    /// </summary>
    public interface IGenericXmlOrderLoadObserver
    {
        /// <summary>
        /// Order has been loaded
        /// </summary>
        void OnOrderLoadComplete(OrderEntity order, XPathNavigator xpath);

        /// <summary>
        /// Order item has been loaded
        /// </summary>
        void OnItemLoadComplete(OrderItemEntity item, XPathNavigator xpath);

        /// <summary>
        /// Item Option has been loaded
        /// </summary>
        void OnItemAttributeLoadComplete(OrderItemAttributeEntity item, XPathNavigator xpath);
    }
}
