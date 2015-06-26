using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Filters;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Filters.Search;
using ShipWorks.Shipping.Settings;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Users.Audit;
using ShipWorks.Filters.Content.Conditions.Orders.Address;
using ShipWorks.Filters.Content.Conditions.OrderCharges;
using ShipWorks.Filters.Content.Conditions.Customers;
using ShipWorks.Templates;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Loads a fresh ShipWorks database with default data
    /// </summary>
    public static class InitialDataLoader
    {
        /// <summary>
        /// Create the core data required to run ShipWorks at all
        /// </summary>
        public static void CreateCoreRequiredData()
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                ConfigurationData.CreateInstance(adapter);
                SystemData.CreateInstance(adapter);
                ShippingSettings.CreateInstance(adapter);
            }

            FilterNodeEntity ordersNode = CreateTopLevelFilter(FilterTarget.Orders);
            FilterNodeEntity customersNode = CreateTopLevelFilter(FilterTarget.Customers);

            SearchManager.CreateSearchPlaceholder(FilterTarget.Orders);
            SearchManager.CreateSearchPlaceholder(FilterTarget.Customers);

            CreateBuiltinTemplateFolders();
        }

        /// <summary>
        /// Create the default data for a fresh install.  This is like status presets and example filters
        /// </summary>
        public static void CreateDefaultFreshInstallData()
        {
            // Before we can create the custom filters, we need to get the FilterLayoutContext initialized, since that is what all filters
            // are created through.  We don't need (and can't have, since there are no real users yet) the "My Layout" stuff, so put the SuperUser
            // in scope to prevent that.
            using (AuditBehaviorScope scope = new AuditBehaviorScope(AuditBehaviorUser.SuperUser, new AuditReason(AuditReasonType.Default), AuditState.Disabled))
            {
                CreateStatusPresets();

                try
                {
                    // We need to push a new scope for the layout context, b\c if the user ends up cancelling the wizard, it needs to be restored to the
                    // way it was.  And if it doesnt, the layout context gets reloaded anyway.
                    FilterLayoutContext.PushScope();

                    CreateOrderFilters(FilterLayoutContext.Current.FindNode(BuiltinFilter.GetTopLevelKey(FilterTarget.Orders)));
                    CreateCustomerFilters(FilterLayoutContext.Current.FindNode(BuiltinFilter.GetTopLevelKey(FilterTarget.Customers)));
                }
                finally
                {
                    FilterLayoutContext.PopScope();
                }
            }
        }

        /// <summary>
        /// Create the default set of order filters
        /// </summary>
        private static void CreateOrderFilters(FilterNodeEntity ordersNode)
        {
            FilterNodeEntity examplesNode = FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterFolderEntity("Examples", FilterTarget.Orders), ordersNode, 0)[0];
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Today's Orders", CreateDefinitionTodaysOrders()), examplesNode, 0);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("International", CreateDefinitionInternational()), examplesNode, 1);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Has Tax", CreateDefinitionHasTax()), examplesNode, 2);

            FilterNodeEntity addressValidationNode = FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterFolderEntity("Address Validation", FilterTarget.Orders), examplesNode, 3)[0];
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Ready to Go", FilterHelper.CreateAddressValidationDefinition(AddressSelector.ReadyToShip)), addressValidationNode, 0);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Not Validated", FilterHelper.CreateAddressValidationDefinition(AddressSelector.NotValidated)), addressValidationNode, 0);
        }

        /// <summary>
        /// Create the default set of customer filters
        /// </summary>
        private static void CreateCustomerFilters(FilterNodeEntity customersNode)
        {
            FilterNodeEntity examplesNode = FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterFolderEntity("Examples", FilterTarget.Customers), customersNode, 0)[0];
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Spent $100 or more", CreateDefinitionSpent100()), examplesNode, 0);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Returning customer", CreateDefinitionMultipleOrders()), examplesNode, 1);
        }

        /// <summary>
        /// Create the definition of the "Not Shipped" filter
        /// </summary>
        public static FilterDefinition CreateDefinitionNotShipped()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);

            // Local Status != 'Shipped'
            LocalStatusCondition statusCondition = new LocalStatusCondition();
            statusCondition.Operator = StringOperator.NotEqual;
            statusCondition.TargetValue = "Shipped";
            definition.RootContainer.FirstGroup.Conditions.Add(statusCondition);

            // [And]
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;

            // If [None] of the following
            definition.RootContainer.SecondGroup = new ConditionGroupContainer(new ConditionGroup());
            definition.RootContainer.SecondGroup.FirstGroup.JoinType = ConditionJoinType.None;

            // For any shipment, Processed = true
            ForAnyShipmentCondition anyShipment = new ForAnyShipmentCondition();
            ShipmentStatusCondition processedCondition = new ShipmentStatusCondition();
            processedCondition.Operator = EqualityOperator.Equals;
            processedCondition.Value = ShipmentStatusType.Processed;
            anyShipment.Container.FirstGroup.Conditions.Add(processedCondition);
            definition.RootContainer.SecondGroup.FirstGroup.Conditions.Add(anyShipment);

            return definition;
        }

        /// <summary>
        /// Create the filter definition for today's orders
        /// </summary>
        private static FilterDefinition CreateDefinitionTodaysOrders()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);

            OrderDateCondition dateCondition = new OrderDateCondition();
            dateCondition.Operator = DateOperator.Today;
            definition.RootContainer.FirstGroup.Conditions.Add(dateCondition);

            return definition;
        }

        /// <summary>
        /// Create the filter definition for international orders
        /// </summary>
        private static FilterDefinition CreateDefinitionInternational()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);

            OrderAddressCountryCondition countryCondition = new OrderAddressCountryCondition();
            countryCondition.AddressOperator = BillShipAddressOperator.Ship;
            countryCondition.Operator = StringOperator.NotEqual;
            countryCondition.TargetValue = "US";
            definition.RootContainer.FirstGroup.Conditions.Add(countryCondition);

            return definition;
        }

        /// <summary>
        /// Create the filter definition for orders that have multiple line items
        /// </summary>
        private static FilterDefinition CreateDefinitionHasTax()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);

            ForAnyChargeCondition anyCharge = new ForAnyChargeCondition();
            anyCharge.Container.FirstGroup.JoinType = ConditionJoinType.All;
            definition.RootContainer.FirstGroup.Conditions.Add(anyCharge);

            OrderChargeTypeCondition chargeType = new OrderChargeTypeCondition();
            chargeType.Operator = StringOperator.Contains;
            chargeType.TargetValue = "TAX";
            anyCharge.Container.FirstGroup.Conditions.Add(chargeType);

            OrderChargeAmountCondition chargeAmount = new OrderChargeAmountCondition();
            chargeAmount.Operator = NumericOperator.GreaterThan;
            chargeAmount.Value1 = 0;
            anyCharge.Container.FirstGroup.Conditions.Add(chargeAmount);

            return definition;
        }

        /// <summary>
        /// Create the definition for a customer that's spent $100 or more
        /// </summary>
        private static FilterDefinition CreateDefinitionSpent100()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Customers);

            AmountSpentCondition spentCondition = new AmountSpentCondition();
            spentCondition.Value1 = 100;
            spentCondition.Operator = NumericOperator.GreaterThanOrEqual;
            definition.RootContainer.FirstGroup.Conditions.Add(spentCondition);

            return definition;
        }

        /// <summary>
        /// Create the definition for a customer who has placed more than one order
        /// </summary>
        private static FilterDefinition CreateDefinitionMultipleOrders()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Customers);

            OrderCountCondition countCondition = new OrderCountCondition();
            countCondition.Operator = NumericOperator.GreaterThanOrEqual;
            countCondition.Value1 = 2;
            definition.RootContainer.FirstGroup.Conditions.Add(countCondition);

            return definition;
        }

        /// <summary>
        /// Create the default status presets
        /// </summary>
        private static void CreateStatusPresets()
        {
            string[] orderPresets = new string[]
            {
                "New Order",
                "Not Shipped",
                "Shipped",
                "In Process",
                "Backordered",
                "Canceled"
            };

            string[] itemPresets = new string[]
            {
                "In Stock",
                "Not Shipped",
                "Shipped",
                "Backordered",
                "Out of Stock",
                "Packed",
                "Not Packed"
            };

            using (SqlAdapter adapter = new SqlAdapter())
            {
                foreach (string text in orderPresets)
                {
                    StatusPresetEntity preset = new StatusPresetEntity();
                    preset.StoreID = null;
                    preset.StatusTarget = (int) StatusPresetTarget.Order;
                    preset.StatusText = text;
                    preset.IsDefault = false;

                    adapter.SaveEntity(preset);
                }

                foreach (string text in itemPresets)
                {
                    StatusPresetEntity preset = new StatusPresetEntity();
                    preset.StoreID = null;
                    preset.StatusTarget = (int) StatusPresetTarget.OrderItem;
                    preset.StatusText = text;
                    preset.IsDefault = false;

                    adapter.SaveEntity(preset);
                }
            }
        }

        /// <summary>
        /// Create the never-changing top-level filter for the given target
        /// </summary>
        private static FilterNodeEntity CreateTopLevelFilter(FilterTarget target)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                // We will be specifying the pk values
                adapter.IdentityInsert = true;

                try
                {
                    long pkValue = BuiltinFilter.GetTopLevelKey(target);

                    FilterEntity filter = new FilterEntity();
                    filter.FilterID = pkValue;
                    filter.Name = EnumHelper.GetDescription(target);
                    filter.FilterTarget = (int) target;
                    filter.IsFolder = true;
                    filter.Definition = null;
                    filter.State = (int)FilterState.Enabled;
                    adapter.SaveAndRefetch(filter);

                    FilterSequenceEntity sequence = new FilterSequenceEntity();
                    sequence.FilterSequenceID = pkValue;
                    sequence.Parent = null;
                    sequence.Filter = filter;
                    sequence.Position = 0;
                    adapter.SaveAndRefetch(sequence);

                    FilterNodeContentEntity content = new FilterNodeContentEntity();
                    content.FilterNodeContentID = pkValue;
                    content.Count = 0;
                    content.CountVersion = 0;
                    content.Cost = 0;
                    content.Status = (int) FilterCountStatus.Ready;
                    content.InitialCalculation = "";
                    content.UpdateCalculation = "";
                    content.ColumnMask = new byte[0];
                    content.JoinMask = 0;
                    adapter.SaveAndRefetch(content);

                    FilterNodeEntity node = new FilterNodeEntity();
                    node.FilterNodeID = pkValue;
                    node.ParentNode = null;
                    node.FilterSequence = sequence;
                    node.FilterNodeContent = content;
                    node.Created = DateTime.UtcNow;
                    node.Purpose = (int) FilterNodePurpose.Standard;
                    adapter.SaveAndRefetch(node);

                    FilterLayoutEntity layout = new FilterLayoutEntity();
                    layout.FilterLayoutID = pkValue;
                    layout.User = null;
                    layout.FilterTarget = (int) target;
                    layout.FilterNode = node;
                    adapter.SaveAndRefetch(layout);

                    return node;
                }
                finally
                {
                    // Get out of identity mode
                    adapter.IdentityInsert = false;
                }
            }
        }


        /// <summary>
        /// Create the set of builtin template folders
        /// </summary>
        private static void CreateBuiltinTemplateFolders()
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                // We will be specifying the pk values
                adapter.IdentityInsert = true;

                TemplateFolderEntity system = new TemplateFolderEntity();
                system.TemplateFolderID = TemplateBuiltinFolders.SystemFolderID;
                system.Name = "System";
                system.ParentFolderID = null;

                adapter.SaveEntity(system);

                TemplateFolderEntity snippets = new TemplateFolderEntity();
                snippets.TemplateFolderID = TemplateBuiltinFolders.SnippetsFolderID;
                snippets.Name = "Snippets";
                snippets.ParentFolderID = TemplateBuiltinFolders.SystemFolderID;

                adapter.SaveEntity(snippets);
            }
        }
    }
}
