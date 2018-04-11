using System;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Administration;
using ShipWorks.Stores.Orders.Archive;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Orders.Archive
{
    public class SingleDatabaseSelectorViewModelTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly SingleDatabaseSelectorViewModel testObject;

        public SingleDatabaseSelectorViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<SingleDatabaseSelectorViewModel>();
        }

        [Fact]
        public void SelectSingleDatabase_ReturnsNull_WhenCollectionIsEmpty()
        {
            var result = testObject.SelectSingleDatabase(Enumerable.Empty<ISqlDatabaseDetail>());
            Assert.Null(result);
        }

        [Fact]
        public void SelectSingleDatabase_ReturnsDatabaseDetails_WhenCollectionHasOneItem()
        {
            var details = mock.Build<ISqlDatabaseDetail>();
            var result = testObject.SelectSingleDatabase(new[] { details });
            Assert.Equal(details, result);
        }

        [Fact]
        public void SelectSingleDatabase_OpensDialog_WhenCollectionHasMultipleItems()
        {
            var dialog = mock.Mock<ISingleDatabaseSelectorDialog>();

            testObject.SelectSingleDatabase(new[] { mock.Build<ISqlDatabaseDetail>(), mock.Build<ISqlDatabaseDetail>() });

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowDialog(dialog.Object));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(false)]
        public void SelectSingleDatabase_ReturnsNull_WhenDialogIsCanceled(bool? dialogResult)
        {
            mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<IDialog>()))
                .Returns(dialogResult);

            testObject.SelectedDatabase = mock.Build<ISqlDatabaseDetail>();

            var result = testObject.SelectSingleDatabase(new[] { mock.Build<ISqlDatabaseDetail>(), mock.Build<ISqlDatabaseDetail>() });

            Assert.Null(result);
        }

        [Fact]
        public void SelectSingleDatabase_ReturnsNull_WhenDialogIsAcceptedButThereIsNoSelectedDatabase()
        {
            mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<IDialog>()))
                .Returns(true);

            testObject.SelectedDatabase = null;

            var result = testObject.SelectSingleDatabase(new[] { mock.Build<ISqlDatabaseDetail>(), mock.Build<ISqlDatabaseDetail>() });

            Assert.Null(result);
        }

        [Fact]
        public void SelectSingleDatabase_ReturnsSelectedDatabase_WhenDialogIsAccepted()
        {
            mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<IDialog>()))
                .Returns(true);

            var expectedDetail = mock.Build<ISqlDatabaseDetail>();
            testObject.SelectedDatabase = expectedDetail;

            var result = testObject.SelectSingleDatabase(new[] { mock.Build<ISqlDatabaseDetail>(), expectedDetail });

            Assert.Equal(expectedDetail, result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
