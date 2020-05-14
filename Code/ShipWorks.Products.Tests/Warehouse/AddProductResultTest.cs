using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse;
using ShipWorks.Products.Warehouse.DTO;
using Xunit;

namespace ShipWorks.Products.Tests.Warehouse
{
    public class AddProductResultTest
    {
        [Fact]
        public void ApplyTo_UpdatesVariant()
        {
            var testObject = new AddProductResult(new AddProductResponseData
            {
                ProductId = "00000000-0000-0000-0000-000000000001",
                Version = 7
            });

            var variant = new ProductVariantEntity();

            testObject.ApplyTo(variant);

            Assert.Equal(Guid.Parse("00000000-0000-0000-0000-000000000001"), variant.HubProductId);
            Assert.Equal(7, variant.HubVersion);
        }
    }
}
