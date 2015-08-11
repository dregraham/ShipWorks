using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Tests.Shipping.Settings
{
    public class EnumCheckboxControlTest
    {
        private EnumCheckBoxControl<FedExServiceType> testObject;

        private List<FedExServiceType> fedExServiceTypes;
            
        [TestInitialize]
        public void Initialize()
        {
            testObject = new EnumCheckBoxControl<FedExServiceType>();

            fedExServiceTypes = Enum.GetValues(typeof (FedExServiceType)).Cast<FedExServiceType>().ToList();
            testObject.Initialize(fedExServiceTypes, new List<FedExServiceType>());
        }

        [Fact]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Constructor_ThrowsInvalidOperationException_WhenTypeProvidedIsNotEnum_Test()
        {
            EnumCheckBoxControl<int> control = new EnumCheckBoxControl<int>();
        }
        
        [Fact]
        public void Initialize_ExcludesSpecificServicesDefinedInExcludedList_Test()
        {
            testObject.Initialize(fedExServiceTypes, new List<FedExServiceType> { FedExServiceType.FedEx1DayFreight, FedExServiceType.FedEx2DayFreight });

            Assert.AreEqual(2, testObject.ExcludedEnumValues.Count());
            Assert.IsTrue(testObject.ExcludedEnumValues.Contains(FedExServiceType.FedEx1DayFreight));
            Assert.IsTrue(testObject.ExcludedEnumValues.Contains(FedExServiceType.FedEx2DayFreight));
        }
    }
}
