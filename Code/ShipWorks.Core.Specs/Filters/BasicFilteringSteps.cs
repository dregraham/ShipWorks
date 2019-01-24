using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using Autofac;
using BoDi;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Tests.Filters;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Filters.Management;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Shipping.Settings.Printing;
using ShipWorks.Startup;
using ShipWorks.Templates;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using TechTalk.SpecFlow;
using Xunit;

namespace ShipWorks.Core.Specs.Filters
{
    [Binding]
    public class BasicFilteringSteps : IDisposable
    {
        private DataContext context;
        private List<TemplateEntity> templates;
        private readonly IObjectContainer objectContainer;

        public BasicFilteringSteps(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void InitializeContext()
        {
            context = objectContainer.Resolve<DatabaseFixture>()
                .CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IMessageHelper>();
            });

            objectContainer.RegisterInstanceAs(context);
        }

        [Given(@"a filter named ""(.*)"" with an order number condition that ([^ ]*) (.*)")]
        public void GivenAFilterNamedWithAnOrderNumberConditionThatBeginsWith(string name, StringOperator conditionOperator, int orderNumber)
        {
            var condition = new OrderNumberCondition
            {
                IsNumeric = false,
                StringOperator = conditionOperator,
                StringValue = orderNumber.ToString()
            };

            var filter = Create.Entity<FilterEntity>()
                      .Set(f => f.Definition = FilterTestingHelper.OrderNumberDefinitionXml(condition))
                      .Set(f => f.Name = name)
                      .Set(f => f.IsFolder = false)
                      .Set(f => f.State = (int) FilterState.Enabled)
                      .Save();

            FilterNodeEntity rootOrderFilter = FilterLayoutContext.Current.FindNode(BuiltinFilter.GetTopLevelKey(FilterTarget.Orders));
            FilterLayoutContext.Current.AddFilter(filter, rootOrderFilter, 0).First();
            FilterLayoutContext.Current.Reload();

            IProgressReporter progressReporter = new ProgressItem("calc initial filter counts");
            using (DbConnection connection = SqlSession.Current.OpenConnection())
            {
                var filterHelper = IoC.UnsafeGlobalLifetimeScope.Resolve<IFilterHelper>();
                filterHelper.RegenerateFilters(connection);
                filterHelper.CalculateInitialFilterCounts(connection, progressReporter, 0);
            }
        }

        [Given(@"a quick filter named ""(.*)"" with an order number condition that ([^ ]*) (.*)")]
        public void GivenAQuickFilterNamedWithAnOrderNumberConditionThatBeginsWith(string name, StringOperator conditionOperator, int orderNumber)
        {
            var condition = new OrderNumberCondition
            {
                IsNumeric = false,
                StringOperator = conditionOperator,
                StringValue = orderNumber.ToString()
            };

            CreateQuickFilter(name, FilterTarget.Orders, condition);
        }

        [Given(@"a shipment quick filter named ""(.*)"" with an order number condition that ([^ ]*) (.*)")]
        public void GivenAShipmentQuickFilterNamedWithAnOrderNumberConditionThatBeginsWith(string name, StringOperator conditionOperator, int orderNumber)
        {
            var condition = new OrderNumberCondition
            {
                IsNumeric = false,
                StringOperator = conditionOperator,
                StringValue = orderNumber.ToString()
            };

            var containerCondition = new OrderCondition();
            containerCondition.Container.FirstGroup.Conditions.Add(condition);

            CreateQuickFilter(name, FilterTarget.Shipments, containerCondition);
        }

        [Given(@"a provider rule for (.*) for filter ""(.*)""")]
        public void GivenAProviderRuleFor(ShipmentTypeCode shipmentType, string filterName)
        {
            var node = GetFilterNodeNamed(filterName);
            var providerRule = Create.Entity<ShippingProviderRuleEntity>()
                .Set(x => x.FilterNodeID, node.FilterNodeID)
                .Set(x => x.ShipmentTypeCode, shipmentType)
                .Build();

            var ruleManager = IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingProviderRuleManager>();
            ruleManager.SaveRule(providerRule);
        }

        [Given(@"a default provider of (.*)")]
        public void GivenADefaultProviderOf(ShipmentTypeCode shipmentType) =>
            IoC.UnsafeGlobalLifetimeScope
                .Resolve<IShippingSettings>()
                .SetDefaultProvider(shipmentType);

        [Given(@"the following ""Other"" profiles")]
        public void GivenTheFollowingOtherProfiles(Table table) =>
            table.Rows.ForEach(CreateProfile);

        [Given(@"a shipping rule for ""(.*)"" that applies ""(.*)"" for filter ""(.*)""")]
        public void GivenAShippingRuleThatApplies(ShipmentTypeCode shipmentType, string profileName, string filterName)
        {
            var profile = ShippingProfileManager.ProfilesReadOnly.Single(x => x.Name == profileName);
            var filterNode = GetFilterNodeNamed(filterName);

            var rule = Create.Entity<ShippingDefaultsRuleEntity>()
                .Set(x => x.ShipmentType, (int) shipmentType)
                .Set(x => x.FilterNodeID, filterNode.FilterNodeID)
                .Set(x => x.ShippingProfileID, profile.ShippingProfileID)
                .Build();

            ShippingDefaultsRuleManager.SaveRule(rule);
        }

        [Given(@"([0-9]*) templates")]
        public void GivenTemplates(int count)
        {
            var parent = TemplateManager.Tree.CreateEditableClone().RootFolders.Single(x => x.Name == "System");

            Enumerable.Range(1, count)
                .Select(i => Create.Entity<TemplateEntity>()
                    .Set(x => x.ParentFolder, parent)
                    .Set(x => x.Name, $"Template {i}")
                    .Set(x => x.TemplateTree, parent.TemplateTree)
                    .Build())
                .ForEach(x => TemplateEditingService.SaveTemplate(x, true, SqlAdapter.Default));

            TemplateManager.CheckForChangesNeeded();
        }

        [Given(@"a print group for (.*) with the following rules")]
        public void APrintGroupForWithTheFollowingRules(ShipmentTypeCode shipmentType, Table table)
        {
            var group = Create.Entity<ShippingPrintOutputEntity>()
                .Set(x => x.ShipmentType, (int) shipmentType)
                .Set(x => x.Name, Path.GetRandomFileName())
                .Build();

            table.Rows.OfType<TableRow>()
                .Select(x => (condition: x["Condition"], template: x["Template"]))
                .Select(CreateOutputRule)
                .ForEach(x => x.ShippingPrintOutput = group);

            ShippingPrintOutputManager.SaveOutputGroup(group);

            ShippingPrintOutputManager.CheckForChangesNeeded();
        }

        [When(@"templates are retrieved for the shipment for order ([0-9]*)")]
        public void WhenTemplatesAreRetrievedForAShipment(int orderNumber) =>
            templates = ShipmentPrintHelper.DetermineTemplatesToPrint(GetShipmentForOrder(orderNumber));

        [Then(@"order (.*) should be in filter ""(.*)""")]
        public void ThenOrderShouldBeInTheFilter(int orderNumber, string filterName) =>
            Assert.NotEmpty(GetOrdersInFilter(orderNumber, filterName));

        [Then(@"order (.*) should (?:not|NOT) be in filter ""(.*)""")]
        public void ThenOrderShouldNOTBeInTheFilter(int orderNumber, string filterName) =>
            Assert.Empty(GetOrdersInFilter(orderNumber, filterName));

        [Then(@"the shipment for order ([0-9]*) should use provider (.*)")]
        public void ThenTheShipmentForOrderShouldUseProvider(int orderNumber, ShipmentTypeCode shipmentType) =>
            Assert.Equal(shipmentType, GetShipmentForOrder(orderNumber).ShipmentTypeCode);

        [Then(@"the shipment for order ([0-9]*) should have carrier ""(.*)""")]
        public void ThenTheShipmentForOrderShouldHaveCarrier(int orderNumber, string carrier) =>
            Assert.Equal(carrier, GetShipmentForOrder(orderNumber).Other.Carrier);

        [Then(@"the following templates should be returned")]
        public void ThenTheFollowingTemplatesShouldBeReturned(Table table)
        {
            var expectedNames = table.Rows.Select(x => x["Name"]).OrderBy(x => x).ToList();
            var actualNames = templates.Select(x => x.Name).OrderBy(x => x).ToList();

            Assert.Equal(expectedNames, actualNames);
        }

        private ShippingPrintOutputRuleEntity CreateOutputRule((string condition, string template) arg) =>
            Create.Entity<ShippingPrintOutputRuleEntity>()
                .Set(x => x.FilterNodeID, arg.condition == "Always" ?
                    ShippingPrintOutputManager.FilterNodeAlwaysID :
                    GetFilterNodeNamed(arg.condition).FilterNodeID)
                .Set(x => x.TemplateID, TemplateManager.Tree.FindTemplate($@"System\{arg.template}").TemplateID)
                .Build();

        private void CreateProfile(TableRow row)
        {
            var isDefault = row["Default"] == "Yes";
            var profile = Create.Profile()
                .AsOther(o => o.Set(x => x.Carrier, row["Carrier"]).SetDefaultsOnNullableFields(isDefault))
                .Set(x => x.ShipmentTypePrimary, isDefault)
                .Set(x => x.Name, row["Name"])
                .SetDefaultsOnNullableFields(isDefault)
                .Build();

            ShippingProfileManager.SaveProfile(profile);
        }

        private FilterNodeEntity GetFilterNodeNamed(string filterName)
        {
            var factory = new QueryFactory();
            var filterQuery = factory.Filter.Where(FilterFields.Name == filterName).Select(FilterFields.FilterID);
            var sequenceQuery = factory.FilterSequence.Where(FilterSequenceFields.FilterID.In(filterQuery)).Select(FilterSequenceFields.FilterSequenceID);
            var nodeQuery = factory.FilterNode.Where(FilterNodeFields.FilterSequenceID.In(sequenceQuery));

            using (var sqlAdapter = IoC.UnsafeGlobalLifetimeScope.Resolve<ISqlAdapterFactory>().Create())
            {
                return sqlAdapter.FetchFirst(nodeQuery);
            }
        }

        private IShipmentEntity GetShipmentForOrder(int orderNumber)
        {
            var factory = new QueryFactory();
            var orderQuery = factory.Order.Where(OrderFields.OrderNumber == orderNumber).Select(OrderFields.OrderID);
            var shipmentQuery = factory.Shipment.Where(ShipmentFields.OrderID.In(orderQuery));

            using (var sqlAdapter = IoC.UnsafeGlobalLifetimeScope.Resolve<ISqlAdapterFactory>().Create())
            {
                var shipment = sqlAdapter.FetchFirst(shipmentQuery);
                ShippingManager.EnsureShipmentLoaded(shipment);
                return shipment;
            }
        }

        private IEnumerable<IOrderEntity> GetOrdersInFilter(int orderNumber, string filterName)
        {
            var contentID = GetFilterNodeNamed(filterName).FilterNodeContentID;

            var factory = new QueryFactory();
            var filterContents = factory.FilterNodeContentDetail.Where(FilterNodeContentDetailFields.FilterNodeContentID == contentID).Select(FilterNodeContentDetailFields.EntityID);
            var orderQuery = factory.Order.Where(OrderFields.OrderNumber == orderNumber).AndWhere(OrderFields.OrderID.In(filterContents));

            using (var sqlAdapter = IoC.UnsafeGlobalLifetimeScope.Resolve<ISqlAdapterFactory>().Create())
            {
                return sqlAdapter.FetchQuery(orderQuery).OfType<IOrderEntity>();
            }
        }

        private static void CreateQuickFilter(string name, FilterTarget target, Condition containerCondition)
        {
            var node = QuickFilterHelper.CreateQuickFilter(target);
            node.Filter.Definition = FilterTestingHelper.CreateDefinitionXml(target, containerCondition);
            node.Filter.Name = name;
            FilterLayoutContext.Current.SaveFilter(node.Filter);
            FilterLayoutContext.Current.Reload();

            IProgressReporter progressReporter = new ProgressItem("calc initial filter counts");
            using (DbConnection connection = SqlSession.Current.OpenConnection())
            {
                var filterHelper = IoC.UnsafeGlobalLifetimeScope.Resolve<IFilterHelper>();
                filterHelper.RegenerateFilters(connection);
                filterHelper.CalculateInitialFilterCounts(connection, progressReporter, 0);
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
