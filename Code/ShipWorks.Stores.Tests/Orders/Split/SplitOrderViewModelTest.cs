using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Orders.Split
{
    public class SplitOrderViewModelTest
    {
        private readonly AutoMock mock;
        private readonly OrderSplitViewModel testObject;

        public SplitOrderViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<OrderSplitViewModel>();
        }

        [Fact]
        public void GetSplitDetailsFromUser_Throws_WhenOrderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.GetSplitDetailsFromUser(null));
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("Foo")]
        public void GetSplitDetailsFromUser_SetsData_WhenOrderIsNotNull(string orderNumber)
        {
            OrderEntity order = new OrderEntity();
            order.ChangeOrderNumber(orderNumber);
            testObject.GetSplitDetailsFromUser(order);

            Assert.Equal(orderNumber, testObject.SelectedOrderNumber);
            Assert.Equal("-1", testObject.OrderNumberPostfix);
        }

        [Fact]
        public void GetSplitDetailsFromUser_DataContext_IsSetToViewModel()
        {
            testObject.GetSplitDetailsFromUser(new OrderEntity());
            mock.Mock<IOrderSplitDialog>().VerifySet(x => x.DataContext = testObject);
        }

        [Fact]
        public void GetSplitDetailsFromUser_CallShowDialog_OnMessageHelper()
        {
            testObject.GetSplitDetailsFromUser(new OrderEntity());
            var dialog = mock.Mock<IOrderSplitDialog>();
            mock.Mock<IMessageHelper>().Verify(x => x.ShowDialog(dialog.Object));
        }

        [Fact]
        public void GetSplitDetailFromUser_ReturnsFailure_WhenDialogIsCancelled()
        {
            var details = testObject.GetSplitDetailsFromUser(new OrderEntity());

            Assert.True(details.Failure);
        }

        [Fact]
        public void GetSplitDetailFromUser_ReturnsSuccess_WhenDialogIsNotCancelled()
        {
            mock.Mock<IMessageHelper>().Setup(x => x.ShowDialog(It.IsAny<IOrderSplitDialog>())).Returns(true);
            var details = testObject.GetSplitDetailsFromUser(new OrderEntity());

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
            var details = testObject.GetSplitDetailsFromUser(new OrderEntity());

            Assert.Equal("123456-2", details.Value.NewOrderNumber);
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
