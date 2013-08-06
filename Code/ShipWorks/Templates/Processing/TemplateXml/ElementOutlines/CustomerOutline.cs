using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using log4net;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for the 'Customer' node
    /// </summary>
    public class CustomerOutline : ElementOutline
    {
        static readonly ILog log = LogManager.GetLogger(typeof(CustomerOutline));

        long customerID;
        CustomerEntity customer;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddAttribute("ID", () => customerID);

            var summaryOutline = AddElement("Summary");
            summaryOutline.AddElement("TotalSpent", () => Customer.RollupOrderTotal);
            summaryOutline.AddElement("OrdersPlaced", () => Customer.RollupOrderCount);

            AddElement("Address", new AddressOutline(context, "ship", true), () => new PersonAdapter(Customer, "Ship"));
            AddElement("Address", new AddressOutline(context, "bill", true), () => new PersonAdapter(Customer, "Bill"));

            AddElement("Order", new OrderOutline(context), () => context.Input.GetOrderKeys(customerID));

            AddElement("Note", new NoteOutline(context), () => DataProvider.GetRelatedKeys(customerID, EntityType.NoteEntity));
            AddElement("Notes", NoteOutline.CreateLegacy2xNotesOutline(context, () => customerID));
        }

        /// <summary>
        /// Create a clone of the outline, bound to the given data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            // Used for progress tracking
            Context.CustomerProcessed();

            return new CustomerOutline(Context) { customerID = (long) data };
        }

        /// <summary>
        /// The CustomerEntity represented by the bound outline
        /// </summary>
        private CustomerEntity Customer
        {
            get
            {
                if (customer == null)
                {
                    customer = (CustomerEntity) DataProvider.GetEntity(customerID);
                    if (customer == null)
                    {
                        log.WarnFormat("Customer {0} was deleted and cannot be processed by template.", customerID);
                        throw new TemplateProcessException("A customer has been deleted.");
                    }
                }

                return customer;
            }
        }
    }
}
