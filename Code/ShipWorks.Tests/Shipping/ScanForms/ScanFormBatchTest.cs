using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.ScanForms;
using Moq;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Tests.Shipping.ScanForms
{
    public class ScanFormBatchTest
    {
        private ScanFormBatch testObject;

        private Mock<IScanFormBatchPrinter> batchPrinter;
        private Mock<IScanFormPrinter> formPrinter;
        private Mock<IScanFormBatchShipmentRepository> batchShipmentRepository;

        private Mock<IScanFormCarrierAccount> carrierAccount;
        private Mock<IScanFormGateway> gateway;

        private List<ShipmentEntity> oneShipment; 

        
        private Mock<IWin32Window> window;

        public ScanFormBatchTest()
        {
            batchPrinter = new Mock<IScanFormBatchPrinter>();
            batchPrinter.Setup(p => p.Print(It.IsAny<IWin32Window>(), It.IsAny<ScanFormBatch>())).Returns(true);

            formPrinter = new Mock<IScanFormPrinter>();
            formPrinter.Setup(p => p.Print(It.IsAny<IWin32Window>(), It.IsAny<ScanForm>())).Returns(true);

            batchShipmentRepository = new Mock<IScanFormBatchShipmentRepository>();
            batchShipmentRepository.Setup(r => r.GetShipmentCount(It.IsAny<ScanFormBatch>())).Returns(1);

            gateway = new Mock<IScanFormGateway>();
            gateway.Setup(g => g.CreateScanForms(It.IsAny<ScanFormBatch>(), It.IsAny<IEnumerable<ShipmentEntity>>()))
                                .Returns((ScanFormBatch form, IEnumerable<ShipmentEntity> shipments) => new List<IEntity2> { new EndiciaScanFormEntity() });

            carrierAccount = new Mock<IScanFormCarrierAccount>();
            carrierAccount.Setup(a => a.GetPrinter()).Returns(formPrinter.Object);
            carrierAccount.Setup(a => a.GetGateway()).Returns(gateway.Object);
            carrierAccount.Setup(a => a.Save(It.IsAny<ScanFormBatch>())).Returns(10001);

            oneShipment = new List<ShipmentEntity>
            {
                new ShipmentEntity()
                {
                    ShipmentType = (int)ShipmentTypeCode.Usps,
                    Postal = new PostalShipmentEntity()
                    {
                        Service = (int)PostalServiceType.ExpressMail
                    }
                }
            };

            window = new Mock<IWin32Window>();

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, batchShipmentRepository.Object);
        }

        [Fact]
        public void Create_ScanFormsListCountIsZero_WhenShipmentsListIsEmpty_Test()
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            testObject.Create(shipments);

            Assert.Equal(0, testObject.ScanForms.Count());
        }

        [Fact]
        public void Create_ScanFormsListCountIsZero_WhenShipmentsListIsNull_Test()
        {
            List<ShipmentEntity> shipments = null;

            testObject.Create(shipments);

            Assert.Equal(0, testObject.ScanForms.Count());
        }

        [Fact]
        public void Create_DelegatesCreationToGateway_Test()
        {
            testObject.Create(oneShipment);

            gateway.Verify(x => x.CreateScanForms(testObject, oneShipment));
        }

        [Fact]
        public void Create_AssignsShipmentCount_Test()
        {
            testObject.Create(oneShipment);

            Assert.Equal(1, testObject.ShipmentCount);
        }

        [Fact]
        public void Create_DelegatesSaveToCarrierAccount_Test()
        {
            testObject.Create(oneShipment);

            carrierAccount.Verify(x => x.Save(testObject));
        }

        [Fact]
        public void Create_SeparatesPostalAndDhlShipments_WhenSendingToGateway_Test()
        {
            ShipmentEntity firstDhlShipment = new ShipmentEntity()
            {
                OriginPostalCode = "63102",
                Postal = new PostalShipmentEntity
                {
                    Service = (int) PostalServiceType.DhlBpmGround
                }
            };

            ShipmentEntity secondDhlShipment = new ShipmentEntity()
            {
                OriginPostalCode = "63102",
                Postal = new PostalShipmentEntity
                {
                    Service = (int)PostalServiceType.DhlCatalogGround
                }
            };

            ShipmentEntity firstPostalShipment = new ShipmentEntity()
            {
                OriginPostalCode = "63102",
                Postal = new PostalShipmentEntity
                {
                    Service = (int)PostalServiceType.PriorityMail
                }
            };

            ShipmentEntity secondPostalShipment = new ShipmentEntity()
            {
                OriginPostalCode = "63102",
                Postal = new PostalShipmentEntity
                {
                    Service = (int)PostalServiceType.FirstClass
                }
            };

            List<ShipmentEntity> shipments = new List<ShipmentEntity>
            {
                firstDhlShipment,
                firstPostalShipment,
                secondPostalShipment,
                secondDhlShipment
            };

            testObject.Create(shipments);

            gateway.Verify(g => g.CreateScanForms(testObject, It.IsAny<IEnumerable<ShipmentEntity>>()), Times.Exactly(2));

            // Verify the two calls made to the gateway separated the Postal and DHL services
            gateway.Verify(g => g.CreateScanForms(testObject, It.Is<IEnumerable<ShipmentEntity>>(batch => batch.All(s => s.Postal.Service == (int)PostalServiceType.FirstClass || s.Postal.Service == (int)PostalServiceType.PriorityMail))), Times.Once());
            gateway.Verify(g => g.CreateScanForms(testObject, It.Is<IEnumerable<ShipmentEntity>>(batch => batch.All(s => s.Postal.Service == (int)PostalServiceType.DhlBpmGround || s.Postal.Service == (int)PostalServiceType.DhlCatalogGround))), Times.Once());
        }

        [Fact]
        public void CreateScanForm_InstantiatesScanForm_WithSingleImage_Test()
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            byte[] image = new byte[10];

            ScanForm result = testObject.CreateScanForm("Test", shipments, null, new List<byte[]> { image });

            Assert.Equal(carrierAccount.Object, result.CarrierAccount);
            Assert.Equal("Test", result.Description);
            Assert.Equal(shipments, result.Shipments);
            Assert.Equal(image, result.Images[0]);
        }

        [Fact]
        public void CreateScanForm_InstantiatesScanForm_WithMultipleImages_Test()
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            byte[] image1 = new byte[10];
            byte[] image2 = new byte[40];

            ScanForm result = testObject.CreateScanForm("Test", shipments, null, new List<byte[]> { image1, image2 });

            Assert.Equal(carrierAccount.Object, result.CarrierAccount);
            Assert.Equal("Test", result.Description);
            Assert.Equal(shipments, result.Shipments);
            Assert.Equal(image1, result.Images[0]);
            Assert.Equal(image2, result.Images[1]);
        }

        [Fact]
        public void CreateScanForm_AddsScanFormToList_Test()
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            ScanForm result = testObject.CreateScanForm("Test", shipments, null, null);

            Assert.Equal(1, testObject.ScanForms.Count());
            Assert.Equal(result, testObject.ScanForms.ElementAt(0));
        }
        
        [Fact]
        public void Print_DelegatesToBatchPrinter_WhenBatchSizeIsGreaterThanOne_Test()
        {
            long batchId = 1000;
            string batchName = "unit test batch";

            List<ScanForm> scanForms = new List<ScanForm>
            {
                new ScanForm(carrierAccount.Object, batchId, batchName),
                new ScanForm(carrierAccount.Object, batchId, batchName)
            };

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms, batchShipmentRepository.Object);
            testObject.Print(window.Object);

            batchPrinter.Verify(p => p.Print(window.Object, testObject), Times.Once());
        }

        [Fact]
        public void Print_DoesNotDelegateToScanFormPrinter_WhenBatchSizeIsGreaterThanOne_Test()
        {
            long batchId = 1000;
            string batchName = "unit test batch";

            List<ScanForm> scanForms = new List<ScanForm>
            {
                new ScanForm(carrierAccount.Object, batchId, batchName),
                new ScanForm(carrierAccount.Object, batchId, batchName)
            };

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms, batchShipmentRepository.Object);
            testObject.Print(window.Object);

            formPrinter.Verify(p => p.Print(window.Object, It.IsAny<ScanForm>()), Times.Never());
        }

        [Fact]
        public void Print_DelegatesToScanFormPrinter_WhenBatchSizeIsOne_Test()
        {
            long batchId = 1000;
            string batchName = "unit test batch";

            List<ScanForm> scanForms = new List<ScanForm>
            {
                new ScanForm(carrierAccount.Object, batchId, batchName)
            };

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms, batchShipmentRepository.Object);
            testObject.Print(window.Object);

            formPrinter.Verify(p => p.Print(window.Object, scanForms[0]), Times.Once());
        }

        [Fact]
        public void Print_DoesNotDelegateToBatchPrinter_WhenBatchSizeIsOne_Test()
        {
            long batchId = 1000;
            string batchName = "unit test batch";

            List<ScanForm> scanForms = new List<ScanForm>
            {
                new ScanForm(carrierAccount.Object, batchId, batchName)
            };

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms, batchShipmentRepository.Object);
            testObject.Print(window.Object);

            batchPrinter.Verify(p => p.Print(window.Object, It.IsAny<ScanFormBatch>()), Times.Never());
        }

        [Fact]
        public void Print_DelegatesToScanFormPrinter_WhenBatchSizeIsZero_Test()
        {
            List<ScanForm> scanForms = new List<ScanForm>();

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms, batchShipmentRepository.Object);
            testObject.Print(window.Object);

            formPrinter.Verify(p => p.Print(window.Object, It.IsAny<ScanForm>()), Times.Never());
        }

        [Fact]
        public void Print_DoesNotDelegateToBatchPrinter_WhenBatchSizeIsZero_Test()
        {
            List<ScanForm> scanForms = new List<ScanForm>();

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms, batchShipmentRepository.Object);
            testObject.Print(window.Object);

            batchPrinter.Verify(p => p.Print(window.Object, It.IsAny<ScanFormBatch>()), Times.Never());
        }

        [Fact]
        public void Print_ReturnsTrue_WhenBatchSizeIsZero_Test()
        {
            List<ScanForm> scanForms = new List<ScanForm>();

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms, batchShipmentRepository.Object);
            bool printed = testObject.Print(window.Object);

            Assert.True(printed);
        }

        [Fact]
        public void Print_ReturnsTrue_WhenBatchSizeIsOne_AndScanFormPrinterReturnsTrue_Test()
        {
            long batchId = 1000;
            string batchName = "unit test batch";

            List<ScanForm> scanForms = new List<ScanForm>
            {
                new ScanForm(carrierAccount.Object, batchId, batchName)
            };

            // Already proved that a list of one will delegate to the form printer, so we can test that the 
            // value returned by the printer is the same as the value returned from out test object
            formPrinter.Setup(p => p.Print(It.IsAny<IWin32Window>(), It.IsAny<ScanForm>())).Returns(true);

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms, batchShipmentRepository.Object);
            bool printed = testObject.Print(window.Object);

            Assert.True(printed);
        }

        [Fact]
        public void Print_ReturnsFalse_WhenBatchSizeIsOne_AndScanFormPrinterReturnsFalse_Test()
        {
            long batchId = 1000;
            string batchName = "unit test batch";

            List<ScanForm> scanForms = new List<ScanForm>
            {
                new ScanForm(carrierAccount.Object, batchId, batchName)
            };

            // Already proved that a list of one will delegate to the form printer, so we can test that the 
            // value returned by the printer is the same as the value returned from out test object
            formPrinter.Setup(p => p.Print(It.IsAny<IWin32Window>(), It.IsAny<ScanForm>())).Returns(false);

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms, batchShipmentRepository.Object);
            bool printed = testObject.Print(window.Object);

            Assert.False(printed);
        }

        [Fact]
        public void Print_ReturnsTrue_WhenBatchSizeIsGreaterThanOne_AndScanFormPrinterReturnsTrue_Test()
        {
            long batchId = 1000;
            string batchName = "unit test batch";

            List<ScanForm> scanForms = new List<ScanForm>
            {
                new ScanForm(carrierAccount.Object, batchId, batchName),
                new ScanForm(carrierAccount.Object, batchId, batchName)
            };

            // Already proved that a list of one will delegate to the batch printer, so we can test that the 
            // value returned by the printer is the same as the value returned from out test object
            batchPrinter.Setup(p => p.Print(It.IsAny<IWin32Window>(), It.IsAny<ScanFormBatch>())).Returns(true);

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms, batchShipmentRepository.Object);
            bool printed = testObject.Print(window.Object);

            Assert.True(printed);
        }

        [Fact]
        public void Print_ReturnsFalse_WhenBatchSizeIsGreaterThanOne_AndScanFormPrinterReturnsFalse_Test()
        {
            long batchId = 1000;
            string batchName = "unit test batch";

            List<ScanForm> scanForms = new List<ScanForm>
            {
                new ScanForm(carrierAccount.Object, batchId, batchName),
                new ScanForm(carrierAccount.Object, batchId, batchName)
            };

            // Already proved that a list of one will delegate to the batch printer, so we can test that the 
            // value returned by the printer is the same as the value returned from out test object
            batchPrinter.Setup(p => p.Print(It.IsAny<IWin32Window>(), It.IsAny<ScanFormBatch>())).Returns(false);

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms, batchShipmentRepository.Object);
            bool printed = testObject.Print(window.Object);

            Assert.False(printed);
        }

        [Fact]
        public void ShipmentCount_DelegatesToShipmentRepository_WhenShipmentCountIsZero_Test()
        {
            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, batchShipmentRepository.Object);
            int count = testObject.ShipmentCount;

            batchShipmentRepository.Verify(r => r.GetShipmentCount(testObject), Times.Once());
        }

        [Fact]
        public void ShipmentCount_DoesNotDelegateToShipmentRepository_WhenShipmentCountIsNotZero_Test()
        {

            // Call the create method, so the shipment count will be initialized
            testObject.Create(oneShipment);
            
            batchShipmentRepository.Verify(r => r.GetShipmentCount(testObject), Times.Never());
        }

        [Fact]
        public void ShipmentCount_ProcessDhlInSeperateBatch_WhenDhlAndNonDhlShipmentsPresent_Test()
        {
            List<ShipmentEntity> shipmentsToBatch = new List<ShipmentEntity>
            {
                new ShipmentEntity()
                {
                    ShipmentType = (int)ShipmentTypeCode.Usps,
                    Postal = new PostalShipmentEntity()
                    {
                        Service = (int)PostalServiceType.ExpressMail
                    }
                },
                new ShipmentEntity()
                {
                    ShipmentType = (int)ShipmentTypeCode.Usps,
                    Postal = new PostalShipmentEntity()
                    {
                        Service = (int)PostalServiceType.DhlParcelPlusExpedited
                    }
                },
                new ShipmentEntity()
                {
                    ShipmentType = (int)ShipmentTypeCode.Usps,
                    Postal = new PostalShipmentEntity()
                    {
                        Service = (int)PostalServiceType.DhlParcelPlusGround
                    }
                }
            };
            testObject.Create(shipmentsToBatch);

            gateway.Verify(m => m.CreateScanForms(It.IsAny<ScanFormBatch>(), It.IsAny<IEnumerable<ShipmentEntity>>()), Times.Exactly(2));
        }
    }
}
