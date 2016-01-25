using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Tests.Shared;
using ShipWorks.UI.Controls.AddressControl;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel
{
    public class InsuranceViewModelTest
    {
        private readonly ShipmentEntity shipment = new ShipmentEntity();
        private Mock<ICarrierShipmentAdapter> shipmentAdapter;
        private Mock<IShipmentServicesBuilder> shipmentServicesBuilder;
        private Mock<IShipmentServicesBuilderFactory> shipmentServicesBuilderFactory;
        private Mock<IShipmentPackageTypesBuilder> shipmentPackageTypesBuilder;
        private Mock<IShipmentPackageTypesBuilderFactory> shipmentPackageTypesBuilderFactory;
        private readonly Dictionary<int, string> expectedServices = new Dictionary<int, string>();
        private readonly Dictionary<int, string> expectedPackageTypes = new Dictionary<int, string>();
        private readonly List<IPackageAdapter> packageAdapters = new List<IPackageAdapter>();

        public InsuranceViewModelTest()
        {
            
        }


        [Theory]
        [InlineData(ShipmentTypeCode.FedEx, 100.00, false, InsuranceProvider.Carrier, true, 2.75)]
        public void ValuesMatch_Test(ShipmentTypeCode shipmentTypeCode, decimal insuredValue, bool pennyOne, 
            InsuranceProvider insuranceProvider, bool insured, decimal expectedInsuranceCost)
        {

            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                IInsuranceUtility insuranceUtility = new InsuranceUtilityWrapper(mock.Create<IShippingSettings>());
                mock.Provide(insuranceUtility);

                CreateDefaultShipmentAdapter(mock, shipmentTypeCode, 1, pennyOne, insuranceProvider, insuredValue, insured);

                InsuranceViewModel testObject = mock.Create<InsuranceViewModel>();

                testObject.Load(packageAdapters, packageAdapters.First(), shipmentAdapter.Object);

                decimal cost = insuranceProvider == InsuranceProvider.Carrier
                    ? testObject.InsuranceCost.Carrier.Value
                    : testObject.InsuranceCost.ShipWorks.Value;

                Assert.Equal(expectedInsuranceCost, cost);

            }
        }




        private Dictionary<int, string> CreateDefaultShipmentAdapter(AutoMock mock, ShipmentTypeCode shipmentTypeCode, 
            int numberOfPackages, bool pennyOne, InsuranceProvider insuranceProvider, decimal insuredValue, bool insured)
        {
            shipmentServicesBuilder = mock.Mock<IShipmentServicesBuilder>();
            shipmentServicesBuilder.Setup(sb => sb.BuildServiceTypeDictionary(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(expectedServices);

            shipmentServicesBuilderFactory = mock.Mock<IShipmentServicesBuilderFactory>();
            shipmentServicesBuilderFactory.Setup(sbf => sbf.Get(It.IsAny<ShipmentTypeCode>())).Returns(shipmentServicesBuilder.Object);

            shipmentPackageTypesBuilder = mock.Mock<IShipmentPackageTypesBuilder>();
            shipmentPackageTypesBuilder.Setup(sb => sb.BuildPackageTypeDictionary(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(expectedPackageTypes);

            shipmentPackageTypesBuilderFactory = mock.Mock<IShipmentPackageTypesBuilderFactory>();
            shipmentPackageTypesBuilderFactory.Setup(sbf => sbf.Get(It.IsAny<ShipmentTypeCode>())).Returns(shipmentPackageTypesBuilder.Object);

            CreatePackageAdapters(numberOfPackages, pennyOne, insuranceProvider, insuredValue, insured);

            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "US";

            shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);
            shipmentAdapter.Setup(sa => sa.ShipmentTypeCode).Returns(shipmentTypeCode);
            shipmentAdapter.Setup(sa => sa.ServiceType).Returns(1);
            shipmentAdapter.Setup(sa => sa.ShipDate).Returns(new DateTime(2015, 1, 1, 1, 1, 1));
            shipmentAdapter.Setup(sa => sa.TotalWeight).Returns(0.5);
            shipmentAdapter.Setup(sa => sa.SupportsPackageTypes).Returns(true);
            shipmentAdapter.Setup(sa => sa.SupportsAccounts).Returns(true);
            shipmentAdapter.Setup(sa => sa.SupportsMultiplePackages).Returns(true);
            shipmentAdapter.Setup(sa => sa.GetPackageAdapters()).Returns(packageAdapters);
            shipmentAdapter.Setup(sa => sa.GetPackageAdapters(It.IsAny<int>())).Returns((int x) =>
            {
                CreatePackageAdapters(x, pennyOne, insuranceProvider, insuredValue, insured);
                return packageAdapters;
            });

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(false);
            shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(new EntityCollection<ShipmentCustomsItemEntity>());

            return expectedServices;
        }

        private void CreatePackageAdapters(int numberOfPackages, bool pennyOne, InsuranceProvider insuranceProvider, decimal insuredValue, bool insured)
        {
            packageAdapters.Clear();

            for (int i = 1; i <= numberOfPackages; i++)
            {
                TestPackageAdapter packageAdapter = new TestPackageAdapter();
                packageAdapter.PropertyChanged += OnPropertyChanged;
                packageAdapter.PackagingType = new PackageTypeBinding() { PackageTypeID = (int)UpsPackagingType.Custom, Name = "Your Pakaging" };
                packageAdapter.AdditionalWeight = 0.1 * i;
                packageAdapter.ApplyAdditionalWeight = false;
                packageAdapter.Index = 1 * i;
                packageAdapter.DimsHeight = 2 * i;
                packageAdapter.DimsLength = 2 * i;
                packageAdapter.DimsWidth = 1 * i;
                packageAdapter.Weight = 0.5 * i;

                Mock<IInsuranceChoice> insuranceChoice = new Mock<IInsuranceChoice>();
                insuranceChoice.Setup(ic => ic.Shipment).Returns(shipment);
                insuranceChoice.Setup(ic => ic.InsurancePennyOne).Returns(pennyOne);
                insuranceChoice.Setup(ic => ic.InsuranceProvider).Returns(insuranceProvider);
                insuranceChoice.Setup(ic => ic.InsuranceValue).Returns(insuredValue);
                insuranceChoice.Setup(ic => ic.Insured).Returns(insured);

                packageAdapter.InsuranceChoice = insuranceChoice.Object;

                packageAdapters.Add(packageAdapter);
            }
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Only used for testing.
        }

    }
}
