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

        [TestInitialize]
        public void Initialize()
        {
            carrierAccount = new Mock<IScanFormCarrierAccount>();
            carrierAccount.Setup(c => c.GetAccountEntity()).Returns(new UspsAccountEntity());

            scanFormBatch = new ScanFormBatch(carrierAccount.Object, null, null);

            testObject = new UspsScanFormGateway(new UspsWebClient(UspsResellerType.None));
        }

        [Fact]
        [ExpectedException(typeof(UspsException))]
        public void CreateScanForms_ThrowsUspsException_WhenAccountEntityIsNull_Test()
        {
            // Setup the GetAccountEntity method to return a null value
            carrierAccount.Setup(c => c.GetAccountEntity()).Returns((IEntity2)null);

            testObject.CreateScanForms(scanFormBatch, new List<ShipmentEntity>());
        }

        [Fact]
        [ExpectedException(typeof(UspsException))]
        public void CreateScanForms_ThrowsUspsException_WhenShipmentsContainNonUspsShipment_Test()
        {
            // Create an Endicia shipment to get the gateway to throw an exception
            List<ShipmentEntity> shipments = new List<ShipmentEntity>()
            {
                new ShipmentEntity()
                {
                    Postal = new PostalShipmentEntity() { Endicia = new EndiciaShipmentEntity() }
                }
            };

            testObject.CreateScanForms(scanFormBatch, shipments);
        }

        [Fact]
        [ExpectedException(typeof(UspsException))]
        public void CreateScanForms_ThrowsUspsException_WhenShipmentsIsNull_Test()
        {
            testObject.CreateScanForms(scanFormBatch, null);
        }

        [Fact]
        [ExpectedException(typeof(UspsException))]
        public void CreateScanForms_ThrowsUspsException_WhenShipmentsIsEmpty_Test()
        {
            testObject.CreateScanForms(scanFormBatch, new List<ShipmentEntity>());
        }

        // Can't effectively unit test the rest of this class since it is calling into 
        // an external dependency that cannot be abstracted
    }
}
