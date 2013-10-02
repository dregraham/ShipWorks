﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.ScanForms;
using Moq;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Tests.Shipping.ScanForms
{
    [TestClass]
    public class ScanFormBatchTest
    {
        private ScanFormBatch testObject;

        private Mock<IScanFormBatchPrinter> batchPrinter;
        private Mock<IScanFormPrinter> formPrinter;

        private Mock<IScanFormCarrierAccount> carrierAccount;
        private Mock<IScanFormGateway> gateway;

        private Mock<IWin32Window> window;

        [TestInitialize]
        public void Initialize()
        {
            batchPrinter = new Mock<IScanFormBatchPrinter>();
            batchPrinter.Setup(p => p.Print(It.IsAny<IWin32Window>(), It.IsAny<ScanFormBatch>())).Returns(true);

            formPrinter = new Mock<IScanFormPrinter>();
            formPrinter.Setup(p => p.Print(It.IsAny<IWin32Window>(), It.IsAny<ScanForm>())).Returns(true);

            gateway = new Mock<IScanFormGateway>();
            gateway.Setup(g => g.CreateScanForms(It.IsAny<ScanFormBatch>(), It.IsAny<IEnumerable<ShipmentEntity>>()))
                                .Returns((ScanFormBatch form, IEnumerable<ShipmentEntity> shipments) => new List<IEntity2> { new EndiciaScanFormEntity() });

            carrierAccount = new Mock<IScanFormCarrierAccount>();
            carrierAccount.Setup(a => a.GetPrinter()).Returns(formPrinter.Object);
            carrierAccount.Setup(a => a.GetGateway()).Returns(gateway.Object);
            carrierAccount.Setup(a => a.Save(It.IsAny<ScanFormBatch>())).Returns(10001);

            window = new Mock<IWin32Window>();

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object);
        }

        [TestMethod]
        public void Create_ScanFormsListCountIsZero_WhenShipmentsListIsEmpty_Test()
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            testObject.Create(shipments);

            Assert.AreEqual(0, testObject.ScanForms.Count());
        }

        [TestMethod]
        public void Create_ScanFormsListCountIsZero_WhenShipmentsListIsNull_Test()
        {
            List<ShipmentEntity> shipments = null;

            testObject.Create(shipments);

            Assert.AreEqual(0, testObject.ScanForms.Count());
        }

        //[TestMethod]
        //public void Create_ScanFormsListCountIsOne_WhenCarrierIsNotExpress1_AndShipmentsListDoesNotContainCubicPackages_Test()
        //{
        //    List<ShipmentEntity> shipments = new List<ShipmentEntity>
        //    {
        //        new ShipmentEntity()
        //        {
        //            ShipmentType = (int) ShipmentTypeCode.Stamps,
        //            Postal = new PostalShipmentEntity
        //            {
        //                PackagingType = (int) PostalPackagingType.FlatRateEnvelope
        //            }
        //        },
        //        new ShipmentEntity()
        //        {
        //            ShipmentType = (int) ShipmentTypeCode.Stamps,
        //            Postal = new PostalShipmentEntity
        //            {
        //                PackagingType = (int) PostalPackagingType.Package
        //            }
        //        },
        //        new ShipmentEntity()
        //        {
        //            ShipmentType = (int) ShipmentTypeCode.Stamps,
        //            Postal = new PostalShipmentEntity
        //            {
        //                PackagingType = (int) PostalPackagingType.FlatRateLargeBox
        //            }
        //        }
        //    };

        //    testObject.Create(shipments);

        //    Assert.AreEqual(1, testObject.ScanForms.Count());
        //}

        [TestMethod]
        public void Create_DelegatesCreationToGateway()
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>
                {
                    new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.Stamps }
                };

            testObject.Create(shipments);

            gateway.Verify(x => x.CreateScanForms(testObject, shipments));
        }

        [TestMethod]
        public void Create_DelegatesSaveToCarrierAccount()
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>
                {
                    new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.Stamps }
                };

            testObject.Create(shipments);

            carrierAccount.Verify(x => x.Save(testObject));
        }

        [TestMethod]
        public void CreateScanForm_InstantiatesScanForm()
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            byte[] image = new byte[10];

            ScanForm result = testObject.CreateScanForm("Test", shipments, null, image);

            Assert.AreEqual(carrierAccount.Object, result.CarrierAccount);
            Assert.AreEqual("Test", result.Description);
            Assert.AreEqual(shipments, result.Shipments);
            Assert.AreEqual(image, result.Image);
        }

        [TestMethod]
        public void CreateScanForm_AddsScanFormToList()
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            ScanForm result = testObject.CreateScanForm("Test", shipments, null, null);

            Assert.AreEqual(1, testObject.ScanForms.Count());
            Assert.AreEqual(result, testObject.ScanForms.ElementAt(0));
        }



        //[TestMethod]
        //public void Create_SeparatesCubicPackages_WhenCarrierIsExpress1_AndShipmentsListContainsCubicPackages_Test()
        //{
        //    List<ShipmentEntity> shipments = new List<ShipmentEntity>
        //    {
        //        new ShipmentEntity()
        //        {
        //            ShipmentType = (int) ShipmentTypeCode.Express1Endicia,
        //            Postal = new PostalShipmentEntity
        //            {
        //                PackagingType = (int) PostalPackagingType.Cubic
        //            }
        //        },
        //        new ShipmentEntity()
        //        {
        //            ShipmentType = (int) ShipmentTypeCode.Express1Endicia,
        //            Postal = new PostalShipmentEntity
        //            {
        //                PackagingType = (int) PostalPackagingType.Cubic
        //            }
        //        },
        //        new ShipmentEntity()
        //        {
        //            ShipmentType = (int) ShipmentTypeCode.Express1Endicia,
        //            Postal = new PostalShipmentEntity
        //            {
        //                PackagingType = (int) PostalPackagingType.FlatRateLargeBox
        //            }
        //        }
        //    };

        //    testObject.Create(shipments);
            
        //    // A bit kludgy since we're depending on the order that scan forms get added to the list.
        //    // Would be ideal to be able to find the list containing the cubic packages
        //    EndiciaScanFormEntity cubicScanFormEntity = (EndiciaScanFormEntity) testObject.ScanForms.ToList()[0].ScanFormEntity;
        //    EndiciaScanFormEntity allOtherScanFormEntity = (EndiciaScanFormEntity)testObject.ScanForms.ToList()[1].ScanFormEntity;

        //    // Can do this since the property on the entity is being assigned based on the shipment count of the shipment list
        //    // provided to the mocked gateway
        //    Assert.AreEqual(2, cubicScanFormEntity.ShipmentCount);
        //    Assert.AreEqual(1, allOtherScanFormEntity.ShipmentCount);
        //}

        [TestMethod]
        public void Print_DelegatesToBatchPrinter_WhenBatchSizeIsGreaterThanOne_Test()
        {
            long batchId = 1000;
            string batchName = "unit test batch";

            List<ScanForm> scanForms = new List<ScanForm>
            {
                new ScanForm(carrierAccount.Object, batchId, batchName),
                new ScanForm(carrierAccount.Object, batchId, batchName)
            };

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms);
            testObject.Print(window.Object);

            batchPrinter.Verify(p => p.Print(window.Object, testObject), Times.Once());
        }

        [TestMethod]
        public void Print_DoesNotDelegateToScanFormPrinter_WhenBatchSizeIsGreaterThanOne_Test()
        {
            long batchId = 1000;
            string batchName = "unit test batch";

            List<ScanForm> scanForms = new List<ScanForm>
            {
                new ScanForm(carrierAccount.Object, batchId, batchName),
                new ScanForm(carrierAccount.Object, batchId, batchName)
            };

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms);
            testObject.Print(window.Object);

            formPrinter.Verify(p => p.Print(window.Object, It.IsAny<ScanForm>()), Times.Never());
        }

        [TestMethod]
        public void Print_DelegatesToScanFormPrinter_WhenBatchSizeIsOne_Test()
        {
            long batchId = 1000;
            string batchName = "unit test batch";

            List<ScanForm> scanForms = new List<ScanForm>
            {
                new ScanForm(carrierAccount.Object, batchId, batchName)
            };

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms);
            testObject.Print(window.Object);

            formPrinter.Verify(p => p.Print(window.Object, scanForms[0]), Times.Once());
        }

        [TestMethod]
        public void Print_DoesNotDelegateToBatchPrinter_WhenBatchSizeIsOne_Test()
        {
            long batchId = 1000;
            string batchName = "unit test batch";

            List<ScanForm> scanForms = new List<ScanForm>
            {
                new ScanForm(carrierAccount.Object, batchId, batchName)
            };

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms);
            testObject.Print(window.Object);

            batchPrinter.Verify(p => p.Print(window.Object, It.IsAny<ScanFormBatch>()), Times.Never());
        }

        [TestMethod]
        public void Print_DelegatesToScanFormPrinter_WhenBatchSizeIsZero_Test()
        {
            List<ScanForm> scanForms = new List<ScanForm>();

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms);
            testObject.Print(window.Object);

            formPrinter.Verify(p => p.Print(window.Object, It.IsAny<ScanForm>()), Times.Never());
        }

        [TestMethod]
        public void Print_DoesNotDelegateToBatchPrinter_WhenBatchSizeIsZero_Test()
        {
            List<ScanForm> scanForms = new List<ScanForm>();

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms);
            testObject.Print(window.Object);

            batchPrinter.Verify(p => p.Print(window.Object, It.IsAny<ScanFormBatch>()), Times.Never());
        }

        [TestMethod]
        public void Print_ReturnsTrue_WhenBatchSizeIsZero_Test()
        {
            List<ScanForm> scanForms = new List<ScanForm>();

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms);
            bool printed = testObject.Print(window.Object);

            Assert.IsTrue(printed);
        }

        [TestMethod]
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

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms);
            bool printed = testObject.Print(window.Object);

            Assert.IsTrue(printed);
        }

        [TestMethod]
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

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms);
            bool printed = testObject.Print(window.Object);

            Assert.IsFalse(printed);
        }

        [TestMethod]
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

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms);
            bool printed = testObject.Print(window.Object);

            Assert.IsTrue(printed);
        }

        [TestMethod]
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

            testObject = new ScanFormBatch(carrierAccount.Object, batchPrinter.Object, scanForms);
            bool printed = testObject.Print(window.Object);

            Assert.IsFalse(printed);
        }
    }
}
