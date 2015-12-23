using System;
using System.Collections.Generic;
using Interapptive.Shared;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Customers;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Filters.Content.Conditions.Orders.Address;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Filters.Search;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores;
using ShipWorks.Templates;
using ShipWorks.Users.Audit;

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
        public static void CreateCoreRequiredData(Func<bool, SqlAdapter> createSqlAdapter)
        {
            using (SqlAdapter adapter = createSqlAdapter(false))
            {
                ConfigurationData.CreateInstance(adapter);
                SystemData.CreateInstance(adapter);
                ShippingSettings.CreateInstance(adapter);
            }

            FilterNodeEntity ordersNode = CreateTopLevelFilter(FilterTarget.Orders, createSqlAdapter);
            FilterNodeEntity customersNode = CreateTopLevelFilter(FilterTarget.Customers, createSqlAdapter);

            SearchManager.CreateSearchPlaceholder(FilterTarget.Orders, createSqlAdapter);
            SearchManager.CreateSearchPlaceholder(FilterTarget.Customers, createSqlAdapter);

            CreateBuiltinTemplateFolders(createSqlAdapter);
        }

        /// <summary>
        /// Create the default data for a fresh install.  This is like status presets and example filters
        /// </summary>
        public static void CreateDefaultFreshInstallData(Func<bool, SqlAdapter> createSqlAdapter)
        {
            // Before we can create the custom filters, we need to get the FilterLayoutContext initialized, since that is what all filters
            // are created through.  We don't need (and can't have, since there are no real users yet) the "My Layout" stuff, so put the SuperUser
            // in scope to prevent that.
            using (AuditBehaviorScope scope = new AuditBehaviorScope(AuditBehaviorUser.SuperUser, new AuditReason(AuditReasonType.Default), AuditState.Disabled))
            {
                CreateStatusPresets(createSqlAdapter);

                try
                {
                    // We need to push a new scope for the layout context, b\c if the user ends up canceling the wizard, it needs to be restored to the
                    // way it was.  And if it doesn't, the layout context gets reloaded anyway.
                    FilterLayoutContext.PushScope();

                    CreateOrderFilters(FilterLayoutContext.Current.FindNode(BuiltinFilter.GetTopLevelKey(FilterTarget.Orders), createSqlAdapter), createSqlAdapter);
                    CreateCustomerFilters(FilterLayoutContext.Current.FindNode(BuiltinFilter.GetTopLevelKey(FilterTarget.Customers), createSqlAdapter), createSqlAdapter);
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
        private static void CreateOrderFilters(FilterNodeEntity ordersNode, Func<bool, SqlAdapter> createSqlAdapter)
        {
            FilterNodeEntity destinationNode = FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterFolderEntity("Destination", FilterTarget.Orders), ordersNode, 0, createSqlAdapter)[0];
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("All U.S.", CreateDefinitionUS()), destinationNode, 0, createSqlAdapter);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("U.S. Residential", CreateDefinitionResidential(true)), destinationNode, 1, createSqlAdapter);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("U.S. Commercial", CreateDefinitionResidential(false)), destinationNode, 2, createSqlAdapter);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("U.S. PO Box", CreateDefinitionPOBox()), destinationNode, 3, createSqlAdapter);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("U.S. Territories", CreateDefinitionTerritory()), destinationNode, 4, createSqlAdapter);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("U.S. Military", CreateDefinitionMilitary()), destinationNode, 5, createSqlAdapter);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("International", CreateDefinitionInternational()), destinationNode, 6, createSqlAdapter);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Ambiguous", CreateAddressValidationDefinition(AddressValidationStatusType.HasSuggestions)), destinationNode, 7, createSqlAdapter);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Invalid", CreateAddressValidationDefinition(AddressValidationStatusType.BadAddress)), destinationNode, 8, createSqlAdapter);

            FilterNodeEntity ageNode = FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterFolderEntity("Age", FilterTarget.Orders), ordersNode, 1, createSqlAdapter)[0];
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Today", CreateDefinitionTodaysOrders()), ageNode, 0, createSqlAdapter);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Last 7 days", CreateDefinitionOrdersAge(7)), ageNode, 1, createSqlAdapter);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Last 30 days", CreateDefinitionOrdersAge(30)), ageNode, 2, createSqlAdapter);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Last 90 days", CreateDefinitionOrdersAge(90)), ageNode, 3, createSqlAdapter);
        }

        /// <summary>
        /// Create the default set of customer filters
        /// </summary>
        private static void CreateCustomerFilters(FilterNodeEntity customersNode, Func<bool, SqlAdapter> createSqlAdapter)
        {
            FilterNodeEntity examplesNode = FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterFolderEntity("Examples", FilterTarget.Customers), customersNode, 0, createSqlAdapter)[0];
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Spent $100 or more", CreateDefinitionSpent100()), examplesNode, 0, createSqlAdapter);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Returning customer", CreateDefinitionMultipleOrders()), examplesNode, 1, createSqlAdapter);
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
        /// Creates the definition shipped.
        /// </summary>
        public static FilterDefinition CreateDefinitionShipped()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);

            // Local Status = 'Shipped'
            LocalStatusCondition statusCondition = new LocalStatusCondition();
            statusCondition.Operator = StringOperator.Equals;
            statusCondition.TargetValue = "Shipped";
            definition.RootContainer.FirstGroup.Conditions.Add(statusCondition);

            // [Or]
            definition.RootContainer.JoinType = ConditionGroupJoinType.Or;

            // If [All] of the following
            definition.RootContainer.SecondGroup = new ConditionGroupContainer(new ConditionGroup());
            definition.RootContainer.SecondGroup.FirstGroup.JoinType = ConditionJoinType.All;

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
        /// Create the filter definition for that last n days
        /// </summary>
        private static FilterDefinition CreateDefinitionOrdersAge(int days)
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);

            OrderDateCondition dateCondition = new OrderDateCondition();
            dateCondition.Operator = DateOperator.WithinTheLast;
            dateCondition.WithinUnit = DateWithinUnit.Days;
            dateCondition.WithinAmount = days;

            definition.RootContainer.FirstGroup.Conditions.Add(dateCondition);

            return definition;
        }

        /// <summary>
        /// Create the filter definition for US Residential
        /// </summary>
        private static FilterDefinition CreateDefinitionResidential(bool Residential)
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);

            //Build the residential condition
            ResidentialStatusCondition residentialCondition = new ResidentialStatusCondition()
            {
                AddressOperator = BillShipAddressOperator.Ship,
                Operator = EqualityOperator.Equals,
                Value = Residential ? ValidationDetailStatusType.Yes : ValidationDetailStatusType.No
            };

            //Add it to the filter
            definition.RootContainer.FirstGroup.Conditions.Add(residentialCondition);

            //Build the condition to exclude PO Box
            POBoxCondition poCondition = new POBoxCondition()
            {
                AddressOperator = BillShipAddressOperator.Ship,
                Operator = EqualityOperator.Equals,
                Value = ValidationDetailStatusType.No,
            };

            //Add it to the filter
            definition.RootContainer.FirstGroup.Conditions.Add(poCondition);

            //Build the condition to exclude us territory addresses
            USTerritoryCondition territoryCondition = new USTerritoryCondition()
            {
                AddressOperator = BillShipAddressOperator.Ship,
                Operator = EqualityOperator.Equals,
                Value = ValidationDetailStatusType.No
            };

            //Add it to the filter
            definition.RootContainer.FirstGroup.Conditions.Add(territoryCondition);

            //Build the condition to exclude Military addresses
            MilitaryAddressCondition militaryCondition = new MilitaryAddressCondition()
            {
                AddressOperator = BillShipAddressOperator.Ship,
                Operator = EqualityOperator.Equals,
                Value = ValidationDetailStatusType.No
            };

            //Add it to the filter
            definition.RootContainer.FirstGroup.Conditions.Add(militaryCondition);

            return definition;
        }

        /// <summary>
        /// Create the filter definition for US PO BOX orders
        /// </summary>
        private static FilterDefinition CreateDefinitionPOBox()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);

            POBoxCondition poCondition = new POBoxCondition()
            {
                AddressOperator = BillShipAddressOperator.Ship,
                Operator = EqualityOperator.Equals,
                Value = ValidationDetailStatusType.Yes
            };

            definition.RootContainer.FirstGroup.Conditions.Add(poCondition);

            return definition;
        }

        /// <summary>
        /// Create the filter definition for Territory
        /// </summary>
        private static FilterDefinition CreateDefinitionTerritory()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);

            USTerritoryCondition territoryCondition = new USTerritoryCondition()
            {
                AddressOperator = BillShipAddressOperator.Ship,
                Operator = EqualityOperator.Equals,
                Value = ValidationDetailStatusType.Yes
            };

            definition.RootContainer.FirstGroup.Conditions.Add(territoryCondition);

            return definition;
        }

        /// <summary>
        /// Create the filter definition for Military
        /// </summary>
        private static FilterDefinition CreateDefinitionMilitary()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);

            MilitaryAddressCondition militaryCondition = new MilitaryAddressCondition()
            {
                AddressOperator = BillShipAddressOperator.Ship,
                Operator = EqualityOperator.Equals,
                Value = ValidationDetailStatusType.Yes
            };

            definition.RootContainer.FirstGroup.Conditions.Add(militaryCondition);

            return definition;
        }

        /// <summary>
        /// Create the filter definition for US orders
        /// </summary>
        private static FilterDefinition CreateDefinitionUS()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);

            OrderAddressCountryCondition countryCondition = new OrderAddressCountryCondition()
            {
                AddressOperator = BillShipAddressOperator.Ship,
                Operator = StringOperator.Equals,
                TargetValue = "US"
            };

            definition.RootContainer.FirstGroup.Conditions.Add(countryCondition);

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
        /// Create the filter definition for orders that have Address Validation of a given status
        /// </summary>
        private static FilterDefinition CreateAddressValidationDefinition(AddressValidationStatusType Status)
        {
            return FilterHelper.CreateAddressValidationDefinition(new List<AddressValidationStatusType>() { Status });
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
        private static void CreateStatusPresets(Func<bool, SqlAdapter> createSqlAdapter)
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

            using (SqlAdapter adapter = createSqlAdapter(false))
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
        [NDependIgnoreLongMethod]
        private static FilterNodeEntity CreateTopLevelFilter(FilterTarget target, Func<bool, SqlAdapter> createSqlAdapter)
        {
            using (SqlAdapter adapter = createSqlAdapter(false))
            {
                // We will be specifying the pk values
                adapter.IdentityInsert = true;

                try
                {
                    long pkValue = BuiltinFilter.GetTopLevelKey(target);

                    FilterEntity filter = new FilterEntity();
                    filter.FilterID = pkValue;
                    filter.Name = "All";
                    filter.FilterTarget = (int) target;
                    filter.IsFolder = true;
                    filter.Definition = null;
                    filter.State = (int) FilterState.Enabled;
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
        private static void CreateBuiltinTemplateFolders(Func<bool, SqlAdapter> createSqlAdapter)
        {
            using (SqlAdapter adapter = createSqlAdapter(false))
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
