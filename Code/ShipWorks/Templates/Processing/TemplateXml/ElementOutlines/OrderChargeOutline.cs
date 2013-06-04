using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using ShipWorks.Data;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for an order 'Charge' element
    /// </summary>
    public class OrderChargeOutline : ElementOutline
    {
        static readonly ILog log = LogManager.GetLogger(typeof(OrderChargeOutline));

        long chargeID;
        OrderChargeEntity charge;

        /// <summary>
        /// Cosntructor
        /// </summary>
        public OrderChargeOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddAttribute("ID", () => chargeID);

            AddElement("Type", () => Charge.Type);
            AddElement("Description", () => Charge.Description);
            AddElement("Amount", () => Charge.Amount);
        }

        /// <summary>
        /// Bind a cloned instance to the specified chargeID
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new OrderChargeOutline(Context) { chargeID = (long) data };
        }

        /// <summary>
        /// The OrderChargeEntity represented by the bound outline
        /// </summary>
        private OrderChargeEntity Charge
        {
            get
            {
                if (charge == null)
                {
                    charge = (OrderChargeEntity) DataProvider.GetEntity(chargeID);
                    if (charge == null)
                    {
                        log.WarnFormat("Order charge {0} was deleted and cannot be processed by template.", chargeID);
                        throw new TemplateProcessException("An order  charge has been deleted.");
                    }
                }

                return charge;
            }
        }

    }
}
