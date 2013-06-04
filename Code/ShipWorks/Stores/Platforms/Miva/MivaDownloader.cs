using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.XPath;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Miva
{
    /// <summary>
    /// Miva Downloader that overrides some base generic functionality
    /// </summary>
    class MivaDownloader : GenericModuleDownloader
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MivaDownloader(MivaStoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Populate custom Miva fields
        /// </summary>
        public override void OnItemAttributeLoadComplete(OrderItemAttributeEntity item, XPathNavigator xpath)
        {
            MivaOrderItemAttributeEntity mivaOption = item as MivaOrderItemAttributeEntity;
            if (mivaOption == null)
            {
                throw new InvalidOperationException("OnOptionLoadComplete was expecting a MivaOrderItemAttributeEntity");
            }

            mivaOption.MivaAttributeID = XPathUtility.Evaluate(xpath, "AttributeID", 0);
            mivaOption.MivaAttributeCode = XPathUtility.Evaluate(xpath, "Debug/MivaAttributeCode", "");
            mivaOption.MivaOptionCode = XPathUtility.Evaluate(xpath, "Debug/MivaOptionCode", "");
        }

        /// <summary>
        /// The order is about to be saved.  Xpath provides access to the source xml for the order.
        /// </summary>
        public override void OnOrderLoadComplete(OrderEntity order, XPathNavigator xpath)
        {
            if (order.IsNew)
            {
                // See if we can find the shipping charge
                OrderChargeEntity shippingCharge = order.OrderCharges.FirstOrDefault(c => c.Type == "SHIPPING");
                if (shippingCharge != null)
                {
                    order.RequestedShipping = shippingCharge.Description.Replace("Shipping:", "").Trim();
                }
            }
        }
    }
}
