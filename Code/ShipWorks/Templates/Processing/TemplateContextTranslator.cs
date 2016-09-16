using System;
using System.Collections.Generic;
using log4net;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Translates a single entity into one or more entities based on a desired context.
    /// </summary>
    public static class TemplateContextTranslator
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(TemplateContextTranslator));

        /// <summary>
        /// Translates a single entity into one or more entities based on a desired context.
        /// </summary>
        public static List<TemplateInput> Translate(List<long> idList, TemplateEntity template)
        {
            if (idList.Count == 0)
            {
                return new List<TemplateInput>();
            }

            TemplateType templateType = (TemplateType) template.Type;
            TemplateInputContext context = (TemplateInputContext) template.Context;

            // If the context is automatic, we need to figure it out based on the entity
            if (context == TemplateInputContext.Auto)
            {
                context = ResolveContextFromEntityType(EntityUtility.GetEntityType(idList[0]));
            }

            // Reports have a single input that is everything
            if (templateType == TemplateType.Report)
            {
                // Get the context keys for all the original inputs
                List<long> contextKeys = DetermineContextKeys(idList, context);

                return new List<TemplateInput> { new TemplateInput(idList, contextKeys, context) };
            }
            else
            {
                List<TemplateInput> inputs = new List<TemplateInput>();

                // For no report templates, we generate one input per translated context ID
                foreach (long originalKey in idList)
                {
                    // Get the context keys for this specific original key
                    List<long> contextKeys = DetermineContextKeys(new List<long> { originalKey }, context);

                    // Create an input for each context key
                    foreach (long contextKey in contextKeys)
                    {
                        inputs.Add(new TemplateInput(new List<long> { originalKey }, new List<long> { contextKey }, context));
                    }
                }

                return inputs;
            }
        }

        /// <summary>
        /// Translate the given ID list into a set of keys based on the specified context
        /// </summary>
        private static List<long> DetermineContextKeys(List<long> idList, TemplateInputContext context)
        {
            return DataProvider.GetRelatedKeys(idList, GetContextEntityType(context));
        }

        /// <summary>
        /// Return what context type to use based on the given entity type
        /// </summary>
        public static TemplateInputContext ResolveContextFromEntityType(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.CustomerEntity:
                    return TemplateInputContext.Customer;

                case EntityType.OrderEntity:
                    return TemplateInputContext.Order;

                case EntityType.OrderItemEntity:
                    return TemplateInputContext.OrderItem;

                case EntityType.ShipmentEntity:
                    return TemplateInputContext.Shipment;
            }

            throw new InvalidOperationException(string.Format("Invalid EntityType {0}", entityType));
        }

        /// <summary>
        /// Get the target entity type of the specified context.
        /// </summary>
        private static EntityType GetContextEntityType(TemplateInputContext context)
        {
            switch (context)
            {
                case TemplateInputContext.Customer:
                    return EntityType.CustomerEntity;

                case TemplateInputContext.Order:
                    return EntityType.OrderEntity;

                case TemplateInputContext.OrderItem:
                    return EntityType.OrderItemEntity;

                case TemplateInputContext.Shipment:
                    return EntityType.ShipmentEntity;

            }

            throw new InvalidOperationException(string.Format("Invalid TemplateContext {0}", context));
        }
    }
}
