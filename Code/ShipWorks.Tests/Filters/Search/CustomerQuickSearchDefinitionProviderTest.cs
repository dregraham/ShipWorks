using System.Linq;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Customers;
using ShipWorks.Filters.Content.Conditions.Customers.PersonName;
using ShipWorks.Filters.Search;
using Xunit;

namespace ShipWorks.Tests.Filters.Search
{
    public class CustomerQuickSearchDefinitionProviderTest
    {
        private readonly CustomerQuickSearchDefinitionProvider testObject;

        public CustomerQuickSearchDefinitionProviderTest()
        {
            testObject = new CustomerQuickSearchDefinitionProvider();
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithCustomerIDCondition_WhenSearchStringIsNumber()
        {
            var def = testObject.GetDefinition("12345");

            CustomerIDCondition condition =
                def.RootContainer.FirstGroup.Conditions.FirstOrDefault(
                    c => c.GetType() == typeof(CustomerIDCondition)) as CustomerIDCondition;

            Assert.Equal(12345, condition.Value1);
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithoutCustomerIDCondition_WhenSearchStringIsNotNumber()
        {
            var def = testObject.GetDefinition("Chris");

            CustomerIDCondition condition =
                def.RootContainer.FirstGroup.Conditions.FirstOrDefault(
                    c => c.GetType() == typeof(CustomerIDCondition)) as CustomerIDCondition;

            Assert.Null(condition);
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithFullSearchStringCustomerFirstNameCondition_WhenSearchStringDoesNotHaveSpaces()
        {
            var def = testObject.GetDefinition("Chris");

            CustomerFirstNameCondition condition =
                def.RootContainer.FirstGroup.Conditions.FirstOrDefault(
                    c => c.GetType() == typeof(CustomerFirstNameCondition)) as CustomerFirstNameCondition;

            Assert.Equal("Chris", condition.TargetValue);
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithFullSearchStringCustomerLastNameCondition_WhenSearchStringDoesNotHaveSpaces()
        {
            var def = testObject.GetDefinition("Chris");

            CustomerLastNameCondition condition =
                def.RootContainer.FirstGroup.Conditions.FirstOrDefault(
                    c => c.GetType() == typeof(CustomerLastNameCondition)) as CustomerLastNameCondition;

            Assert.Equal("Chris", condition.TargetValue);
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithFullSearchStringCustomerEmailCondition_WhenSearchStringDoesNotHaveSpaces()
        {
            var def = testObject.GetDefinition("Chris");

            CustomerEmailAddressCondition condition =
                def.RootContainer.FirstGroup.Conditions.FirstOrDefault(
                    c => c.GetType() == typeof(CustomerEmailAddressCondition)) as CustomerEmailAddressCondition;

            Assert.Equal("Chris", condition.TargetValue);
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithParsedSearchStringCustomerFirstNameCondition_WhenSearchStringHasSpaces()
        {
            var def = testObject.GetDefinition("Chris is cool");

            CustomerFirstNameCondition condition =
                def.RootContainer.FirstGroup.Conditions.FirstOrDefault(
                    c => c.GetType() == typeof(CustomerFirstNameCondition)) as CustomerFirstNameCondition;

            Assert.Equal("Chris", condition.TargetValue);
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithParsedSearchStringCustomerLastNameCondition_WhenSearchStringHasSpaces()
        {
            var def = testObject.GetDefinition("Chris is cool");

            CustomerLastNameCondition condition =
                def.RootContainer.FirstGroup.Conditions.FirstOrDefault(
                    c => c.GetType() == typeof(CustomerLastNameCondition)) as CustomerLastNameCondition;

            Assert.Equal("cool", condition.TargetValue);
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithoutCustomerEmailCondition_WhenSearchStringHasSpaces()
        {
            var def = testObject.GetDefinition("Chris is cool");

            CustomerEmailAddressCondition condition =
                def.RootContainer.FirstGroup.Conditions.FirstOrDefault(
                    c => c.GetType() == typeof(CustomerEmailAddressCondition)) as CustomerEmailAddressCondition;

            Assert.Null(condition);
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithFirstGroupJoinTypeAny_WhenSearchStringDoesNotHaveSpaces()
        {
            var def = testObject.GetDefinition("Chris");

            Assert.Equal(ConditionJoinType.Any, def.RootContainer.FirstGroup.JoinType);
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithFirstGroupJoinTypeAll_WhenSearchStringHasSpaces()
        {
            var def = testObject.GetDefinition("Chris is cool");

            Assert.Equal(ConditionJoinType.All, def.RootContainer.FirstGroup.JoinType);
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithConditionAddressOperatorShipOrBill()
        {
            var def = testObject.GetDefinition("Chris");

            CustomerFirstNameCondition condition =
                def.RootContainer.FirstGroup.Conditions.FirstOrDefault(
                    c => c.GetType() == typeof(CustomerFirstNameCondition)) as CustomerFirstNameCondition;

            Assert.Equal(BillShipAddressOperator.ShipOrBill, condition.AddressOperator);
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithConditionOperatorBeginsWith()
        {
            var def = testObject.GetDefinition("Chris");

            CustomerFirstNameCondition condition =
                def.RootContainer.FirstGroup.Conditions.FirstOrDefault(
                    c => c.GetType() == typeof(CustomerFirstNameCondition)) as CustomerFirstNameCondition;

            Assert.Equal(StringOperator.BeginsWith, condition.Operator);
        }
    }
}