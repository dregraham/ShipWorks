using Interapptive.Shared.Enums;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExPackageDetailsManipulatorTest
    {
        private FedExPackageDetailsManipulator testObject;

        private FedExShipRequest request;

        private ProcessShipmentRequest processShipmentRequest;
        private Mock<ICarrierSettingsRepository> settingsRepository;
        private FedExAccountEntity fedExAccount;

        [TestInitialize]
        public void Initiliaze()
        {
            settingsRepository = new Mock<ICarrierSettingsRepository>();

            request = new FedExShipRequest(
                null,
                BuildFedExShipmentEntity.SetupRequestShipmentEntity(),
                null,
                null, 
                settingsRepository.Object,
                new ProcessShipmentRequest());

            processShipmentRequest = ((ProcessShipmentRequest)request.NativeRequest);

            fedExAccount = new FedExAccountEntity { AccountNumber = "123", CountryCode = "US", LastName = "Doe", FirstName = "John" };
            //carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(fedExAccount);

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(fedExAccount);

            testObject = new FedExPackageDetailsManipulator(new FedExSettings(settingsRepository.Object));
        }

        [Fact]
        public void Manipulate_PackageCountIsTwo_TwoPacakgesInShipment_Test()
        {
            testObject.Manipulate(request);

            Assert.AreEqual(request.ShipmentEntity.FedEx.Packages.Count, 2, "Test request Expected to have two packages");
            Assert.AreEqual(processShipmentRequest.RequestedShipment.PackageCount, "2");
        }

        [Fact]
        public void Manipulate_DimensionsSetProperly_TwoPackagesWithDimensionsInShipment_AndLinearUnitsIsInchesTest()
        {
            request.ShipmentEntity.FedEx.LinearUnitType = (int) FedExLinearUnitOfMeasure.IN;

            testObject.Manipulate(request);

            Assert.AreEqual((int) FedExPackagingType.Custom, request.ShipmentEntity.FedEx.PackagingType, "Expecting Test Data PackagingType to be Custom");

            CompareDimensions(request.ShipmentEntity, request.ShipmentEntity.FedEx.Packages[0], processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [Fact]
        public void Manipulate_DimensionsSetProperly_TwoPackagesWithDimensionsInShipment_AndLinearUnitsIsCentimetersTest()
        {
            request.ShipmentEntity.FedEx.LinearUnitType = (int)FedExLinearUnitOfMeasure.CM;

            testObject.Manipulate(request);

            Assert.AreEqual((int)FedExPackagingType.Custom, request.ShipmentEntity.FedEx.PackagingType, "Expecting Test Data PackagingType to be Custom");

            CompareDimensions(request.ShipmentEntity, request.ShipmentEntity.FedEx.Packages[0], processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [Fact]
        public void Manipulate_WeightSetProperly_TwoPackagesWithWeightInShipment_AndWieghtUnitsIsPounds_Test()
        {
            request.ShipmentEntity.FedEx.WeightUnitType = (int) WeightUnitOfMeasure.Pounds;

            testObject.Manipulate(request);

            ValidateWeight(request.ShipmentEntity, request.ShipmentEntity.FedEx.Packages[0], processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [Fact]
        public void Manipulate_WeightSetProperly_TwoPackagesWithWeightInShipment_AndWieghtUnitsIsKilograms_Test()
        {
            request.ShipmentEntity.FedEx.WeightUnitType = (int)WeightUnitOfMeasure.Kilograms;

            testObject.Manipulate(request);

            ValidateWeight(request.ShipmentEntity, request.ShipmentEntity.FedEx.Packages[0], processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [Fact]
        public void Manipulate_InsuredValueSetProperly_TwoPacakgesWithInsuredValue_Test()
        {
            testObject.Manipulate(request);

            ValidateValue(request.ShipmentEntity.FedEx.Packages[0], processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0]);
            
        }

        [Fact]
        public void Manipulate_AssignsSequenceNumber_Test()
        {
            request.SequenceNumber = 0;

            testObject.Manipulate(request);

            ProcessShipmentRequest nativeRequest = request.NativeRequest as ProcessShipmentRequest;
            Assert.AreEqual(1.ToString(), nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SequenceNumber);
        }

        private void ValidateValue(FedExPackageEntity fedExPackageEntity, RequestedPackageLineItem requestedPackageLineItem)
        {
            Assert.AreEqual("USD", requestedPackageLineItem.InsuredValue.Currency.ToString());

            Assert.AreEqual(fedExPackageEntity.DeclaredValue,requestedPackageLineItem.InsuredValue.Amount);
        }

        /// <summary>
        /// Compare weight of request and entity
        /// </summary>
        /// <param name="fedExPackageEntity"></param>
        /// <param name="requestedPackageLineItem"></param>
        private void ValidateWeight(ShipmentEntity shipment, FedExPackageEntity fedExPackageEntity, RequestedPackageLineItem requestedPackageLineItem)
        {
            decimal packageEntityWeight = FedExUtility.GetPackageTotalWeight(fedExPackageEntity);
            Assert.AreEqual(packageEntityWeight, requestedPackageLineItem.Weight.Value);

            if (shipment.FedEx.WeightUnitType == (int)WeightUnitOfMeasure.Pounds)
            {
                Assert.AreEqual(WeightUnits.LB, requestedPackageLineItem.Weight.Units);
            }
            else
            {
                Assert.AreEqual(WeightUnits.KG, requestedPackageLineItem.Weight.Units);
            }
        }

        /// <summary>
        /// Compare dimensions of request and entity
        /// </summary>
        private static void CompareDimensions(ShipmentEntity shipment, FedExPackageEntity fedExPackageEntity, RequestedPackageLineItem requestedPackage)
        {
            if (shipment.FedEx.LinearUnitType == (int)FedExLinearUnitOfMeasure.CM)
            {
                Assert.AreEqual(LinearUnits.CM, requestedPackage.Dimensions.Units);
            }
            else
            {
                Assert.AreEqual(LinearUnits.IN, requestedPackage.Dimensions.Units);
            }
            Assert.AreEqual(fedExPackageEntity.DimsLength.ToString(), requestedPackage.Dimensions.Length);
            Assert.AreEqual(fedExPackageEntity.DimsWidth.ToString(), requestedPackage.Dimensions.Width);
            Assert.AreEqual(fedExPackageEntity.DimsHeight.ToString(), requestedPackage.Dimensions.Height);
        }
    }
}
