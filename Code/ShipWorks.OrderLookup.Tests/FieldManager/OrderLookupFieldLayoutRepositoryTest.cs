using System.Collections.Generic;
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

        public OrderLookupFieldLayoutRepositoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shippingSettings = mock.Mock<IShippingSettings>();
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
    }
}
