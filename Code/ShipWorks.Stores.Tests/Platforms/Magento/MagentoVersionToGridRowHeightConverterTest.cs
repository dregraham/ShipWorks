using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.Stores.UI.Platforms.Magento;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Magento
{
    public class MagentoVersionToGridRowHeightConverterTest
    {
        [Theory]
        [InlineData(MagentoVersion.PhpFile, "*")]
        [InlineData(MagentoVersion.MagentoConnect, "*")]
        [InlineData(MagentoVersion.MagentoTwo, "0")]
        [InlineData(MagentoVersion.MagentoTwoREST, "0")]
        public void Convert_ReturnsAppropriateGridHeight(MagentoVersion version, string expectedHeight)
        {
            var testObject = new MagentoVersionToGridHeightConverter(false);
            var result = (GridLength) testObject.Convert(version, null, null, null);

            Assert.Equal(expectedHeight, result.ToString());
        }
    }
}
