using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Moq;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Policies;
using Xunit;

namespace ShipWorks.Tests.Shipping.Policies
{
    [Collection("ShippingPolicy")]
    public class ShippingPoliciesTest
    {
        private ShippingPolicies policies;
        private Mock<IShippingPolicy> bestRateApplicablePolicy1;
        private Mock<IShippingPolicy> bestRateApplicablePolicy2;
        private Mock<IShippingPolicy> bestRateNonApplicablePolicy;
        private Mock<IShippingPolicy> uspsApplicablePolicy;
        private Mock<IShippingPolicy> uspsNonApplicablePolicy;
        private MockRepository mockRepository;
        private Mock<IShippingPolicyFactory> policyFactoryMock;

        private const string bestRateFeatureXml = @"
<DummyRoot>
    <Feature>
        <Type>Foo</Type>
        <Config>1</Config>
    </Feature>
    <Feature>
        <Type>Bar</Type>
        <Config>True</Config>
    </Feature>
    <Feature>
        <Type>Bar</Type>
        <Config>False</Config>
    </Feature>
    <Feature>
        <Type>Foo</Type>
        <Config>6</Config>
    </Feature>
</DummyRoot>
";

        private const string uspsFeatureXml = @"
<DummyRoot>
    <Feature>
        <Type>Bar</Type>
        <Config>1</Config>
    </Feature>
    <Feature>
        <Type>Baz</Type>
        <Config>1</Config>
    </Feature>
</DummyRoot>
";

        private const string uspsFeature2Xml = @"
<DummyRoot>
    <Feature>
        <Type>Bar</Type>
        <Config>2</Config>
    </Feature>
    <Feature>
        <Type>Baz</Type>
        <Config>3</Config>
    </Feature>
</DummyRoot>
";

        private readonly Dictionary<ShipmentTypeCode, IEnumerable<XElement>> features = new Dictionary<ShipmentTypeCode, IEnumerable<XElement>>
            {
                {ShipmentTypeCode.BestRate, LoadElements(bestRateFeatureXml)},
                {ShipmentTypeCode.Usps, LoadElements(uspsFeatureXml)}
            };

        public ShippingPoliciesTest()
        {
            mockRepository = new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };
            policyFactoryMock = mockRepository.Create<IShippingPolicyFactory>();

            bestRateApplicablePolicy1 = CreatePolicyMock(mockRepository, true);
            bestRateApplicablePolicy2 = CreatePolicyMock(mockRepository, true);
            bestRateNonApplicablePolicy = CreatePolicyMock(mockRepository, false);
            uspsApplicablePolicy = CreatePolicyMock(mockRepository, true);
            uspsNonApplicablePolicy = CreatePolicyMock(mockRepository, false);

            // Make sure we're starting with a fresh cache each time
            ShippingPolicies.ClearCache();

            policies = new ShippingPolicies(new List<KeyValuePair<ShipmentTypeCode, IShippingPolicy>>()
            {
                CreatePolicyAssociation(ShipmentTypeCode.BestRate, bestRateApplicablePolicy1),
                CreatePolicyAssociation(ShipmentTypeCode.BestRate, bestRateApplicablePolicy1),
                CreatePolicyAssociation(ShipmentTypeCode.Usps, uspsNonApplicablePolicy),
                CreatePolicyAssociation(ShipmentTypeCode.Usps, uspsApplicablePolicy),
                CreatePolicyAssociation(ShipmentTypeCode.BestRate, bestRateNonApplicablePolicy),
                CreatePolicyAssociation(ShipmentTypeCode.BestRate, bestRateApplicablePolicy2)
            });
        }

        [Fact]
        public void Apply_DelegatesToIsApplicable()
        {
            object testObject = new object();
            policies.Apply(ShipmentTypeCode.BestRate, testObject);

            bestRateApplicablePolicy1.Verify(x => x.IsApplicable(testObject));
            bestRateApplicablePolicy2.Verify(x => x.IsApplicable(testObject));
            bestRateNonApplicablePolicy.Verify(x => x.IsApplicable(testObject));
        }

        [Fact]
        public void Apply_CallsApplyOnPolicies_WhenShippingTypesMatchAndIsApplicable()
        {
            object testObject = new object();
            policies.Apply(ShipmentTypeCode.BestRate, testObject);

            bestRateApplicablePolicy1.Verify(x => x.Apply(testObject));
            bestRateApplicablePolicy2.Verify(x => x.Apply(testObject));
        }

        [Fact]
        public void Apply_DoesNotCallApplyOnPolicies_WhenShippingTypesAreNotApplicable()
        {
            object testObject = new object();
            policies.Apply(ShipmentTypeCode.BestRate, testObject);

            bestRateNonApplicablePolicy.Verify(x => x.Apply(It.IsAny<object>()), Times.Never);
            uspsApplicablePolicy.Verify(x => x.Apply(It.IsAny<object>()), Times.Never);
            uspsNonApplicablePolicy.Verify(x => x.Apply(It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public void Apply_DoesNotCrash_WhenCalledWithShipmentTypeThatHasNoPolicies()
        {
            policies.Apply(ShipmentTypeCode.None, new object());
        }

        [Fact]
        public void Apply_WithNoPolicies_DoesNotThrow()
        {
            policies = new ShippingPolicies(new List<KeyValuePair<ShipmentTypeCode, IShippingPolicy>>());
            policies.Apply(ShipmentTypeCode.BestRate, new object());
        }

        [Fact]
        public void Constructor_DoesNotThrow_WhenParameterIsEmpty()
        {
            policies = new ShippingPolicies(null);
            policies.Apply(ShipmentTypeCode.BestRate, new object());
        }

        [Fact]
        public void Load_UpdatesCurrent()
        {
            policies = ShippingPolicies.Current;
            ShippingPolicies.Load(0, new List<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>>());
            Assert.NotSame(policies, ShippingPolicies.Current);
        }

        [Fact]
        public void Load_DelegatesToFactory_ToCreateShippingPolicies()
        {
            ShippingPolicies.Load(0, features, policyFactoryMock.Object);

            policyFactoryMock.Verify(x => x.Create(It.IsAny<ShipmentTypeCode>(), "Foo"), Times.Once);
            policyFactoryMock.Verify(x => x.Create(It.IsAny<ShipmentTypeCode>(), "Bar"), Times.Exactly(2));
            policyFactoryMock.Verify(x => x.Create(It.IsAny<ShipmentTypeCode>(), "Baz"), Times.Once);
        }

        [Fact]
        public void Load_DelegatesConfigurationToPolicy()
        {
            List<Mock<IShippingPolicy>> fooPolicies = CreateAndRegisterFactoryMocks("Foo", 1);
            List<Mock<IShippingPolicy>> barPolicies = CreateAndRegisterFactoryMocks("Bar", 2);
            List<Mock<IShippingPolicy>> bazPolicies = CreateAndRegisterFactoryMocks("Baz", 1);

            ShippingPolicies.Load(0, features, policyFactoryMock.Object);

            fooPolicies[0].Verify(x => x.Configure("1"));
            fooPolicies[0].Verify(x => x.Configure("6"));
            barPolicies[0].Verify(x => x.Configure("True"));
            barPolicies[0].Verify(x => x.Configure("False"));
            barPolicies[1].Verify(x => x.Configure("1"));
            bazPolicies[0].Verify(x => x.Configure("1"));
        }

        [Fact]
        public void Load_IncludePolicyOfExistingStore_WhenLoadingNewStore()
        {
            features.Remove(ShipmentTypeCode.BestRate);
            ShippingPolicies.Load(0, features, policyFactoryMock.Object);

            List<Mock<IShippingPolicy>> barPolicies = CreateAndRegisterFactoryMocks("Bar", 2);
            List<Mock<IShippingPolicy>> bazPolicies = CreateAndRegisterFactoryMocks("Baz", 2);

            var extraFeatures = new Dictionary<ShipmentTypeCode, IEnumerable<XElement>>
            {
                {ShipmentTypeCode.Usps, LoadElements(uspsFeature2Xml)}
            };

            ShippingPolicies.Load(2, extraFeatures, policyFactoryMock.Object);

            barPolicies[0].Verify(x => x.Configure("1"));
            barPolicies[1].Verify(x => x.Configure("2"));

            bazPolicies[0].Verify(x => x.Configure("1"));
            bazPolicies[1].Verify(x => x.Configure("3"));
        }

        [Fact]
        public void Load_ReplacePolicyOfExistingStore_WhenLoadingExistingStore()
        {
            features.Remove(ShipmentTypeCode.BestRate);
            ShippingPolicies.Load(0, features, policyFactoryMock.Object);

            List<Mock<IShippingPolicy>> barPolicies = CreateAndRegisterFactoryMocks("Bar", 1);
            List<Mock<IShippingPolicy>> bazPolicies = CreateAndRegisterFactoryMocks("Baz", 1);

            var extraFeatures = new Dictionary<ShipmentTypeCode, IEnumerable<XElement>>
            {
                {ShipmentTypeCode.Usps, LoadElements(uspsFeature2Xml)}
            };

            ShippingPolicies.Load(0, extraFeatures, policyFactoryMock.Object);

            barPolicies[0].Verify(x => x.Configure("2"));
            bazPolicies[0].Verify(x => x.Configure("3"));
        }

        [Fact]
        public void Unload_ExcludePolicyOfUnloadedStore()
        {
            // Load multiple stores
            features.Remove(ShipmentTypeCode.BestRate);
            ShippingPolicies.Load(0, features, policyFactoryMock.Object);

            var extraFeatures = new Dictionary<ShipmentTypeCode, IEnumerable<XElement>>
            {
                {ShipmentTypeCode.Usps, LoadElements(uspsFeature2Xml)}
            };
            ShippingPolicies.Load(2, extraFeatures, policyFactoryMock.Object);

            // Test unloading a store
            List<Mock<IShippingPolicy>> barPolicies = CreateAndRegisterFactoryMocks("Bar", 1);
            List<Mock<IShippingPolicy>> bazPolicies = CreateAndRegisterFactoryMocks("Baz", 1);

            ShippingPolicies.Unload(0, policyFactoryMock.Object);

            barPolicies[0].Verify(x => x.Configure("2"));
            bazPolicies[0].Verify(x => x.Configure("3"));
        }

        [Fact]
        public void Unload_DoesNotUpdateCurrentPolicies_WhenStoreDoesNotExist()
        {
            var currentPolicies = ShippingPolicies.Current;

            ShippingPolicies.Unload(1);

            Assert.Same(currentPolicies, ShippingPolicies.Current);
        }

        private List<Mock<IShippingPolicy>> CreateAndRegisterFactoryMocks(string key, int count)
        {
            List<Mock<IShippingPolicy>> policy = Enumerable.Range(0, count).Select(_ => mockRepository.Create<IShippingPolicy>()).ToList();
            policyFactoryMock.Setup(x => x.Create(It.IsAny<ShipmentTypeCode>(), key)).Returns(new Queue<IShippingPolicy>(policy.Select(x => x.Object)).Dequeue);

            return policy;
        }

        private static Mock<IShippingPolicy> CreatePolicyMock(MockRepository mockRepository, bool returnValue)
        {
            Mock<IShippingPolicy> policy = mockRepository.Create<IShippingPolicy>();
            policy.Setup(x => x.IsApplicable(It.IsAny<object>())).Returns(returnValue);
            return policy;
        }

        private static KeyValuePair<ShipmentTypeCode, IShippingPolicy> CreatePolicyAssociation(ShipmentTypeCode bestRate, IMock<IShippingPolicy> mock)
        {
            return new KeyValuePair<ShipmentTypeCode, IShippingPolicy>(bestRate, mock.Object);
        }

        private static IEnumerable<XElement> LoadElements(string xml)
        {
            XDocument document = XDocument.Parse(xml);
            return document.Descendants("Feature");
        }
    }
}
