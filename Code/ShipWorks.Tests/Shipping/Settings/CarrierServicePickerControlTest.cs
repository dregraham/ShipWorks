﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Tests.Shipping.Settings
{
    [TestClass]
    public class CarrierServicePickerControlTest
    {
        private CarrierServicePickerControl<FedExServiceType> testObject;

        private List<FedExServiceType> fedExServiceTypes;
            
        [TestInitialize]
        public void Initialize()
        {
            testObject = new CarrierServicePickerControl<FedExServiceType>();

            fedExServiceTypes = Enum.GetValues(typeof (FedExServiceType)).Cast<FedExServiceType>().ToList();
            testObject.Initialize(fedExServiceTypes, new List<FedExServiceType>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Constructor_ThrowsInvalidOperationException_WhenTypeProvidedIsNotEnum_Test()
        {
            CarrierServicePickerControl<int> control = new CarrierServicePickerControl<int>();
        }
        
        [TestMethod]
        public void Initialize_ExcludesSpecificServicesDefinedInExcludedList_Test()
        {
            testObject.Initialize(fedExServiceTypes, new List<FedExServiceType> { FedExServiceType.FedEx1DayFreight, FedExServiceType.FedEx2DayFreight });

            Assert.AreEqual(2, testObject.ExcludedServiceTypes.Count());
            Assert.IsTrue(testObject.ExcludedServiceTypes.Contains(FedExServiceType.FedEx1DayFreight));
            Assert.IsTrue(testObject.ExcludedServiceTypes.Contains(FedExServiceType.FedEx2DayFreight));
        }
    }
}
