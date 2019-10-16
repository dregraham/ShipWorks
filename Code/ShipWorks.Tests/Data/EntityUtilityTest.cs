using System;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using Xunit;

namespace ShipWorks.Tests.Data
{
    public class EntityUtilityTest
    {

        [Fact]
        public void CopyChangedFields_CopiesNewValuesToDestination()
        {
            ShipmentEntity source = new ShipmentEntity(1234)
            {
                Processed = true,
                ShipmentType = (int) ShipmentTypeCode.Usps,
                Postal = new PostalShipmentEntity(1234)
                {
                    Service = 2,
                    Usps = new UspsShipmentEntity(1234)
                    {
                        UspsTransactionID = Guid.NewGuid()
                    }
                }
            };

            // Force shipment type code to not be changed so that we can verify that it is NOT copied over.
            source.Fields[ShipmentFields.ShipmentType.FieldIndex].IsChanged = false;

            ShipmentEntity destination = new ShipmentEntity(1234)
            {
                Processed = false,
                ShipmentType = (int) ShipmentTypeCode.UpsOnLineTools,
                Postal = new PostalShipmentEntity(1234)
                {
                    Service = 2,
                    Usps = new UspsShipmentEntity(1234)
                    {
                        UspsTransactionID = Guid.NewGuid()
                    }
                }
            };

            EntityUtility.CopyChangedFields(source, destination);

            Assert.Equal(source.Processed, destination.Processed);
            Assert.Equal(source.Postal.Service, destination.Postal.Service);
            Assert.Equal(source.Postal.Usps.UspsTransactionID, destination.Postal.Usps.UspsTransactionID);
            Assert.Equal(source.ShipmentID, destination.ShipmentID);

            Assert.Equal((int) ShipmentTypeCode.Usps, source.ShipmentType);
            Assert.Equal((int) ShipmentTypeCode.UpsOnLineTools, destination.ShipmentType);
        }
    }
}
