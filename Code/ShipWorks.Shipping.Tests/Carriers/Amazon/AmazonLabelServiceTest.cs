using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonLabelServiceTest : IDisposable
    {
        readonly ShipmentEntity defaultShipment = new ShipmentEntity
        {
            Order = new AmazonOrderEntity(),
            Amazon = new AmazonShipmentEntity { ShippingServiceID = "something", CarrierName = "Foo" }
        };

        readonly AutoMock mock = null;
        readonly List<IAmazonLabelEnforcer> labelEnforcers;

        public AmazonLabelServiceTest()
        {
            mock = AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });
            labelEnforcers = new List<IAmazonLabelEnforcer>();

            mock.Provide<IEnumerable<IAmazonLabelEnforcer>>(labelEnforcers);
        }

        [Fact]
        public void Create_ThrowsArgumentNullException_WithNullShipment()
        {
            var testObject = mock.Create<AmazonLabelService>();

            Assert.Throws<ArgumentNullException>(() => testObject.Create(null));
        }

        [Fact]
        public void Create_CallsIsAllowed_OnEnforcers()
        {
            var enforcer1 = mock.MockRepository.Create<IAmazonLabelEnforcer>();
            enforcer1.Setup(x => x.CheckRestriction(It.IsAny<ShipmentEntity>()))
                .Returns(EnforcementResult.Success);

            var enforcer2 = mock.MockRepository.Create<IAmazonLabelEnforcer>();
            enforcer2.Setup(x => x.CheckRestriction(It.IsAny<ShipmentEntity>()))
                .Returns(EnforcementResult.Success);

            labelEnforcers.AddRange(new[] { enforcer1.Object, enforcer2.Object });

            var testObject = mock.Create<AmazonLabelService>();
            testObject.Create(defaultShipment);

            enforcer1.Verify(x => x.CheckRestriction(defaultShipment));
            enforcer2.Verify(x => x.CheckRestriction(defaultShipment));
        }

        [Fact]
        public void Create_ThrowsShippingException_WhenIsAllowedFails()
        {
            var enforcer = mock.Mock<IAmazonLabelEnforcer>();
            enforcer.Setup(x => x.CheckRestriction(It.IsAny<ShipmentEntity>()))
                .Returns(new EnforcementResult("Failed!"));

            labelEnforcers.Add(enforcer.Object);

            var testObject = mock.Create<AmazonLabelService>();

            Assert.Throws<AmazonShippingException>(() => testObject.Create(defaultShipment));
        }

        [Fact]
        public void Create_DoesNotCallIsAllowedOnSecondEnforcer_WhenFirstEnforcerIsNotAllowed()
        {
            var enforcer1 = mock.MockRepository.Create<IAmazonLabelEnforcer>();
            enforcer1.Setup(x => x.CheckRestriction(It.IsAny<ShipmentEntity>()))
                .Returns(new EnforcementResult("Foo!!!"));

            var enforcer2 = mock.MockRepository.Create<IAmazonLabelEnforcer>();

            labelEnforcers.AddRange(new[] { enforcer1.Object, enforcer2.Object });

            var testObject = mock.Create<AmazonLabelService>();
            Assert.Throws<AmazonShippingException>(() => testObject.Create(defaultShipment));

            enforcer2.Verify(x => x.CheckRestriction(It.IsAny<ShipmentEntity>()), Times.Never);
        }

        [Fact]
        public void Create_ReturnsLabelData_WithShipmentAndApiResults()
        {
            var labelData = new AmazonShipment();
            mock.Mock<IAmazonCreateShipmentRequest>().Setup(x => x.Submit(It.IsAny<ShipmentEntity>()))
                .Returns(labelData);

            var response = mock.Create<AmazonDownloadedLabelData>(TypedParameter.From(defaultShipment));
            mock.Provide<Func<ShipmentEntity, AmazonShipment, AmazonDownloadedLabelData>>(
                (s, a) => s == defaultShipment && a == labelData ? response : null);

            var testObject = mock.Create<AmazonLabelService>();
            var result = testObject.Create(defaultShipment);

            Assert.Equal(response, result);
        }

        [Fact]
        public void Void_Calls_IAmazonShipmentRequest()
        {
            AmazonLabelService testObject = mock.Create<AmazonLabelService>();

            ShipmentEntity shipment = new ShipmentEntity();

            testObject.Void(shipment);

            mock.Mock<IAmazonCancelShipmentRequest>().Verify(x => x.Submit(shipment));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
