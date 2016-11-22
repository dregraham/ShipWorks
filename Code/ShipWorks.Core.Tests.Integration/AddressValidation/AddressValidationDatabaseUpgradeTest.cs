using System;
using System.Threading.Tasks;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using System.Linq;

namespace ShipWorks.Core.Tests.Integration.AddressValidation
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class AddressValidationDatabaseUpgradeTest : IDisposable
    {
        private readonly DataContext context;

        public AddressValidationDatabaseUpgradeTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void Upgrade_CreatesNodes_IfAddressValidationFiltersDoNotExist()
        {
            var testObject = context.Mock.Create<AddressValidationDatabaseUpgrade>();

            using (var adapter = SqlAdapter.Create(false))
            {
                var currentNodes = GetNodesOfInterest();
                Assert.Null(currentNodes.ExampleNode);
                Assert.Null(currentNodes.AddressValidationNode);

                testObject.Upgrade(adapter);
                adapter.Commit();
            }

            var newNodes = GetNodesOfInterest();
            Assert.NotNull(newNodes.AddressValidationNode);
            Assert.NotNull(newNodes.NotValidated);
            Assert.NotNull(newNodes.ReadyToGo);
        }

        [Fact]
        public void Upgrade_DoesNothing_IfAddressValidationFiltersExist()
        {
            var testObject = context.Mock.Create<AddressValidationDatabaseUpgrade>();

            var currentNodes = GetNodesOfInterest();
            // There are no AV nodes to begin with
            Assert.Null(currentNodes.ExampleNode);
            Assert.Null(currentNodes.AddressValidationNode);

            using (var adapter = SqlAdapter.Create(false))
            {
                testObject.Upgrade(adapter);
                adapter.Commit();
            }

            // The nodes have been added
            var newNodes = GetNodesOfInterest();
            Assert.NotNull(newNodes.AddressValidationNode);
            Assert.NotNull(newNodes.NotValidated);
            Assert.NotNull(newNodes.ReadyToGo);

            using (var adapter = SqlAdapter.Create(false))
            {
                testObject.Upgrade(adapter);
                adapter.Commit();
            }

            // Since nodes are selected using SingleOrDefault, if there were more than one this should error out.
            var sameNodes = GetNodesOfInterest();

            // Make sure all the nodes are the same
            Assert.Equal(newNodes.AddressValidationNode.FilterNodeID, sameNodes.AddressValidationNode.FilterNodeID);
            Assert.Equal(newNodes.NotValidated.FilterNodeID, sameNodes.NotValidated.FilterNodeID);
            Assert.Equal(newNodes.ReadyToGo.FilterNodeID, sameNodes.ReadyToGo.FilterNodeID);
        }

        private NodesOfInterest GetNodesOfInterest()
        {
            var nodes = new NodesOfInterest();

            FilterLayoutContext.Current.Reload();

            nodes.OrderNode = FilterLayoutContext.Current.FindNode(BuiltinFilter.GetTopLevelKey(FilterTarget.Orders));
            nodes.ExampleNode = nodes.OrderNode.ChildNodes.SingleOrDefault(filter => filter.Filter.Name == AddressValidationDatabaseUpgrade.ExamplesFolderName);
            nodes.AddressValidationNode = nodes.ExampleNode?.ChildNodes.SingleOrDefault(filter => filter.Filter.Name == AddressValidationDatabaseUpgrade.AddressValidationFolderName);

            nodes.NotValidated = nodes.AddressValidationNode?.ChildNodes.SingleOrDefault(filter => filter.Filter.Name == AddressValidationDatabaseUpgrade.NotValidated);
            nodes.ReadyToGo = nodes.AddressValidationNode?.ChildNodes.SingleOrDefault(filter => filter.Filter.Name == AddressValidationDatabaseUpgrade.ReadyToGo);
            
            return nodes;
        }

        struct NodesOfInterest
        {
            public FilterNodeEntity OrderNode { get; set; }
            public FilterNodeEntity ExampleNode { get; set; }
            public FilterNodeEntity AddressValidationNode { get; set; }
            public FilterNodeEntity NotValidated { get; set; }
            public FilterNodeEntity ReadyToGo { get; set; }
        }

        public void Dispose() => context.Dispose();
    }
}
