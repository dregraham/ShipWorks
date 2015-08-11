using System;
using log4net;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Tests.Stores.Platforms.GenericModule
{
    public class GenericStoreStatusCodeProviderTest
    {
        private static readonly MockRepository mockRepository = new MockRepository(MockBehavior.Loose);

        [Fact]
        public void ConvertCodeValue_ReturnsText_WhenStoreUsesText()
        {
            GenericStoreStatusCodeProvider provider = CreateTestProvider(GenericVariantDataType.Text);

            string codeValue = (string)provider.ConvertCodeValue("Foo");
            Assert.AreEqual("Foo", codeValue);
        }

        [Fact]
        public void ConvertCodeValue_ReturnsNumberAsText_WhenStoreUsesText()
        {
            GenericModuleStoreEntity store = new GenericModuleStoreEntity { ModuleOnlineStatusDataType = (int)GenericVariantDataType.Text };
            GenericStoreStatusCodeProvider provider = new GenericStoreStatusCodeProvider(store);

            string codeValue = (string)provider.ConvertCodeValue(55);
            Assert.AreEqual("55", codeValue);
        }

        [Fact]
        public void ConvertCodeValue_ReturnsNull_WhenStoreUsesTextAndValueIsNull()
        {
            GenericModuleStoreEntity store = new GenericModuleStoreEntity { ModuleOnlineStatusDataType = (int)GenericVariantDataType.Text };
            GenericStoreStatusCodeProvider provider = new GenericStoreStatusCodeProvider(store);

            object codeValue = provider.ConvertCodeValue(null);
            Assert.IsNull(codeValue);
        }

        [Fact]
        public void ConvertCodeValue_LogsWarning_WhenStoreUsesTextAndValueIsNull()
        {
            Mock<ILog> logger = mockRepository.Create<ILog>();
            GenericStoreStatusCodeProvider provider = CreateTestProvider(GenericVariantDataType.Text, logger);

            provider.ConvertCodeValue(null);
            logger.Verify(x => x.Warn(It.IsAny<string>()));
        }

        [Fact]
        public void ConvertCodeValue_ReturnsNumber_WhenStoreUsesNumeric()
        {
            GenericModuleStoreEntity store = new GenericModuleStoreEntity { ModuleOnlineStatusDataType = (int)GenericVariantDataType.Numeric };
            GenericStoreStatusCodeProvider provider = new GenericStoreStatusCodeProvider(store);

            long codeValue = (long)provider.ConvertCodeValue(55);
            Assert.AreEqual(55, codeValue);
        }

        [Fact]
        public void ConvertCodeValue_ReturnsNull_WhenStoreUsesNumericAndValueCannotBeConverted()
        {
            GenericModuleStoreEntity store = new GenericModuleStoreEntity { ModuleOnlineStatusDataType = (int)GenericVariantDataType.Numeric };
            GenericStoreStatusCodeProvider provider = new GenericStoreStatusCodeProvider(store);

            object codeValue = provider.ConvertCodeValue("Foo");
            Assert.IsNull(codeValue);
        }

        [Fact]
        public void ConvertCodeValue_LogsWarning_WhenStoreUsesNumericAndValueCannotBeConverted()
        {
            Mock<ILog> logger = mockRepository.Create<ILog>();
            GenericStoreStatusCodeProvider provider = CreateTestProvider(GenericVariantDataType.Numeric, logger);

            provider.ConvertCodeValue("Foo");
            logger.Verify(x => x.Warn(It.IsAny<string>(), It.IsAny<FormatException>()));
        }

        [Fact]
        public void IsValidCode_ReturnsTrue_WhenTypeAndValueAreText()
        {
            GenericModuleStoreEntity store = new GenericModuleStoreEntity { ModuleOnlineStatusDataType = (int)GenericVariantDataType.Text };
            GenericStoreStatusCodeProvider provider = new GenericStoreStatusCodeProvider(store);

            Assert.IsTrue(provider.IsValidCode("Foo"));
        }

        [Fact]
        public void IsValidCode_ReturnsFalse_WhenTypeIsTextAndValueIsNot()
        {
            GenericModuleStoreEntity store = new GenericModuleStoreEntity { ModuleOnlineStatusDataType = (int)GenericVariantDataType.Text };
            GenericStoreStatusCodeProvider provider = new GenericStoreStatusCodeProvider(store);

            Assert.IsFalse(provider.IsValidCode(55));
        }

        [Fact]
        public void IsValidCode_ReturnsFalse_WhenTypeIsTextAndValueIsNull()
        {
            GenericModuleStoreEntity store = new GenericModuleStoreEntity { ModuleOnlineStatusDataType = (int)GenericVariantDataType.Text };
            GenericStoreStatusCodeProvider provider = new GenericStoreStatusCodeProvider(store);

            Assert.IsFalse(provider.IsValidCode(null));
        }

        [Fact]
        public void IsValidCode_ReturnsTrue_WhenTypeAndValueAreNumeric()
        {
            GenericModuleStoreEntity store = new GenericModuleStoreEntity { ModuleOnlineStatusDataType = (int)GenericVariantDataType.Numeric };
            GenericStoreStatusCodeProvider provider = new GenericStoreStatusCodeProvider(store);

            Assert.IsTrue(provider.IsValidCode(55));
        }

        [Fact]
        public void IsValidCode_ReturnsTrue_WhenTypeAndValueAreNumericAndValueIsLong()
        {
            GenericModuleStoreEntity store = new GenericModuleStoreEntity { ModuleOnlineStatusDataType = (int)GenericVariantDataType.Numeric };
            GenericStoreStatusCodeProvider provider = new GenericStoreStatusCodeProvider(store);

            Assert.IsTrue(provider.IsValidCode(55L));
        }

        [Fact]
        public void IsValidCode_ReturnsFalse_WhenTypeIsNumericAndValueIsNot()
        {
            GenericModuleStoreEntity store = new GenericModuleStoreEntity { ModuleOnlineStatusDataType = (int)GenericVariantDataType.Numeric };
            GenericStoreStatusCodeProvider provider = new GenericStoreStatusCodeProvider(store);

            Assert.IsFalse(provider.IsValidCode("Foo"));
        }

        [Fact]
        public void IsValidCode_ReturnsFalse_WhenTypeIsNumericAndValueIsNull()
        {
            GenericModuleStoreEntity store = new GenericModuleStoreEntity { ModuleOnlineStatusDataType = (int)GenericVariantDataType.Numeric };
            GenericStoreStatusCodeProvider provider = new GenericStoreStatusCodeProvider(store);

            Assert.IsFalse(provider.IsValidCode(null));
        }

        /// <summary>
        /// Create a test provider from a store with the specified status data type
        /// </summary>
        private static GenericStoreStatusCodeProvider CreateTestProvider(GenericVariantDataType statusDataType)
        {
            return CreateTestProvider(statusDataType, mockRepository.Create<ILog>());
        }

        /// <summary>
        /// Create a test provider from a store with the specified status data type and a mocked logger
        /// </summary>
        private static GenericStoreStatusCodeProvider CreateTestProvider(GenericVariantDataType statusDataType, Mock<ILog> logger)
        {
            GenericModuleStoreEntity store = new GenericModuleStoreEntity { ModuleOnlineStatusDataType = (int)statusDataType };
            GenericStoreStatusCodeProvider provider = new GenericStoreStatusCodeProvider(store, logger.Object);
            return provider;
        }
    }
}
