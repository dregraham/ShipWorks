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
            defaultSectionLayouts.Add(new SectionLayout() { Id = "asdf", Column = 9, Name = "testing", Row = 3, Selected = false, SectionFields = Enumerable.Empty<SectionFieldLayout>()});

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
            defaultsWithNewSectionLayout.Add(new SectionLayout() { Id = "asdf", Column = 9, Name = "testing", Row = 3, Selected = false, SectionFields = Enumerable.Empty<SectionFieldLayout>() });

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
                Id = "ToAddress",
                Row = 0,
                Column = 0,
                Selected = true,
                SectionFields = new List<SectionFieldLayout>()
                    {
                        new SectionFieldLayout() { Id = "FullName", Name = "Full Name", Row = 0 },
                        new SectionFieldLayout() { Id = "Street", Name = "Street", Row = 1, Selected = false},
                        new SectionFieldLayout() { Id = "StateProvince", Name = "State Province", Row = 2 }
                    }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "From Address",
                Id = "FromAddress",
                Selected = false,
                Row = 0,
                Column = 1,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = "FullName", Name = "Full Name", Row = 0 },
                    new SectionFieldLayout() { Id = "Street", Name = "Street", Row = 1 },
                    new SectionFieldLayout() { Id = "City", Name = "City", Row = 2, Selected = false },
                    new SectionFieldLayout() { Id = "StateProvince", Name = "State Province", Row = 3, Selected = false }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "Label Options",
                Id = "LabelOptions",
                Selected = false,
                Row = 1,
                Column = 0,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = "ShipDate", Name = "Ship Date", Row = 0 },
                    new SectionFieldLayout() { Id = "USPSStealthPostage", Name = "USPS - Stealth Postage", Row = 1, Selected = false  },
                    new SectionFieldLayout() { Id = "RequestedLabelFormat", Name = "Requested Label Format", Row = 2 }
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
                Id = "ToAddress",
                Row = 10,
                Column = 10,
                Selected = false,
                SectionFields = new List<SectionFieldLayout>()
                    {
                        new SectionFieldLayout() { Id = "FullName", Name = "Full Name New", Row = 2, Selected = false },
                        new SectionFieldLayout() { Id = "Street", Name = "Street New", Row = 2, Selected = true},
                        new SectionFieldLayout() { Id = "StateProvince", Name = "State Province New", Row = 22, Selected = false }
                    }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "From Address New",
                Id = "FromAddress",
                Selected = false,
                Row = 30,
                Column = 31,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = "FullName", Name = "Full Name New", Row = 30, Selected = false },
                    new SectionFieldLayout() { Id = "Street", Name = "Street New", Row = 31, Selected = false},
                    new SectionFieldLayout() { Id = "City", Name = "City New", Row = 32, Selected = true },
                    new SectionFieldLayout() { Id = "StateProvince", Name = "State Province New", Row = 33, Selected = true }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "Label Options New",
                Id = "LabelOptions",
                Selected = false,
                Row = 41,
                Column = 40,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = "ShipDate", Name = "Ship Date New", Row = 40, Selected = false },
                    new SectionFieldLayout() { Id = "USPSStealthPostage", Name = "USPS - Stealth Postage New", Row = 41, Selected = true  },
                    new SectionFieldLayout() { Id = "RequestedLabelFormat", Name = "Requested Label Format New", Row = 42, Selected = false }
                }
            });

            return sectionLayouts;
        }
    }
}
