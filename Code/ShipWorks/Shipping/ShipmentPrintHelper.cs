using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings.Printing;
using ShipWorks.Templates;
using ShipWorks.Filters;
using log4net;
using System.Drawing;
using ShipWorks.ApplicationCore;
using System.IO;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.Data;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Administration;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Management;
using ShipWorks.Filters.Content.Conditions.Shipments.ShipTo.Address;
using ShipWorks.Shipping.Carriers.UPS.CoreExtensions.Filters;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Utility class for resolving what templates to print a shipment with
    /// </summary>
    public static class ShipmentPrintHelper
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipmentPrintHelper));

        /// <summary>
        /// Determine what templates to use to print the given shipment with based on rules of the shipment type
        /// </summary>
        public static List<TemplateEntity> DetermineTemplatesToPrint(ShipmentEntity shipment)
        {
            List<TemplateEntity> templates = new List<TemplateEntity>();

            // Get all the configured groups for this shipment
            List<ShippingPrintOutputEntity> groups = ShippingPrintOutputManager.GetOutputGroups((ShipmentTypeCode) shipment.ShipmentType);

            // There will be 0 or 1 templates per group
            foreach (ShippingPrintOutputEntity group in groups)
            {
                TemplateEntity template = DetermineGroupTemplate(group, shipment);
                if (template != null)
                {
                    templates.Add(template);
                }
            }

            log.DebugFormat("Printing Shipment {0}: {1} Templates", shipment.ShipmentID, templates.Count);

            foreach (TemplateEntity template in templates)
            {
                log.DebugFormat("   Template '{0}'", template.FullName);
            }

            return templates;
        }

        /// <summary>
        /// Determine the template to use for the given group.  If there are no matches, null is returned
        /// </summary>
        private static TemplateEntity DetermineGroupTemplate(ShippingPrintOutputEntity group, ShipmentEntity shipment)
        {
            // Test each rule in the group in order to see what template matches
            foreach (ShippingPrintOutputRuleEntity rule in group.Rules)
            {
                TemplateEntity template = TemplateManager.Tree.GetTemplate(rule.TemplateID);
                if (template != null)
                {
                    if (rule.FilterNodeID == ShippingPrintOutputManager.FilterNodeAlwaysID)
                    {
                        return template;
                    }

                    long? filterContentID = FilterHelper.GetFilterNodeContentID(rule.FilterNodeID);
                    if (filterContentID != null)
                    {
                        if (FilterHelper.IsObjectInFilterContent(shipment.ShipmentID, filterContentID.Value))
                        {
                            return template;
                        }
                        else
                        {
                            log.Info($"ShipmentPrintHelper.DetermineGroupTemplate: ShipmentID wasn't found in filter content, trying again.");

                            // Double check that quick filters are up to date, and check the filter content again.
                            FilterHelper.EnsureQuickFiltersUpToDate();

                            if (FilterHelper.IsObjectInFilterContent(shipment.ShipmentID, filterContentID.Value))
                            {
                                return template;
                            }

                            log.Info($"ShipmentPrintHelper.DetermineGroupTemplate: Event after calling EnsureQuickFiltersUpToDate(), ShipmentID wasn't found in filter content, trying again.");
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Find out if a Print Output Group already exists with the given name.
        /// </summary>
        private static bool GroupMissing(List<ShippingPrintOutputEntity> existingGroups, string groupName)
        {
            return (existingGroups.Count(o => String.Compare(o.Name, groupName, StringComparison.OrdinalIgnoreCase) == 0) == 0);
        }

        /// <summary>
        /// Installs the default set of printing rules for the given shipment type.  Any questions or messages to the user are displayed using the given owner.
        /// Any existing rules will be deleted.
        /// 
        /// Specifying reinstallMissing = true will only re-create missing output groups.  No rules are deleted, only added.
        /// </summary>
        public static void InstallDefaultRules(ShipmentTypeCode shipmentType, bool reinstallMissing, IWin32Window owner)
        {
            // get the existing groups
            List<ShippingPrintOutputEntity> existingGroups = ShippingPrintOutputManager.GetOutputGroups(shipmentType);

            // Cant be transacted, b\c if templates are mising we show ui and need to check TemplateManager for changes.
            using (SqlAdapter adapter = new SqlAdapter())
            {
                ShippingPrintOutputManager.CheckForChangesNeeded();

                // full install just means to wipe pre-existing
                if (!reinstallMissing)
                {
                    DeleteAllRules(shipmentType);
                }

                // These services have no labels, and thus no default rules
                if (shipmentType == ShipmentTypeCode.None ||
                    shipmentType == ShipmentTypeCode.Other ||
                    shipmentType == ShipmentTypeCode.UpsWorldShip)
                {
                    // Do Nothing
                }
                else
                {
                    if (!reinstallMissing || GroupMissing(existingGroups, "Labels"))
                    {
                        ShippingPrintOutputEntity labelsGroup = new ShippingPrintOutputEntity();
                        labelsGroup.Name = "Labels";
                        labelsGroup.ShipmentType = (int)shipmentType;
                        adapter.SaveAndRefetch(labelsGroup);

                        // This is for the "Labels" output group
                        if (shipmentType == ShipmentTypeCode.PostalWebTools)
                        {
                            CreateStandardLabelRules(labelsGroup, owner);
                        }
                        else
                        {
                            CreateThermalStandardLabelRules(labelsGroup, owner);
                        }
                    }

                    // This is for the "Commercial Invoice" group
                    if (shipmentType == ShipmentTypeCode.FedEx || shipmentType == ShipmentTypeCode.UpsOnLineTools || shipmentType == ShipmentTypeCode.DhlExpress)
                    {
                        if (!reinstallMissing || GroupMissing(existingGroups, "Commercial Invoice"))
                        {
                            ShippingPrintOutputEntity invoiceGroup = new ShippingPrintOutputEntity();
                            invoiceGroup.Name = "Commercial Invoice";
                            invoiceGroup.ShipmentType = (int)shipmentType;
                            adapter.SaveAndRefetch(invoiceGroup);

                            CreateCommercialInvoiceRules(invoiceGroup, owner);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create the printing ruleset that just prints standard labels always
        /// </summary>
        private static void CreateStandardLabelRules(ShippingPrintOutputEntity group, IWin32Window owner)
        {
            ShippingPrintOutputRuleEntity rule = new ShippingPrintOutputRuleEntity();
            rule.FilterNodeID = ShippingPrintOutputManager.FilterNodeAlwaysID;
            rule.TemplateID = GetTemplateID(@"Labels\Standard", owner);
            group.Rules.Add(rule);

            ShippingPrintOutputManager.SaveOutputGroup(group);
        }

        /// <summary>
        /// Create the printing ruleset that prints standard labels and thermal labels depending on label type
        /// </summary>
        private static void CreateThermalStandardLabelRules(ShippingPrintOutputEntity group, IWin32Window owner)
        {
            ShippingPrintOutputRuleEntity thermalRule = new ShippingPrintOutputRuleEntity();
            thermalRule.FilterNodeID = GetFilterNodeID(CreateFilterDefinitionThermal(), "Thermal");
            thermalRule.TemplateID = GetTemplateID(@"Labels\Thermal", owner);
            group.Rules.Add(thermalRule);

            ShippingPrintOutputRuleEntity standardRule = new ShippingPrintOutputRuleEntity();
            standardRule.FilterNodeID = ShippingPrintOutputManager.FilterNodeAlwaysID;
            standardRule.TemplateID = GetTemplateID(@"Labels\Standard", owner);
            group.Rules.Add(standardRule);

            ShippingPrintOutputManager.SaveOutputGroup(group);
        }

        /// <summary>
        /// Create the printing ruleset that prints UPS Return Shipments (those that aren't electronic).  
        /// This won't be used until we find out many people are needing to configure the action and are calling support,
        /// at which time we'll include a separate Print Output Group type specifically for returns.
        /// </summary>
        private static void CreateUPSReturnShipmentRules(ShippingPrintOutputEntity group, IWin32Window owner)
        {
            ShippingPrintOutputRuleEntity thermalRule = new ShippingPrintOutputRuleEntity();
            thermalRule.FilterNodeID = GetFilterNodeID(CreateFilterDefinitionUPSPrintableReturn(true), "Thermal Return Labels");
            thermalRule.TemplateID = GetTemplateID(@"Labels\Thermal", owner);
            group.Rules.Add(thermalRule);

            ShippingPrintOutputRuleEntity standardRule = new ShippingPrintOutputRuleEntity();
            standardRule.FilterNodeID = GetFilterNodeID(CreateFilterDefinitionUPSPrintableReturn(false), "Standard Returns Labels");
            standardRule.TemplateID = GetTemplateID(@"Labels\Standard", owner);
            group.Rules.Add(standardRule);

            ShippingPrintOutputManager.SaveOutputGroup(group);
        }

        /// <summary>
        /// Create the printing ruleset that prints a commercial invoice depending on conditions
        /// </summary>
        private static void CreateCommercialInvoiceRules(ShippingPrintOutputEntity group, IWin32Window owner)
        {
            ShippingPrintOutputRuleEntity rule = new ShippingPrintOutputRuleEntity();
            rule.FilterNodeID = GetFilterNodeID(CreateFilterDefinitionInternational(), "International");
            rule.TemplateID = GetTemplateID(@"Labels\Commercial Invoice", owner);
            group.Rules.Add(rule);

            ShippingPrintOutputManager.SaveOutputGroup(group);
        }

        /// <summary>
        /// Delete all the existing rules for the given shipment type
        /// </summary>
        private static void DeleteAllRules(ShipmentTypeCode shipmentType)
        {
            // Delete every output group (which cascades down to the rules)
            foreach (ShippingPrintOutputEntity group in ShippingPrintOutputManager.GetOutputGroups(shipmentType))
            {
                ShippingPrintOutputManager.DeleteOutputGroup(group);
            }
        }

        /// <summary>
        /// Get the FilterID of a filter with the given definition.  Local filters are checked,
        /// and if not found a new local filter is created with the given name.
        /// </summary>
        private static long GetFilterNodeID(FilterDefinition definition, string defaultName)
        {
            string desiredXml = definition.GetXml();

            foreach (FilterNodeEntity node in QuickFilterHelper.GetQuickFilters(FilterTarget.Shipments))
            {
                FilterDefinition nodeDefinition = new FilterDefinition(node.Filter.Definition);

                if (nodeDefinition.GetXml() == desiredXml)
                {
                    return node.FilterNodeID;
                }
            }

            FilterNodeEntity newNode = QuickFilterHelper.CreateQuickFilter(FilterTarget.Shipments);
            newNode.Filter.Name = defaultName;
            newNode.Filter.Definition = definition.GetXml();

            // Save the filter
            FilterLayoutContext.Current.SaveFilter(newNode.Filter);

            return newNode.FilterNodeID;
        }

        /// <summary>
        /// Get the TemplateID of the template with the specified name.  If the template does not exist the user is asked if they want to select
        /// an existing template or create it automatically.  If they choose to create, its pulled from the builtin template distribution set.
        /// </summary>
        private static long GetTemplateID(string fullName, IWin32Window owner)
        {
            // See if the template of the given name exists.  If it does, we'll assume its the one we are looking for - or one the user has edited to be what the want.  Either way, we're going to use it.
            TemplateEntity template = TemplateManager.Tree.FindTemplate(fullName);
            if (template != null)
            {
                return template.TemplateID;
            }

            // Didn't find it, we have to let the user pick or creat a new one
            using (PrintRuleInstallMissingTemplateDlg dlg = new PrintRuleInstallMissingTemplateDlg(fullName))
            {
                dlg.ShowDialog(owner);

                return dlg.TemplateID;
            }
        }

        /// <summary>
        /// Create the filter definition for filtering on thermal labels
        /// </summary>
        private static FilterDefinition CreateFilterDefinitionThermal()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Shipments);
            definition.RootContainer.FirstGroup.Conditions.Add(new LabelFormatCondition() { Value = LabelFormatType.Thermal, Operator = EqualityOperator.Equals });

            return definition;
        }

        /// <summary>
        /// Create the filter definition for filtering on UPS Return shipments, thermal or standard
        /// </summary>
        private static FilterDefinition CreateFilterDefinitionUPSPrintableReturn(bool thermal)
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Shipments);
            definition.RootContainer.FirstGroup.Conditions.Add(new UpsReturnServiceCondition() { Value = UpsReturnServiceType.PrintReturnLabel, Operator = EqualityOperator.Equals });
            definition.RootContainer.FirstGroup.Conditions.Add(new LabelFormatCondition() { Value = (thermal ? LabelFormatType.Thermal : LabelFormatType.Standard), Operator = EqualityOperator.Equals });

            return definition;
        }

        /// <summary>
        /// Create the filter definition for filtering on international shipments
        /// </summary>
        private static FilterDefinition CreateFilterDefinitionInternational()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Shipments);
            definition.RootContainer.FirstGroup.Conditions.Add(new ShipToCountryCondition() { TargetValue = "US", Operator = StringOperator.NotEqual });

            return definition;
        }
    }
}
