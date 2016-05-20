using Autofac.Extras.Moq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using System;
using System.Collections.Generic;
using Xunit;

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
        public void Execute_DoNotDelegateToValidateAddress_NoPhysicalAddress()
        {
            var webClient = mock.Mock<IUspsWebClient>();
 
            var testObject = mock.Create<AssociateShipworksWithItselfRequest>();

            testObject.Execute();

            webClient.Verify(c => c.ValidateAddress(It.IsAny<PersonAdapter>()), Times.Never);
        }

        [Fact]
        public void Execute_ResponseTypeIsAddressValidationFailed_CannotValidateAddress()
        {
            var webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(c => c.ValidateAddress(It.IsAny<PersonAdapter>()))
                .Returns(new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = false
                });

            var testObject = mock.Create<AssociateShipworksWithItselfRequest>();

            testObject.PhysicalAddress = new PersonAdapter();

            var result = testObject.Execute();

            Assert.Equal(AssociateShipWorksWithItselfResponseType.AddressValidationFailed, result.ResponseType);
        }

        [Fact]
        public void Execute_MessageIsAddressValidationFailedDescription_WhenCannotValidateAddress_AndNullSuggestions()
        {
            var webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(c => c.ValidateAddress(It.IsAny<PersonAdapter>()))
                .Returns(new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = false
                });

            var testObject = mock.Create<AssociateShipworksWithItselfRequest>();

            testObject.PhysicalAddress = new PersonAdapter();

            var result = testObject.Execute();

            Assert.Equal(EnumHelper.GetDescription(AssociateShipWorksWithItselfResponseType.AddressValidationFailed), 
                result.Message);
        }

        [Fact]
        public void Execute_MessageIsAddressValidationFailedDescription_WhenCannotValidateAddress_AndNoSuggestions()
        {
            var webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(c => c.ValidateAddress(It.IsAny<PersonAdapter>()))
                .Returns(new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = false,
                    Candidates = new List<Address>()
                });

            var testObject = mock.Create<AssociateShipworksWithItselfRequest>();

            testObject.PhysicalAddress = new PersonAdapter();

            var result = testObject.Execute();

            Assert.Equal(EnumHelper.GetDescription(AssociateShipWorksWithItselfResponseType.AddressValidationFailed),
                result.Message);
        }

        [Fact]
        public void Execute_MessageContainsSuggestedAddress_WhenCannotValidateAddress_AndHasSuggestions()
        {
            string includedInSuggestedAddress = "123 Elm St.";

            var webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(c => c.ValidateAddress(It.IsAny<PersonAdapter>()))
                .Returns(new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = false,
                    Candidates = new List<Address>() {new Address() {Address1 = includedInSuggestedAddress}}
                });

            var testObject = mock.Create<AssociateShipworksWithItselfRequest>();

            testObject.PhysicalAddress = new PersonAdapter();

            var result = testObject.Execute();

            Assert.Contains(includedInSuggestedAddress, result.Message);
        }

        [Fact]
        public void Execute_MessageContainsThirdSuggestedAddress_WhenCannotValidateAddress_AndHasSuggestions()
        {
            string includedInSuggestedAddress = "123 Elm St.";

            var webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(c => c.ValidateAddress(It.IsAny<PersonAdapter>()))
                .Returns(new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = false,
                    Candidates =
                        new List<Address>
                        {
                            new Address(),
                            new Address(),
                            new Address {Address1 = includedInSuggestedAddress}
                        }
                });

            var testObject = mock.Create<AssociateShipworksWithItselfRequest>();

            testObject.PhysicalAddress = new PersonAdapter();

            var result = testObject.Execute();

            Assert.Contains(includedInSuggestedAddress, result.Message);
        }

        [Fact]
        public void Execute_MessageDoesNotContainForthSuggestedAddress_WhenCannotValidateAddress_AndHasSuggestions()
        {
            string includedInSuggestedAddress = "123 Elm St.";

            var webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(c => c.ValidateAddress(It.IsAny<PersonAdapter>()))
                .Returns(new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = false,
                    Candidates =
                        new List<Address>
                        {
                            new Address(),
                            new Address(),
                            new Address(),
                            new Address {Address1 = includedInSuggestedAddress}
                        }
                });

            var testObject = mock.Create<AssociateShipworksWithItselfRequest>();

            testObject.PhysicalAddress = new PersonAdapter();

            var result = testObject.Execute();

            Assert.DoesNotContain(includedInSuggestedAddress, result.Message);
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
        public void Execute_ResultIsPoBoxNotAllowed_WhenValidationSetsIsPoBoxToTrue()
        {
            var webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(c => c.ValidateAddress(It.IsAny<PersonAdapter>()))
                .Returns(new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = true,
                    IsPoBox = true
                });

            var testObject = mock.Create<AssociateShipworksWithItselfRequest>();

            testObject.PhysicalAddress = new PersonAdapter();
            
            var result = testObject.Execute();

            Assert.Equal(AssociateShipWorksWithItselfResponseType.POBoxNotAllowed, result.ResponseType);
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
