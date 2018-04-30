using System;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Tests.Users.Security
{
    public class SecurityContextFactoryTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly SecurityContextFactory testObject;

        public SecurityContextFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            Func<UserEntity, SecurityContext> createContext = u => new SecurityContext(null, false);
            testObject = mock.Create<SecurityContextFactory>(TypedParameter.From(createContext));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("foo")]
        [InlineData("<ArchivalSettings/>")]
        public void Create_ReturnsSecurityContext_WhenArchiveDataHasNoValue(string xml)
        {
            mock.FromFactory<IConfigurationData>()
                .Mock(x => x.FetchReadOnly())
                .SetupGet(x => x.ArchivalSettingsXml)
                .Returns(xml);

            var context = testObject.Create(new UserEntity());

            Assert.IsAssignableFrom<SecurityContext>(context);
        }

        [Fact]
        public void Create_ReturnsArchiveSecurityContext_WhenArchiveDataHasChildren()
        {
            mock.FromFactory<IConfigurationData>()
                .Mock(x => x.FetchReadOnly())
                .SetupGet(x => x.ArchivalSettingsXml)
                .Returns("<ArchivalSettings><Foo /></ArchivalSettings>");

            var context = testObject.Create(new UserEntity());

            Assert.IsAssignableFrom<ArchiveSecurityContext>(context);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
