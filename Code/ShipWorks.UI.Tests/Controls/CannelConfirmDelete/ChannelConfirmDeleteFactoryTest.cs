using Autofac.Extras.Moq;
using ShipWorks.Stores;
using ShipWorks.UI.Controls.ChannelConfirmDelete;
using Xunit;
using Moq;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.UI.Tests.Controls.CannelConfirmDelete
{
    public class ChannelConfirmDeleteFactoryTest
    {
        [Fact]
        public void GetConfirmDeleteDlg_LoadsViewModelWithStoreTypeCode()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDialog>();

                Mock<IConfirmChannelDeleteViewModel> viewModel = mock.Mock<IConfirmChannelDeleteViewModel>();

                IChannelConfirmDeleteFactory testObject = mock.Create<ChannelConfirmDeleteFactory>();

                testObject.GetConfirmDeleteDlg(StoreTypeCode.Amazon, null);

                viewModel.Verify(v => v.Load(StoreTypeCode.Amazon), Times.Once);
            }
        }
    }
}
