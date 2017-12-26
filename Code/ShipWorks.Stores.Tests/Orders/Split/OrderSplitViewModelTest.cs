using System;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Orders.Split
{
    public class OrderSplitViewModelTest
    {
        private readonly AutoMock mock;
        private readonly OrderSplitViewModel testObject;

        public OrderSplitViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<OrderSplitViewModel>();
        }

        [Fact]
        public void GetSplitDetailsFromUser_Throws_WhenOrderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => (object) testObject.GetSplitDetailsFromUser(null, "foo"));
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("Foo")]
        public void GetSplitDetailsFromUser_SetsData_WhenOrderIsNotNull(string orderNumber)
        {
            OrderEntity order = new OrderEntity();
            order.ChangeOrderNumber(orderNumber);
            testObject.GetSplitDetailsFromUser(order, "foo");

            Assert.Equal(orderNumber, testObject.SelectedOrderNumber);
            Assert.Equal("foo", testObject.OrderNumberPostfix);
        }

        [Fact]
        public void GetSplitDetailsFromUser_PopulatesOrderItems()
        {
            var order = Create.Order(new StoreEntity(), new CustomerEntity())
                .WithItem(i => i.Set(x => x.Quantity, 2).Set(x => x.Name, "Foo"))
                .WithItem(i => i.Set(x => x.Quantity, 3).Set(x => x.Name, "Bar"))
                .Build();

            testObject.GetSplitDetailsFromUser(order, "foo");

            Assert.True(testObject.Items.Any(x => x.Name == "Foo" && x.OriginalQuantity.IsEquivalentTo(2)));
            Assert.True(testObject.Items.Any(x => x.Name == "Bar" && x.OriginalQuantity.IsEquivalentTo(3)));
        }

        [Fact]
        public void GetSplitDetailsFromUser_PopulatesOrderCharges()
        {
            var order = Create.Order(new StoreEntity(), new CustomerEntity())
                .WithCharge(c => c.Set(x => x.Amount, 2).Set(x => x.Type, "Foo"))
                .WithCharge(i => i.Set(x => x.Amount, 3).Set(x => x.Type, "Bar"))
                .Build();

            testObject.GetSplitDetailsFromUser(order, "foo");

            Assert.True(testObject.Charges.Any(x => x.Type == "Foo" && x.OriginalAmount == 2));
            Assert.True(testObject.Charges.Any(x => x.Type == "Bar" && x.OriginalAmount == 3));
        }

        [Fact]
        public void GetSplitDetailsFromUser_DataContext_IsSetToViewModel()
        {
            testObject.GetSplitDetailsFromUser(new OrderEntity(), "foo");
            mock.Mock<IOrderSplitDialog>().VerifySet(x => x.DataContext = testObject);
        }

        [Fact]
        public void GetSplitDetailsFromUser_CallShowDialog_OnMessageHelper()
        {
            testObject.GetSplitDetailsFromUser(new OrderEntity(), "foo");
            var dialog = mock.Mock<IOrderSplitDialog>();
            mock.Mock<IMessageHelper>().Verify(x => x.ShowDialog(dialog.Object));
        }

        [Fact]
        public void GetSplitDetailFromUser_ReturnsFailure_WhenDialogIsCancelled()
        {
            var details = testObject.GetSplitDetailsFromUser(new OrderEntity(), "foo");

            Assert.True(details.Failure);
        }

        [Fact]
        public void GetSplitDetailFromUser_ReturnsSuccess_WhenDialogIsNotCancelled()
        {
            mock.Mock<IMessageHelper>().Setup(x => x.ShowDialog(It.IsAny<IOrderSplitDialog>())).Returns(true);
            var details = testObject.GetSplitDetailsFromUser(new OrderEntity(), "foo");

            Assert.True(details.Success);
        }

        [Fact]
        public void GetSplitDetailFromUser_ReturnsCorrectOrderNumber_WhenDialogIsNotCancelled()
        {
            mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<IOrderSplitDialog>()))
                .Callback(() =>
                {
                    testObject.SelectedOrderNumber = "123456";
                    testObject.OrderNumberPostfix = "-2";
                })
                .Returns(true);
            var details = testObject.GetSplitDetailsFromUser(new OrderEntity(), "foo");

            Assert.Equal("123456-2", details.Value.NewOrderNumber);
        }

        [Fact]
        public void GetSplitDetailFromUser_ReturnsSplitItemQuantities_WhenDialogIsNotCancelled()
        {
            var order = new OrderEntity();
            mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<IOrderSplitDialog>()))
                .Callback(() =>
                {
                    testObject.Items = new[]
                    {
                        new OrderSplitItemViewModel(new OrderItemEntity { OrderItemID = 1050, Quantity = 2}) { SplitQuantity = 1},
                        new OrderSplitItemViewModel(new OrderItemEntity { OrderItemID = 2050, Quantity = 6}) { SplitQuantity = 2}
                    };
                })
                .Returns(true);

            var details = testObject.GetSplitDetailsFromUser(order, "foo");

            Assert.Equal(1, details.Value.ItemQuantities[1050]);
            Assert.Equal(2, details.Value.ItemQuantities[2050]);
        }

        [Fact]
        public void GetSplitDetailFromUser_ReturnsSplitChargeQuantities_WhenDialogIsNotCancelled()
        {
            var order = new OrderEntity();
            mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<IOrderSplitDialog>()))
                .Callback(() =>
                {
                    testObject.Charges = new[]
                    {
                        new OrderSplitChargeViewModel(new OrderChargeEntity { OrderChargeID = 1050, Amount = 2}) { SplitAmount = 1},
                        new OrderSplitChargeViewModel(new OrderChargeEntity { OrderChargeID = 2050, Amount = 6}) { SplitAmount = 2}
                    };
                })
                .Returns(true);

            var details = testObject.GetSplitDetailsFromUser(order, "foo");

            Assert.Equal(1, details.Value.ChargeAmounts[1050]);
            Assert.Equal(2, details.Value.ChargeAmounts[2050]);
        }

        [Fact]
        public void GetSplitDetailFromUser_ReturnsOrder_WhenDialogIsNotCancelled()
        {
            var order = new OrderEntity();
            mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<IOrderSplitDialog>()))
                .Returns(true);

            var details = testObject.GetSplitDetailsFromUser(order, "foo");

            Assert.Equal(order, details.Value.Order);
        }

        [Fact]
        public void CancelSplitAction_CallsClose_OnSplitOrderDialog()
        {
            testObject.CancelSplit.Execute(null);
            mock.Mock<IOrderSplitDialog>().Verify(x => x.Close());
        }

        [Fact]
        public void ConfirmSplitAction_CallsClose_OnSplitOrderDialog()
        {
            testObject.ConfirmSplit.Execute(null);
            mock.Mock<IOrderSplitDialog>().Verify(x => x.Close());
        }

        [Fact]
        public void ConfirmSplitAction_SetsDialogResultToTrue_OnSplitOrderDialog()
        {
            testObject.ConfirmSplit.Execute(null);
            mock.Mock<IOrderSplitDialog>().VerifySet(x => x.DialogResult = true);
        }
    }
}
