using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping.Settings;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests.FieldManager
{
    public class OrderLookupFieldLayoutRepositoryTest
    {
        private readonly AutoMock mock;
        private OrderLookupFieldLayoutRepository testObject;
        private readonly Mock<IShippingSettings> shippingSettings;
        private readonly Mock<IOrderLookupFieldLayoutDefaults> defaultsProvider;

        public OrderLookupFieldLayoutRepositoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shippingSettings = mock.Mock<IShippingSettings>();
            defaultsProvider = mock.Mock<IOrderLookupFieldLayoutDefaults>();

            defaultsProvider.Setup(dp => dp.GetDefaults()).Returns(Defaults());
        }

        [Fact]
        public void Fetch_ReturnsCorrectOrderLookupFieldLayoutValue()
        {
            string json = JsonConvert.SerializeObject(Defaults());
            shippingSettings.Setup(ss => ss.FetchReadOnly()).Returns(new ShippingSettingsEntity() { OrderLookupFieldLayout = json });

            testObject = mock.Create<OrderLookupFieldLayoutRepository>();

            var results = testObject.Fetch();
            string resultsJson = JsonConvert.SerializeObject(results);

            Assert.Equal(json, resultsJson);
        }

        [Fact]
        public void Fetch_ReturnsCorrectOrderLookupFieldLayoutValue_WhenDbValueHasMissingSectionLayout()
        {
            string shippingSettingsJson = JsonConvert.SerializeObject(Defaults().Take(1));
            shippingSettings.Setup(ss => ss.FetchReadOnly()).Returns(new ShippingSettingsEntity() { OrderLookupFieldLayout = shippingSettingsJson });

            testObject = mock.Create<OrderLookupFieldLayoutRepository>();

            var results = testObject.Fetch();
            string resultsJson = JsonConvert.SerializeObject(results);

            Assert.Equal(DefaultsJson(), resultsJson);
        }

        [Fact]
        public void Fetch_ReturnsCorrectOrderLookupFieldLayoutValue_WhenDbValueHasExtraSectionLayout()
        {
            List<SectionLayout> defaultSectionLayouts = Defaults().ToList();
            defaultSectionLayouts.Add(new SectionLayout() { Id = SectionLayoutIDs.FedExSignatureAndReference, Name = "testing", Selected = false, SectionFields = Enumerable.Empty<SectionFieldLayout>().ToList()});

            string shippingSettingsJson = JsonConvert.SerializeObject(defaultSectionLayouts);
            shippingSettings.Setup(ss => ss.FetchReadOnly()).Returns(new ShippingSettingsEntity() { OrderLookupFieldLayout = shippingSettingsJson });

            testObject = mock.Create<OrderLookupFieldLayoutRepository>();

            var results = testObject.Fetch();
            string resultsJson = JsonConvert.SerializeObject(results);

            Assert.Equal(DefaultsJson(), resultsJson);
        }

        [Fact]
        public void Fetch_ReturnsCorrectOrderLookupFieldLayoutValue_WhenDbValuesHaveChanged()
        {
            string shippingSettingsJson = DefaultsWithDifferentValuesJson();
            shippingSettings.Setup(ss => ss.FetchReadOnly()).Returns(new ShippingSettingsEntity() { OrderLookupFieldLayout = shippingSettingsJson });

            testObject = mock.Create<OrderLookupFieldLayoutRepository>();

            var results = testObject.Fetch();
            string resultsJson = JsonConvert.SerializeObject(results);

            Assert.Equal(DefaultsWithDifferentValuesJson(), resultsJson);
        }

        [Fact]
        public void Fetch_ReturnsCorrectOrderLookupFieldLayoutValue_WhenDbValuesHaveChangedAndDefaultsHasNewSectionLayout()
        {
            string shippingSettingsJson = DefaultsWithDifferentValuesJson();
            shippingSettings.Setup(ss => ss.FetchReadOnly()).Returns(new ShippingSettingsEntity() { OrderLookupFieldLayout = shippingSettingsJson });

            List<SectionLayout> defaultsWithNewSectionLayout = DefaultsWithDifferentValues().ToList();
            defaultsWithNewSectionLayout.Add(new SectionLayout() { Id = SectionLayoutIDs.FedExSignatureAndReference, Name = "testing", Selected = false, SectionFields = Enumerable.Empty<SectionFieldLayout>().ToList() });

            defaultsProvider.Setup(dp => dp.GetDefaults()).Returns(defaultsWithNewSectionLayout);

            testObject = mock.Create<OrderLookupFieldLayoutRepository>();

            var results = testObject.Fetch();
            string resultsJson = JsonConvert.SerializeObject(results);

            Assert.Equal(JsonConvert.SerializeObject(defaultsWithNewSectionLayout), resultsJson);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("asdf")]
        [InlineData("[xcv]")]
        [InlineData("{\"test\": \"1\"}")]
        public void Fetch_ReturnsDefaults_WhenJsonIsInvalid(string testJson)
        {
            string expectedJson = JsonConvert.SerializeObject(Defaults());
            shippingSettings.Setup(ss => ss.FetchReadOnly()).Returns(new ShippingSettingsEntity() { OrderLookupFieldLayout = testJson });

            testObject = mock.Create<OrderLookupFieldLayoutRepository>();

            var results = testObject.Fetch();
            string resultsJson = JsonConvert.SerializeObject(results);

            Assert.Equal(expectedJson, resultsJson);
        }

        [Fact]
        public void Save_SavesOrderLookupFieldLayoutValue_ToShippingSettings()
        {
            string json = JsonConvert.SerializeObject(Defaults());
            ShippingSettingsEntity shippingSettingsEntity = new ShippingSettingsEntity() { OrderLookupFieldLayout = json };

            shippingSettings.Setup(ss => ss.FetchReadOnly()).Returns(shippingSettingsEntity);
            shippingSettings.Setup(ss => ss.Fetch()).Returns(shippingSettingsEntity);

            testObject = mock.Create<OrderLookupFieldLayoutRepository>();

            testObject.Save(Defaults());

            shippingSettings.Verify(ss => ss.Save(shippingSettingsEntity), Times.Exactly(1));
        }


        private string DefaultsJson()
        {
            return JsonConvert.SerializeObject(Defaults());
        }

        /// <summary>
        /// Load default layouts
        /// </summary>
        private IEnumerable<SectionLayout> Defaults()
        {
            List<SectionLayout> sectionLayouts = new List<SectionLayout>();

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "To Address",
                Id = SectionLayoutIDs.To,
                Selected = true,
                SectionFields = new List<SectionFieldLayout>()
                    {
                        new SectionFieldLayout() { Id = SectionLayoutFieldIDs.FullName, Name = "Full Name"},
                        new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Street, Name = "Street", Selected = false},
                        new SectionFieldLayout() { Id = SectionLayoutFieldIDs.StateProvince, Name = "State Province" }
                    }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "From Address",
                Id = SectionLayoutIDs.From,
                Selected = false,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.FullName, Name = "Full Name" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Street, Name = "Street"},
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.City, Name = "City", Selected = false },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.StateProvince, Name = "State Province", Selected = false }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "Label Options",
                Id = SectionLayoutIDs.LabelOptions,
                Selected = false,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.LabelOptionsShipDate, Name = "Ship Date"},
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.LabelOptionsUspsHideStealth, Name = "USPS - Stealth Postage", Selected = false },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.LabelOptionsRequestedLabelFormat, Name = "Requested Label Format" }
                }
            });

            return sectionLayouts;
        }


        private string DefaultsWithDifferentValuesJson()
        {
            return JsonConvert.SerializeObject(DefaultsWithDifferentValues());
        }

        /// <summary>
        /// Load default layouts
        /// </summary>
        private IEnumerable<SectionLayout> DefaultsWithDifferentValues()
        {
            List<SectionLayout> sectionLayouts = new List<SectionLayout>();

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "To Address New",
                Id = SectionLayoutIDs.To,
                Selected = false,
                SectionFields = new List<SectionFieldLayout>()
                    {
                        new SectionFieldLayout() { Id = SectionLayoutFieldIDs.FullName, Name = "Full Name New", Selected = false },
                        new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Street, Name = "Street New", Selected = true},
                        new SectionFieldLayout() { Id = SectionLayoutFieldIDs.StateProvince, Name = "State Province New", Selected = false }
                    }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "From Address New",
                Id = SectionLayoutIDs.From,
                Selected = false,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.FullName, Name = "Full Name New", Selected = false },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Street, Name = "Street New", Selected = false},
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.City, Name = "City New", Selected = true },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.StateProvince, Name = "State Province New", Selected = true }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "Label Options New",
                Id = SectionLayoutIDs.LabelOptions,
                Selected = false,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.LabelOptionsShipDate, Name = "Ship Date New", Selected = false },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.LabelOptionsUspsHideStealth, Name = "USPS - Stealth Postage New", Selected = true  },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.LabelOptionsRequestedLabelFormat, Name = "Requested Label Format New", Selected = false }
                }
            });

            return sectionLayouts;
        }
    }
}
