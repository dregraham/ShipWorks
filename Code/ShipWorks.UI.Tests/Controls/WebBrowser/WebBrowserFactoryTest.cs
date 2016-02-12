﻿using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.UI.Controls.WebBrowser;
using Xunit;

namespace ShipWorks.UI.Tests.Controls.WebBrowser
{
    public class WebBrowserFactoryTest
    {
        [Fact]
        public void Create_LoadsViewModelWithUriAndTitle()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var viewModel = mock.Mock<IWebBrowserDlgViewModel>();

                var webBrowserFactory = mock.Create<WebBrowserFactory>();

                Uri uri = new Uri("http://www.shipworks.com");
                webBrowserFactory.Create(uri, "title", null);

                viewModel.Verify(v => v.Load(uri, "title"), Times.Once);
            }
        }
    }
}
