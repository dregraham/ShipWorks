using System;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Content
{
    /// <summary>
    /// Tests for CombineOrdersViewModel
    /// </summary>
    public class CombineOrdersViewModelTest : IDisposable
    {
        readonly AutoMock mock;

        public CombineOrdersViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void SurvivingOrder_UpdatesAddress_WhenChanged()
        {
            var testObject = mock.Create<CombineOrdersViewModel>();

            testObject.SurvivingOrder = new OrderEntity
            {
                ShipPerson =
                {
                    UnparsedName = "Foo Bar",
                    Street1 = "123 Main",
                    City = "St. Louis"
                }
            };

            Assert.Equal("Foo Bar", testObject.AddressName);
            Assert.Equal("123 Main", testObject.AddressStreet);
            Assert.Equal("St. Louis", testObject.AddressCityStateZip);
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData("123 Main", "", "123 Main")]
        [InlineData("", "Suite 2000", "Suite 2000")]
        [InlineData("123 Main", "Suite 2000", "123 Main, Suite 2000")]
        public void AddressStreet_IsFormmattedCorrectly(string street1, string street2, string expected)
        {
            var testObject = mock.Create<CombineOrdersViewModel>();

            testObject.SurvivingOrder = new OrderEntity
            {
                ShipPerson = { Street1 = street1, Street2 = street2 }
            };

            Assert.Equal(expected, testObject.AddressStreet);
        }

        [Theory]
        [InlineData("", "", "", "")]
        [InlineData("St. Louis", "", "", "St. Louis")]
        [InlineData("", "MO", "", "MO")]
        [InlineData("", "", "63123", "63123")]
        [InlineData("St. Louis", "MO", "", "St. Louis, MO")]
        [InlineData("St. Louis", "", "63123", "St. Louis, 63123")]
        [InlineData("", "MO", "63123", "MO 63123")]
        [InlineData("St. Louis", "MO", "63123", "St. Louis, MO 63123")]
        public void AddressCityStateZip_IsFormmattedCorrectly(string city, string state, string zip, string expected)
        {
            var testObject = mock.Create<CombineOrdersViewModel>();

            testObject.SurvivingOrder = new OrderEntity
            {
                ShipPerson =
                {
                    City = city,
                    StateProvCode = state,
                    PostalCode = zip
                }
            };

            Assert.Equal(expected, testObject.AddressCityStateZip);
        }

        [Fact]
        public void GetCombinationDetailsFromUser_ThrowsArgumentException_WhenOrdersAreNull()
        {
            var testObject = mock.Create<CombineOrdersViewModel>();

            Assert.Throws<ArgumentException>(() => testObject.GetCombinationDetailsFromUser(null));
        }

        [Fact]
        public void GetCombinationDetailsFromUser_ThrowsArgumentException_WhenOrdersAreEmpty()
        {
            var testObject = mock.Create<CombineOrdersViewModel>();

            Assert.Throws<ArgumentException>(() => testObject.GetCombinationDetailsFromUser(Enumerable.Empty<IOrderEntity>()));
        }

        [Fact]
        public void GetCombinationDetailsFromUser_SetsOrdersProperty_WhenOrdersAreValid()
        {
            var testObject = mock.Create<CombineOrdersViewModel>();

            testObject.GetCombinationDetailsFromUser(Enumerable.Range(1, 2).Select(x => new OrderEntity { OrderID = x }));

            Assert.Equal(new long[] { 1, 2 }, testObject.Orders.Select(x => x.OrderID));
        }

        [Fact]
        public void GetCombinationDetailsFromUser_SetsNewOrderNumber_BasedOnFirstOrder()
        {
            var testObject = mock.Create<CombineOrdersViewModel>();

            testObject.GetCombinationDetailsFromUser(Enumerable.Range(1, 2).Select(x => new OrderEntity { OrderNumber = x }));

            Assert.Equal("1-C", testObject.NewOrderNumber);
        }

        [Fact]
        public void GetCombinationDetailsFromUser_SetsSurvivingOrder_ToFirstOrder()
        {
            var testObject = mock.Create<CombineOrdersViewModel>();

            var orders = Enumerable.Range(1, 2).Select(x => new OrderEntity { OrderID = x });
            testObject.GetCombinationDetailsFromUser(orders);

            Assert.Equal(orders.First().OrderID, testObject.SurvivingOrder.OrderID);
        }

        [Fact]
        public void GetCombinationDetailsFromUser_SetsAddress_ToFirstOrder()
        {
            var testObject = mock.Create<CombineOrdersViewModel>();

            var orders = Enumerable.Range(1, 2).Select(x => new OrderEntity { ShipStreet1 = x.ToString() });
            testObject.GetCombinationDetailsFromUser(orders);

            Assert.Equal("1", testObject.AddressStreet);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void GetCombinationDetailsFromUser_SetsAllAddressesMatch(bool addressesMatch, bool expected)
        {
            mock.Mock<IOrderCombineAddressComparer>()
                .Setup(x => x.Equals(It.IsAny<PersonAdapter>(), It.IsAny<PersonAdapter>()))
                .Returns(addressesMatch);
            var testObject = mock.Create<CombineOrdersViewModel>();

            testObject.GetCombinationDetailsFromUser(Enumerable.Range(1, 2).Select(x => new OrderEntity()));

            Assert.Equal(expected, testObject.AllAddressesMatch);
        }

        [Fact]
        public void GetCombinationDetailsFromUser_SetsDataContext_OnDialog()
        {
            var testObject = mock.Create<CombineOrdersViewModel>();

            testObject.GetCombinationDetailsFromUser(Enumerable.Range(1, 2).Select(x => new OrderEntity()));

            mock.Mock<ICombineOrdersDialog>()
                .VerifySet(x => x.DataContext = testObject);
        }

        [Fact]
        public void GetCombinationDetailsFromUser_DelegatesToMessageHelper_ToShowDialog()
        {
            var dialog = mock.Mock<ICombineOrdersDialog>().Object;
            var testObject = mock.Create<CombineOrdersViewModel>();

            testObject.GetCombinationDetailsFromUser(Enumerable.Range(1, 2).Select(x => new OrderEntity()));

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowDialog(dialog));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(null)]
        public void GetCombinationDetailsFromUser_ReturnsFailure_WhenDialogResultIsNotTrue(bool? returnValue)
        {
            mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<IDialog>()))
                .Returns(returnValue);
            var testObject = mock.Create<CombineOrdersViewModel>();

            var result = testObject.GetCombinationDetailsFromUser(Enumerable.Range(1, 2).Select(x => new OrderEntity()));

            Assert.True(result.Failure);
            Assert.Equal("Canceled", result.Message);
        }

        [Fact]
        public void GetCombinationDetailsFromUser_ReturnsSuccessfulData_WhenDialogResultIsTrue()
        {
            mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<IDialog>()))
                .Returns(true);
            var testObject = mock.Create<CombineOrdersViewModel>();

            var orders = Enumerable.Range(1, 2).Select(x => new OrderEntity { OrderID = x, OrderNumber = x });
            var result = testObject.GetCombinationDetailsFromUser(orders);

            Assert.True(result.Success);
            Assert.Equal(1, result.Value.Item1);
            Assert.Equal("1-C", result.Value.Item2);
        }

        [Fact]
        public void ConfirmCombine_SetsDialogResultToTrue()
        {
            var testObject = mock.Create<CombineOrdersViewModel>();

            testObject.SurvivingOrder = mock.Create<IOrderEntity>();
            testObject.NewOrderNumber = "foo";

            testObject.ConfirmCombine.Execute(null);

            mock.Mock<ICombineOrdersDialog>()
                .VerifySet(x => x.DialogResult = true);
        }

        [Fact]
        public void ConfirmCombine_ClosesDialog()
        {
            var testObject = mock.Create<CombineOrdersViewModel>();

            testObject.SurvivingOrder = mock.Create<IOrderEntity>();
            testObject.NewOrderNumber = "foo";

            testObject.ConfirmCombine.Execute(null);

            mock.Mock<ICombineOrdersDialog>().Verify(x => x.Close());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
