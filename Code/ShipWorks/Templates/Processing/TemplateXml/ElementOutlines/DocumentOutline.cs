using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Xml;
using System.Linq;
using Interapptive.Shared;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Users;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Model;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Top-level document "ShipWorks" element of the output
    /// </summary>
    public class DocumentOutline : ElementOutline
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DocumentOutline(TemplateTranslationContext context) 
            : base(context)
        {

            AddElement("Generated", DateTime.UtcNow);
            AddElement("Timestamp", DateTimeUtility.ToUnixTimestamp(DateTime.UtcNow));
            AddElement("UniqueID", Guid.NewGuid().ToString("B"));
            AddElement("Context", EnumHelper.GetDescription(context.Input.ContextType));
            AddElement("Reference", CreateLegacy2xReferenceOutline(context));
            
            // Add Template node
            if (context.Template != null)
            {
                AddElement("Template", new TemplateOutline(context));
            }

            // Add User node
            AddElement("User", new UserOutline(context), () => UserSession.User);

            // Store node(s)
            AddElement("Store", new StoreOutline(context), context.Input.GetStores());

            // Customer node(s)
            AddElement("Customer", new CustomerOutline(context), context.Input.GetCustomerKeys());
        }

        /// <summary>
        /// Create the outline for the legacy "Reference" element
        /// </summary>
        [NDependIgnoreLongMethod]
        private static ElementOutline CreateLegacy2xReferenceOutline(TemplateTranslationContext context)
        {
            ElementOutline outline = new ElementOutline(context);
            outline.AddAttributeLegacy2x();
            outline.AddTextContent(() =>
                {
                    TemplateInput input = context.Input;
                    TemplateEntity template = context.Template;

                    if (template != null && template.Type == (int) TemplateType.Report)
                    {
                        return "Report";
                    }

                    if (input.ContextKeys.Count == 0)
                    {
                        return "None";
                    }

                    long contextKey = input.ContextKeys[0];

                    // Shipment always returns shipment
                    if (input.ContextType == TemplateInputContext.Shipment)
                    {
                        return "Shipment";
                    }

                    // For customer
                    if (input.ContextType == TemplateInputContext.Customer)
                    {
                        return string.Format("Customer {0}", contextKey);
                    }

                    // For order, a little more complicated
                    if (input.ContextType == TemplateInputContext.Order)
                    {
                        OrderEntity order = (OrderEntity) DataProvider.GetEntity(contextKey);
                        if (order != null)
                        {
                            EbayOrderEntity eBayOrder = order as EbayOrderEntity;
                            if (eBayOrder != null)
                            {
                                List<OrderItemEntity> items = DataProvider.GetRelatedEntities(contextKey, EntityType.OrderItemEntity).Cast<OrderItemEntity>().ToList();

                                if (items.Any(i => !i.IsManual))
                                {
                                    StringBuilder sb = new StringBuilder();
                                    foreach (EbayOrderItemEntity item in items.OfType<EbayOrderItemEntity>())
                                    {
                                        if (sb.Length > 0)
                                        {
                                            sb.Append(", ");
                                        }

                                        sb.AppendFormat("{0}: {1}", item.EbayItemID, item.Name);
                                    }

                                    return sb.ToString();
                                }
                            }

                            return string.Format("Order {0}", order.OrderNumberComplete);
                        }
                    }

                    return "None";
                });

            return outline;
        }
    }
}
