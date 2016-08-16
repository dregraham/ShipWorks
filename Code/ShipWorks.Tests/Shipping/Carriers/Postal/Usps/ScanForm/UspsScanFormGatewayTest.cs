using System.Collections.Generic;
using Xunit;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.ScanForm;
using ShipWorks.Shipping.ScanForms;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.ScanForm
{
    public class UspsScanFormGatewayTest
    {
        private ScanFormBatch scanFormBatch;
        private Mock<IScanFormCarrierAccount> carrierAccount;

        private UspsScanFormGateway testObject;

        public UspsScanFormGatewayTest()
        {
            carrierAccount = new Mock<IScanFormCarrierAccount>();
            carrierAccount.Setup(c => c.GetAccountEntity()).Returns(new UspsAccountEntity());

            scanFormBatch = new ScanFormBatch(carrierAccount.Object, null, null);

            testObject = new UspsScanFormGateway(new UspsWebClient(UspsResellerType.None));
        }

        [Fact]
        public void CreateScanForms_ThrowsUspsException_WhenAccountEntityIsNull()
        {
            // Setup the GetAccountEntity method to return a null value
            carrierAccount.Setup(c => c.GetAccountEntity()).Returns((IEntity2)null);

            Assert.Throws<UspsException>(() => testObject.CreateScanForms(scanFormBatch, new List<ShipmentEntity>()));
        }

        [Fact]
        public void CreateScanForms_ThrowsUspsException_WhenShipmentsContainNonUspsShipment()
        {
            // Create an Endicia shipment to get the gateway to throw an exception
            List<ShipmentEntity> shipments = new List<ShipmentEntity>()
            {
                new ShipmentEntity()
                {
                    Postal = new PostalShipmentEntity() { Endicia = new EndiciaShipmentEntity() }
                }
            };

            Assert.Throws<UspsException>(() => testObject.CreateScanForms(scanFormBatch, shipments));
        }

        [Fact]
        public void CreateScanForms_ThrowsUspsException_WhenShipmentsIsNull()
        {
            Assert.Throws<UspsException>(() => testObject.CreateScanForms(scanFormBatch, null));
        }

        [Fact]
        public void CreateScanForms_ThrowsUspsException_WhenShipmentsIsEmpty()
        {
            Assert.Throws<UspsException>(() => testObject.CreateScanForms(scanFormBatch, new List<ShipmentEntity>()));
        }

        // Can't effectively unit test the rest of this class since it is calling into 
        // an external dependency that cannot be abstracted
    }
}
