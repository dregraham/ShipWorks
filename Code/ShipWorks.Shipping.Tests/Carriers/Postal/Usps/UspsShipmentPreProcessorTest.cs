using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Usps
{
    public class UspsShipmentPreProcessorTest
    {
        private readonly AutoMock mock;
        private readonly UspsShipmentPreProcessor testObject;
        private readonly Mock<IDateTimeProvider> dateTimeProvider;
        private readonly Mock<IUspsFirstClassInternationalShipmentValidator> internationalValidator;
        private readonly Mock<IDefaultShipmentPreProcessor> defaultPreProcessor;

        public UspsShipmentPreProcessorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            dateTimeProvider = mock.Mock<IDateTimeProvider>();
            dateTimeProvider.SetupGet(d => d.Now).Returns(new DateTime(2018, 2, 1));
            internationalValidator = mock.Mock<IUspsFirstClassInternationalShipmentValidator>();
            defaultPreProcessor = mock.Mock<IDefaultShipmentPreProcessor>();
            
            testObject = mock.Create<UspsShipmentPreProcessor>();
        }
        
        [Fact]
        public void Run_DeligatesToFirstClassInternationalShipmentValidator_WhenDateIsGreatherThanStartDate()
        {
            ShipmentEntity shipment = new ShipmentEntity();

            testObject.Run(shipment, null, null);
            internationalValidator.Verify(i => i.ValidateShipment(shipment));
        }

        [Fact]
        public void Run_DoesNotDeligatesToFirstClassInternationalShipmentValidator_WhenDateIsNotGreatherThanStartDate()
        {
            ShipmentEntity shipment = new ShipmentEntity();

            dateTimeProvider.SetupGet(d => d.Now).Returns(new DateTime(2017, 2, 1));

            testObject.Run(shipment, null, null);
            internationalValidator.Verify(i => i.ValidateShipment(It.IsAny<ShipmentEntity>()), Times.Never);
        }

        [Fact]
        public void Run_DeligatesToDefaultShipmentPreProcessor()
        {
            ShipmentEntity shipment = new ShipmentEntity();

            testObject.Run(shipment, null, null);
            defaultPreProcessor.Verify(v => v.Run(shipment, null, null));
        }
    }
}
