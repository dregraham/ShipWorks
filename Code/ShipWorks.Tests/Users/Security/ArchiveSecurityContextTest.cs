using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Security;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Users.Security
{
    public class ArchiveSecurityContextTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ArchiveSecurityContext testObject;
        private readonly ITestOutputHelper testOutputHelper;

        public ArchiveSecurityContextTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ArchiveSecurityContext>();
        }

        [Fact]
        public void HasPermission_ReturnsFalse_WhenPermissionIsForModification()
        {
            mock.Mock<ISecurityContext>()
                .Setup(x => x.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>()))
                .Returns(true);

            var failures = Enum.GetValues(typeof(PermissionType))
                .OfType<PermissionType>()
                .Except(allowedPermissions)
                .Select(x => (permission: x, result: testObject.HasPermission(x, null)))
                .Where(x => x.result);

            foreach (var (permission, result) in failures)
            {
                testOutputHelper.WriteLine($"Permission {permission} should return false");
            }

            Assert.Empty(failures);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void HasPermission_ReturnsSecurityContextValue_WhenPermissionIsAllowed(bool expected)
        {
            mock.Mock<ISecurityContext>()
                .Setup(x => x.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>()))
                .Returns(expected);

            var failures = allowedPermissions
                .Select(x => (permission: x, result: testObject.HasPermission(x, null)))
                .Where(x => x.result != expected);

            foreach (var (permission, result) in failures)
            {
                testOutputHelper.WriteLine($"Permission {permission} should return {expected}");
            }

            Assert.Empty(failures);
        }

        private readonly HashSet<PermissionType> allowedPermissions = new HashSet<PermissionType>
        {
            PermissionType.ManageFilters,
            PermissionType.ManageUsers,
            PermissionType.ManageTemplates,
            PermissionType.OrdersViewPaymentData,
            PermissionType.DatabaseBackup,
            PermissionType.DatabaseSetup,
        };

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
