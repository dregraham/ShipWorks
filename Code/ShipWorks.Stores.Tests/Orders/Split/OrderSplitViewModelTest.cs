using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Extensions;
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
        public async Task GetSplitDetailsFromUser_Throws_WhenOrderIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => testObject.GetSplitDetailsFromUser(null, "foo"));
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("Foo")]
        public async Task GetSplitDetailsFromUser_SetsData_WhenOrderIsNotNull(string orderNumber)
        {
            OrderEntity order = new OrderEntity();
            order.ChangeOrderNumber(orderNumber);

            await testObject.GetSplitDetailsFromUser(order, "foo").Recover(_ => null);

            Assert.Equal(orderNumber, testObject.SelectedOrderNumber);
            Assert.Equal("foo", testObject.OrderNumberPostfix);
        }

        [Fact]
        public async Task GetSplitDetailsFromUser_PopulatesOrderItems()
        {
            var order = Create.Order(new StoreEntity(), new CustomerEntity())
                .WithItem(i => i.Set(x => x.Quantity, 2).Set(x => x.Name, "Foo"))
                .WithItem(i => i.Set(x => x.Quantity, 3).Set(x => x.Name, "Bar"))
                .Build();

            await testObject.GetSplitDetailsFromUser(order, "foo").Recover(_ => null);

            Assert.True(testObject.Items.Any(x => x.Name == "Foo" && x.OriginalQuantity.IsEquivalentTo(2)));
            Assert.True(testObject.Items.Any(x => x.Name == "Bar" && x.OriginalQuantity.IsEquivalentTo(3)));
        }

        [Fact]
        public async Task GetSplitDetailsFromUser_PopulatesOrderCharges()
        {
            var order = Create.Order(new StoreEntity(), new CustomerEntity())
                .WithCharge(c => c.Set(x => x.Amount, 2).Set(x => x.Type, "Foo"))
                .WithCharge(i => i.Set(x => x.Amount, 3).Set(x => x.Type, "Bar"))
                .Build();

            await testObject.GetSplitDetailsFromUser(order, "foo").Recover(_ => null);

            Assert.True(testObject.Charges.Any(x => x.Type == "Foo" && x.OriginalAmount == 2));
            Assert.True(testObject.Charges.Any(x => x.Type == "Bar" && x.OriginalAmount == 3));

            Assert.Equal(5, testObject.OriginalTotalCharge);
            Assert.Equal(0, testObject.SplitTotalCharge);
        }

        [Fact]
        public async Task GetSplitDetailsFromUser_DataContext_IsSetToViewModel()
        {
            mock.Mock<IAsyncMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<Func<IDialog>>()))
                .Callback((Func<IDialog> func) => func());

            await testObject.GetSplitDetailsFromUser(new OrderEntity(), "foo").Recover(_ => null);

            mock.Mock<IOrderSplitDialog>().VerifySet(x => x.DataContext = testObject);
        }

        [Fact]
        public async Task GetSplitDetailFromUser_ReturnsFailure_WhenDialogIsCancelled()
        {
            await Assert.ThrowsAsync<Exception>(() => testObject.GetSplitDetailsFromUser(new OrderEntity(), "foo"));
        }

        [Fact]
        public async Task GetSplitDetailFromUser_ReturnsCorrectOrderNumber_WhenDialogIsNotCancelled()
        {
            mock.Mock<IAsyncMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<Func<IDialog>>()))
                .Callback(() =>
                {
                    testObject.SelectedOrderNumber = "123456";
                    testObject.OrderNumberPostfix = "-2";
                })
                .ReturnsAsync(true);

            var details = await testObject.GetSplitDetailsFromUser(new OrderEntity(), "foo");

            Assert.Equal("123456-2", details.NewOrderNumber);
        }

        [Fact]
        public async Task GetSplitDetailFromUser_ReturnsSplitItemQuantities_WhenDialogIsNotCancelled()
        {
            var order = new OrderEntity();
            mock.Mock<IAsyncMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<Func<IDialog>>()))
                .Callback(() =>
                {
                    testObject.Items = new[]
                    {
                        new OrderSplitItemViewModel(new OrderItemEntity { OrderItemID = 1050, Quantity = 2}) { SplitQuantity = 1},
                        new OrderSplitItemViewModel(new OrderItemEntity { OrderItemID = 2050, Quantity = 6}) { SplitQuantity = 2}
                    };
                })
                .ReturnsAsync(true);

            var details = await testObject.GetSplitDetailsFromUser(order, "foo");

            Assert.Equal(1, details.ItemQuantities[1050]);
            Assert.Equal(2, details.ItemQuantities[2050]);
        }

        [Fact]
        public async Task GetSplitDetailFromUser_ReturnsSplitChargeQuantities_WhenDialogIsNotCancelled()
        {
            var order = new OrderEntity();
            mock.Mock<IAsyncMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<Func<IDialog>>()))
                .Callback(() =>
                {
                    testObject.Charges = new[]
                    {
                        new OrderSplitChargeViewModel(new OrderChargeEntity { OrderChargeID = 1050, Amount = 2}) { SplitAmount = 1},
                        new OrderSplitChargeViewModel(new OrderChargeEntity { OrderChargeID = 2050, Amount = 6}) { SplitAmount = 2}
                    };
                })
                .ReturnsAsync(true);

            var details = await testObject.GetSplitDetailsFromUser(order, "foo");

            Assert.Equal(1, details.ChargeAmounts[1050]);
            Assert.Equal(2, details.ChargeAmounts[2050]);
        }

        [Fact]
        public async Task GetSplitDetailFromUser_ReturnsOrder_WhenDialogIsNotCancelled()
        {
            var order = new OrderEntity();
            mock.Mock<IAsyncMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<Func<IDialog>>()))
                .ReturnsAsync((bool?) true);

            var details = await testObject.GetSplitDetailsFromUser(order, "foo");

            Assert.Equal(order, details.Order);
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
