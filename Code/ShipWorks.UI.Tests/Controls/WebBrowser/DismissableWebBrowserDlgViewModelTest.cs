using Autofac.Extras.Moq;
using ShipWorks.UI.Controls.WebBrowser;
using System;
using Xunit;

namespace ShipWorks.UI.Tests.Controls.WebBrowser
{
    public class DismissableWebBrowserDlgViewModelTest : IDisposable
    {
        private AutoMock mock;

        public DismissableWebBrowserDlgViewModelTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void Constructor_SetsMoreInfoClickCommand()
        {
            var testObject = mock.Create<DismissableWebBrowserDlgViewModel>();

            Assert.NotNull(testObject.MoreInfoClickCommand);
        }

        [Fact]
        public void Load_SetsUrl()
        {
            var testObject = mock.Create<DismissableWebBrowserDlgViewModel>();

            var uri = new Uri("http://www.google.com");
            testObject.Load(uri, "Title");

            Assert.Equal(uri, testObject.Url);
        }

        [Fact]
        public void Load_SetsTitle()
        {
            var testObject = mock.Create<DismissableWebBrowserDlgViewModel>();

            var uri = new Uri("http://www.google.com");
            testObject.Load(uri, "Title");

            Assert.Equal("Title", testObject.Title);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}