using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Filters.Content.Conditions.Orders.PersonName;
using ShipWorks.Filters.Content.Conditions.Special;
using ShipWorks.Filters.Search;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Filters.Search
{
    public class OrderQuickSearchDefinitionProviderTest : IDisposable
    {
        readonly AutoMock mock;

        private const string testNumericQuery = "42";
        private const string testOneWordQuery = "WordUp";
        private const string testTwoWordQuery = "First Last";
        OrderQuickSearchDefinitionProvider testObject;


        public OrderQuickSearchDefinitionProviderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<OrderQuickSearchDefinitionProvider>();
        }

        [Fact]
        public void GetDefinition_ReturnsOrderDefinition()
        {
            var definition = testObject.GetDefinition(testNumericQuery);

            Assert.Equal(FilterTarget.Orders, definition.FilterTarget);
        }

        [Fact]
        public void GetDefinition_DefinitionIncludesOrderNumberCondition()
        {
            var definition = testObject.GetDefinition(testNumericQuery);

            IEnumerable<OrderNumberCondition> orderNumberConditions = definition.RootContainer.FirstGroup.Conditions.Cast<OrderNumberCondition>();
            Assert.Equal(1, orderNumberConditions.Count());

            OrderNumberCondition orderNumberCondition = orderNumberConditions.Single();

            Assert.False(orderNumberCondition.IsNumeric);
            Assert.Equal(StringOperator.BeginsWith, orderNumberCondition.StringOperator);
            Assert.Equal(testNumericQuery, orderNumberCondition.StringValue);
        }

        [Fact]
        public void GetDefinition_DefinitionDoesNotIncludeNameConditions()
        {
            var definition = testObject.GetDefinition(testNumericQuery);

            var numberOfOtherConditions = definition.RootContainer.FirstGroup.Conditions.Count(c => !(c is OrderNumberCondition));

            Assert.Equal(0, numberOfOtherConditions);
        }

        [Fact]
        public void GetDefinition_DefinitionIncludesNameConditionsWhenPassedFirstAndLastName()
        {
            var definition = testObject.GetDefinition(testTwoWordQuery);

            var rootConditions = definition.RootContainer.FirstGroup.Conditions.ToList();
            
            Assert.Equal(2, rootConditions.Count);

            var combinedResultCondition = rootConditions.OfType<CombinedResultCondition>().Single();

            var nameConditions = combinedResultCondition.Container.FirstGroup.Conditions;
            Assert.Equal(2, nameConditions.Count());
            Assert.Equal("First", nameConditions.OfType<OrderFirstNameCondition>().Single().TargetValue);
            Assert.Equal("Last", nameConditions.OfType<OrderLastNameCondition>().Single().TargetValue);
        }

        [Fact]
        public void GetDefinition_DefinitionIncludesNameAndEmailConditionWhenSingleWord()
        {
            var definition = testObject.GetDefinition(testOneWordQuery);

            var rootConditions = definition.RootContainer.FirstGroup.Conditions.ToList();

            Assert.Equal(2, rootConditions.Count);

            var combinedResultCondition = rootConditions.OfType<CombinedResultCondition>().Single();

            var nameConditions = combinedResultCondition.Container.FirstGroup.Conditions;
            Assert.Equal(3, nameConditions.Count());
            Assert.Equal(testOneWordQuery, nameConditions.OfType<OrderFirstNameCondition>().Single().TargetValue);
            Assert.Equal(testOneWordQuery, nameConditions.OfType<OrderLastNameCondition>().Single().TargetValue);
            Assert.Equal(testOneWordQuery, nameConditions.OfType<OrderEmailAddressCondition>().Single().TargetValue);
        }

        [Fact]
        public void GetDefinition_IncludesStoreSpecificCriteria_WhenStoreHasCriteria()
        {
            var storeType = mock.CreateMock<StoreType>();
            ConditionGroup storeCondition = new ConditionGroup();
            storeType.Setup(s => s.CreateBasicSearchOrderConditions(testOneWordQuery)).Returns(storeCondition);

            mock.Mock<IStoreManager>()
                .Setup(m => m.GetUniqueStoreTypes())
                .Returns(new List<StoreType> {storeType.Object});

            var definition = testObject.GetDefinition(testOneWordQuery);
            
            ConditionGroup condition =
                definition.RootContainer.SecondGroup.FirstGroup.Conditions.Cast<CombinedResultCondition>()
                    .Single()
                    .Container.FirstGroup;

            Assert.Equal(storeCondition, condition);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}