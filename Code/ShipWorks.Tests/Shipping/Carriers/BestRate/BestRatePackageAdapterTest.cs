using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    public class BestRatePackageAdapterTest
    {
        /// <summary>
        /// To support ShipSense, we do not want to have PackagingType added to the hash of the package adapter.
        /// </summary>
        [Fact]
        public void Hash_DoesNotContain_PackagingType_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                BestRate = new BestRateShipmentEntity()
            };

            BestRatePackageAdapter testObject = new BestRatePackageAdapter(shipment)
            {
                PackagingType = 999999,
                AdditionalWeight = 3.1,
                ApplyAdditionalWeight = true,
                Height = 2.2,
                Length = 3.3,
                Weight = 4.4,
                Width = 5.5
            };

            string firstTry = testObject.HashCode();

            testObject.PackagingType = 0;
            string secondTry = testObject.HashCode();

            // Make sure PackagingType WAS NOT PART OF THE HASH!
            Assert.Equal(firstTry, secondTry);
        }
    }
}
