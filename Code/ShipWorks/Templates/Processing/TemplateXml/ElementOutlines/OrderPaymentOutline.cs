using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Stores.Content;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for the order payment node
    /// </summary>
    public class OrderPaymentOutline : ElementOutline
    {
        long orderID;
        List<OrderPaymentDetailEntity> details;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderPaymentOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddElement("CreditCard", CreateCreditCardOutline(context), If(() => CreditCardNumber.Length > 0));
            AddElement("Detail", new OrderPaymentDetailOutline(context), () => Details);
        }

        /// <summary>
        /// Create the outline to use for our "CreditCard" element
        /// </summary>
        private ElementOutline CreateCreditCardOutline(TemplateTranslationContext context)
        {
            ElementOutline outline = new ElementOutline(context);
            outline.AddElement("Number", () => CreditCardNumber );
            outline.AddElement("SecureNumber", () => PaymentDetailSecurity.MaskCreditCardNumber(CreditCardNumber));
            outline.AddElement("Expiration", () => CreditCardExpiration );

            return outline;
        }

        /// <summary>
        /// Retrieves the credit card number from the detail set (if any)
        /// </summary>
        private string CreditCardNumber
        {
            get
            {
                return PaymentDetailSecurity.GetCreditCardNumber(Details);
            }
        }

        /// <summary>
        /// Retrieves the credit card expiration from the detail set (if any)
        /// </summary>
        private string CreditCardExpiration
        {
            get
            {
                return PaymentDetailSecurity.GetExpiration(Details);
            }
        }

        /// <summary>
        /// Bind a new cloned instance to a specific orderID
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new OrderPaymentOutline(Context) { orderID = (long) data };
        }

        /// <summary>
        /// The lazy-loaded set of detail entities
        /// </summary>
        private List<OrderPaymentDetailEntity> Details
        {
            get
            {
                if (details == null)
                {
                    details = DataProvider.GetRelatedEntities(orderID, EntityType.OrderPaymentDetailEntity).Cast<OrderPaymentDetailEntity>().ToList();
                }

                return details;
            }
        }
    }
}
