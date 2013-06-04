using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for the detail entry of an order payment
    /// </summary>
    public class OrderPaymentDetailOutline : ElementOutline
    {
        OrderPaymentDetailEntity detail;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderPaymentDetailOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddAttribute("ID", () => detail.OrderPaymentDetailID);

            AddElement("Label", () => detail.Label);
            AddElement("Value", () => PaymentDetailSecurity.ReadValue(detail));
        }

        /// <summary>
        /// Bind a new cloned instance to given specific data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new OrderPaymentDetailOutline(Context) { detail = (OrderPaymentDetailEntity) data };
        }
    }
}
