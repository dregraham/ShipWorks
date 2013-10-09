﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Shipping.Api;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx;
using ProcessShipmentRequest = ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExDryIceManipulatorTest
    {
        private FedExDryIceManipulator testObject;

        private ShipmentEntity shipmentEntity;

        private FedExShipRequest shipRequest;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        [TestInitialize]
        public void Initialize()
        {
            settingsRepository = new Mock<ICarrierSettingsRepository>();

            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            // All dry ice shipments need to use custom packaging
            shipmentEntity.FedEx.PackagingType = (int) FedExPackagingType.Custom;

            shipRequest = new FedExShipRequest(
                null,
                shipmentEntity,
                null,
                null, 
                settingsRepository.Object,
                new ProcessShipmentRequest());

            testObject=new FedExDryIceManipulator();

            shipmentEntity.FedEx.Packages[0].DryIceWeight = 2.2046; //1KG
            shipmentEntity.FedEx.Packages[1].DryIceWeight = 4.4092; //2KG

            shipmentEntity.FedEx.Service = (int) FedExServiceType.FedExGround;
        }

        [TestMethod]
        public void Manipulate_DryIceShipmentWeightIsThree_ShipmentEntityHasThreeKgOfDryIce_Test()
        {
            testObject.Manipulate(shipRequest);

            Assert.AreEqual(3,ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.ShipmentDryIceDetail.TotalWeight.Value);
        }

        [TestMethod]
        public void Manipulate_DryIceShipmentWeightIsInKg_ShipmentEntityHasThreeKgOfDryIce_Test()
        {
            testObject.Manipulate(shipRequest);

            Assert.AreEqual(WeightUnits.KG, ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.ShipmentDryIceDetail.TotalWeight.Units);
        }

        [TestMethod]
        public void Manipulate_DryIceInShipmentSpecialServiceTypes_ShipmentEntityHasThreeKgOfDryIce_Test()
        {
            testObject.Manipulate(shipRequest);

            Assert.AreEqual(1, ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Count(t => t == ShipmentSpecialServiceType.DRY_ICE));
        }

        [TestMethod]
        public void Manipulate_HasOneShipmentSpecialServiceType_ShipmentEntityHasThreeKgOfDryIce_Test()
        {
            testObject.Manipulate(shipRequest);

            Assert.AreEqual(1, ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Count());
        }

        [TestMethod]
        public void Manipulate_DryIceInPackageSpecialServiceType_WhenNotUsingGroundService_ShipmentEntityHasThreeKgOfDryIce_Test()
        {
            shipmentEntity.FedEx.Service = (int) FedExServiceType.PriorityOvernight;

            testObject.Manipulate(shipRequest);

            Assert.AreEqual(1, ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.Count(t=> t == PackageSpecialServiceType.DRY_ICE));
        }

        [TestMethod]
        public void Manipulate_HasOnePackageSpecialServiceType_WhenNotUsingGroundService_ShipmentEntityHasThreeKgOfDryIce_Test()
        {
            shipmentEntity.FedEx.Service = (int) FedExServiceType.PriorityOvernight;

            testObject.Manipulate(shipRequest);

            Assert.AreEqual(1, ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.Count());
        }

        [TestMethod]
        public void Manipulate_WeightOfPackageIsOne_WhenNotUsingGroundService_ShipmentEntityHasThreeKgOfDryIce_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.PriorityOvernight;

            testObject.Manipulate(shipRequest);

            Assert.AreEqual(1, ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DryIceWeight.Value);
        }

        [TestMethod]
        public void Manipulate_WeightOfPackageIsInKg_FirstPackageHasDryIceWeight_WhenNotUsingGroundService_Test()
        {
            shipmentEntity.FedEx.Service = (int) FedExServiceType.PriorityOvernight;

            testObject.Manipulate(shipRequest);

            Assert.AreEqual(WeightUnits.KG, ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DryIceWeight.Units);
        }

        [TestMethod]
        public void Manipulate_NumberOfPackagesWithDryIceIsTwo_WhenUsingGroundService_ShipmentEntityHasTwoPackagesWithDryIce_Test()
        {
            shipmentEntity.FedEx.Service = (int) FedExServiceType.FedExGround;

            testObject.Manipulate(shipRequest);

            Assert.AreEqual("2", ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.ShipmentDryIceDetail.PackageCount);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenUsingFedExBoxPackagingType_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int) FedExPackagingType.Box;
            testObject.Manipulate(shipRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenUsingEnvelopePackagingType_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Envelope;
            testObject.Manipulate(shipRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenUsingPakPackagingType_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Pak;
            testObject.Manipulate(shipRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenUsingTubePackagingType_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Tube;
            testObject.Manipulate(shipRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenUsing10KgBoxPackagingType_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box10Kg;
            testObject.Manipulate(shipRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenUsing25KgBoxPackagingType_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box25Kg;
            testObject.Manipulate(shipRequest);
        }

        [TestMethod]
        public void Manipulate_DryIceNotAdded_WhenDryIceAmountIs0AndCustomPackagingType_Test()
        {
            shipmentEntity.FedEx.Packages[0].DryIceWeight = 0;
            shipmentEntity.FedEx.Packages[1].DryIceWeight = 0;
            
            testObject.Manipulate(shipRequest);

            Assert.AreEqual(0, ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.Count(t => t == PackageSpecialServiceType.DRY_ICE));
        }

        [TestMethod]
        public void Manipulate_DryIceNotAdded_WhenDryIceAmountIs0AndUsing25KgBoxPackageType_Test()
        {
            shipmentEntity.FedEx.Packages[0].DryIceWeight = 0;
            shipmentEntity.FedEx.Packages[1].DryIceWeight = 0;

            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box25Kg;

            testObject.Manipulate(shipRequest);

            Assert.AreEqual(0, ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.Count(t => t == PackageSpecialServiceType.DRY_ICE));
        }

        /// <summary>
        /// NativeRequest from shipRequest converted to ProcessShipmentRequest
        /// </summary>
        private ProcessShipmentRequest ProcessShipmentRequest
        {
            get
            {
                return shipRequest.NativeRequest as ProcessShipmentRequest;
            }
        }


    }
}
