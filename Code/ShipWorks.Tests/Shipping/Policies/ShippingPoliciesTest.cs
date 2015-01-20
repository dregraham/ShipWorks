using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Policies;

namespace ShipWorks.Tests.Shipping.Policies
{
    [TestClass]
    public class ShippingPoliciesTest
    {
        private ShippingPolicies policies;
        private Mock<IShippingPolicy> bestRateApplicablePolicy1;
        private Mock<IShippingPolicy> bestRateApplicablePolicy2;
        private Mock<IShippingPolicy> bestRateNonApplicablePolicy;
        private Mock<IShippingPolicy> stampsApplicablePolicy;
        private Mock<IShippingPolicy> stampsNonApplicablePolicy;
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

        private const string stampsFeatureXml = @"
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

        private readonly Dictionary<ShipmentTypeCode, IEnumerable<XElement>> features = new Dictionary<ShipmentTypeCode, IEnumerable<XElement>>
            {
                {ShipmentTypeCode.BestRate, LoadElements(bestRateFeatureXml)},
                {ShipmentTypeCode.Stamps, LoadElements(stampsFeatureXml)}
            };

        [TestInitialize]
        public void Setup()
        {
            mockRepository = new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };
            policyFactoryMock = mockRepository.Create<IShippingPolicyFactory>();

            bestRateApplicablePolicy1 = CreatePolicyMock(mockRepository, true);
            bestRateApplicablePolicy2 = CreatePolicyMock(mockRepository, true);
            bestRateNonApplicablePolicy = CreatePolicyMock(mockRepository, false);
            stampsApplicablePolicy = CreatePolicyMock(mockRepository, true);
            stampsNonApplicablePolicy = CreatePolicyMock(mockRepository, false);

            policies = new ShippingPolicies(new List<KeyValuePair<ShipmentTypeCode, IShippingPolicy>>()
            {
                CreatePolicyAssociation(ShipmentTypeCode.BestRate, bestRateApplicablePolicy1),
                CreatePolicyAssociation(ShipmentTypeCode.BestRate, bestRateApplicablePolicy1),
                CreatePolicyAssociation(ShipmentTypeCode.Stamps, stampsNonApplicablePolicy),
                CreatePolicyAssociation(ShipmentTypeCode.Stamps, stampsApplicablePolicy),
                CreatePolicyAssociation(ShipmentTypeCode.BestRate, bestRateNonApplicablePolicy),
                CreatePolicyAssociation(ShipmentTypeCode.BestRate, bestRateApplicablePolicy2)
            });
        }

        [TestMethod]
        public void Apply_DelegatesToIsApplicable()
        {
            object testObject = new object();
            policies.Apply(ShipmentTypeCode.BestRate, testObject);

            bestRateApplicablePolicy1.Verify(x => x.IsApplicable(testObject));
            bestRateApplicablePolicy2.Verify(x => x.IsApplicable(testObject));
            bestRateNonApplicablePolicy.Verify(x => x.IsApplicable(testObject));
        }

        [TestMethod]
        public void Apply_CallsApplyOnPolicies_WhenShippingTypesMatchAndIsApplicable()
        {
            object testObject = new object();
            policies.Apply(ShipmentTypeCode.BestRate, testObject);

            bestRateApplicablePolicy1.Verify(x => x.Apply(testObject));
            bestRateApplicablePolicy2.Verify(x => x.Apply(testObject));
        }

        [TestMethod]
        public void Apply_DoesNotCallApplyOnPolicies_WhenShippingTypesAreNotApplicable()
        {
            object testObject = new object();
            policies.Apply(ShipmentTypeCode.BestRate, testObject);

            bestRateNonApplicablePolicy.Verify(x => x.Apply(It.IsAny<object>()), Times.Never);
            stampsApplicablePolicy.Verify(x => x.Apply(It.IsAny<object>()), Times.Never);
            stampsNonApplicablePolicy.Verify(x => x.Apply(It.IsAny<object>()), Times.Never);
        }

        [TestMethod]
        public void Apply_DoesNotCrash_WhenCalledWithShipmentTypeThatHasNoPolicies()
        {
            policies.Apply(ShipmentTypeCode.None, new object());
        }

        [TestMethod]
        public void Apply_WithNoPolicies_DoesNotThrow()
        {
            policies = new ShippingPolicies(new List<KeyValuePair<ShipmentTypeCode, IShippingPolicy>>());
            policies.Apply(ShipmentTypeCode.BestRate, new object());
        }

        [TestMethod]
        public void Constructor_DoesNotThrow_WhenParameterIsEmpty()
        {
            policies = new ShippingPolicies(null);
            policies.Apply(ShipmentTypeCode.BestRate, new object());
        }

        [TestMethod]
        public void Load_UpdatesCurrent()
        {
            policies = ShippingPolicies.Current;
            ShippingPolicies.Load(new List<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>>());
            Assert.AreNotSame(policies, ShippingPolicies.Current);
        }

        [TestMethod]
        public void Load_DelegatesToFactory_ToCreateShippingPolicies()
        {
            ShippingPolicies.Load(features, policyFactoryMock.Object);

            policyFactoryMock.Verify(x => x.Create("Foo"), Times.Once);
            policyFactoryMock.Verify(x => x.Create("Bar"), Times.Exactly(2));
            policyFactoryMock.Verify(x => x.Create("Baz"), Times.Once);
        }

        [TestMethod]
        public void Load_DelegatesConfigurationToPolicy()
        {
            List<Mock<IShippingPolicy>> fooPolicies = CreateAndRegisterFactoryMocks("Foo", 1);
            List<Mock<IShippingPolicy>> barPolicies = CreateAndRegisterFactoryMocks("Bar", 2);
            List<Mock<IShippingPolicy>> bazPolicies = CreateAndRegisterFactoryMocks("Baz", 1);

            ShippingPolicies.Load(features, policyFactoryMock.Object);

            fooPolicies[0].Verify(x => x.Configure("1"));
            fooPolicies[0].Verify(x => x.Configure("6"));
            barPolicies[0].Verify(x => x.Configure("True"));
            barPolicies[0].Verify(x => x.Configure("False"));
            barPolicies[1].Verify(x => x.Configure("1"));
            bazPolicies[0].Verify(x => x.Configure("1"));
        }

        private List<Mock<IShippingPolicy>> CreateAndRegisterFactoryMocks(string key, int count)
        {
            List<Mock<IShippingPolicy>> policy = Enumerable.Range(0, count).Select(_ => mockRepository.Create<IShippingPolicy>()).ToList();
            policyFactoryMock.Setup(x => x.Create(key)).Returns(new Queue<IShippingPolicy>(policy.Select(x => x.Object)).Dequeue);
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
