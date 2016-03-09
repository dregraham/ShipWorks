using Autofac.Extras.Moq;
using Interapptive.Shared.Business;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using System;
using Xunit;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class AssociateShipworksWithItselfRequestTest : IDisposable
    {
        AutoMock mock; 

        public AssociateShipworksWithItselfRequestTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void Execute_DelegateValidateAddress_HasPhysicalAddress()
        {
            var webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(c => c.ValidateAddress(It.IsAny<PersonAdapter>()))
                .Returns(new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = true
                });
            
            var testObject = mock.Create<AssociateShipworksWithItselfRequest>();
         
            testObject.PhysicalAddress = new PersonAdapter();

            testObject.Execute();

            webClient.Verify(c => c.ValidateAddress(testObject.PhysicalAddress), Times.Once);
        }

        [Fact]
        public void Execute_DontDelegateToValidateAddress_NoPhysicalAddress()
        {
            var webClient = mock.Mock<IUspsWebClient>();
 
            var testObject = mock.Create<AssociateShipworksWithItselfRequest>();

            testObject.Execute();

            webClient.Verify(c => c.ValidateAddress(It.IsAny<PersonAdapter>()), Times.Never);
        }

        [Fact]
        public void Execute_ThrowsAddressValidationException_CannotValidateAddress()
        {
            var webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(c => c.ValidateAddress(It.IsAny<PersonAdapter>()))
                .Returns(new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = false
                });

            var testObject = mock.Create<AssociateShipworksWithItselfRequest>();

            testObject.PhysicalAddress = new PersonAdapter();

            Assert.Throws<AddressValidationException>(()=> testObject.Execute());          
        }

        [Fact]
        public void Execute_MatchedAddressIsSet_WhenPhysicalAddressValidated()
        {
            var matchedAddress = new Address();

            var webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(c => c.ValidateAddress(It.IsAny<PersonAdapter>()))
                .Returns(new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = true,
                    MatchedAddress = matchedAddress
                });

            var testObject = mock.Create<AssociateShipworksWithItselfRequest>();

            testObject.PhysicalAddress = new PersonAdapter();

            testObject.Execute();

            Assert.Equal(matchedAddress, testObject.MatchedPhysicalAddress);
        }

        [Fact]
        public void Execute_DelegatesToTangoWebClient()
        {
            var webClient = mock.Mock<ITangoWebClient>();

            var testObject = mock.Create<AssociateShipworksWithItselfRequest>();

            testObject.Execute();

            webClient.Verify(c => c.AssociateShipworksWithItself(testObject), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
