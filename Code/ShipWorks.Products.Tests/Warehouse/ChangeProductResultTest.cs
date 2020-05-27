using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse;
using ShipWorks.Products.Warehouse.DTO;
using Xunit;

namespace ShipWorks.Products.Tests.Warehouse
{
    public class ChangeProductResultTest
    {
        [Fact]
        public void ApplyTo_UpdatesVariant()
        {
            var testObject = new ChangeProductResult(new ChangeProductResponseData
            {
                Version = 7
            });

            var variant = new ProductVariantEntity();

            testObject.ApplyTo(variant);

            Assert.Equal(7, variant.HubVersion);
        }
    }
}
