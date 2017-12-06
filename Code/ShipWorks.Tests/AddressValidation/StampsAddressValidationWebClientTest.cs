using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Business;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.AddressValidation
{
    public class StampsAddressValidationWebClientTest : IDisposable
    {
        private readonly AutoMock mock;

        public StampsAddressValidationWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public async Task ValidateAddressAsync_ReturnsMatchesAddressAsValid()
        {
            mock.Mock<IUspsWebClient>()
                .Setup(c => c.ValidateAddressAsync(It.IsAny<PersonAdapter>(), It.IsAny<UspsAccountEntity>()))
                .ReturnsAsync(new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = true,
                    IsCityStateZipOk = true,
                    ResidentialIndicator = ResidentialDeliveryIndicatorType.Yes,
                    MatchedAddress = new Address() { Address1 = "Add1" },
                    Candidates = new[] { new Address { Address1 = "Canidate1" } }
                });

            var testObject = mock.Create<StampsAddressValidationWebClient>();
            var result = await testObject.ValidateAddressAsync(new AddressAdapter());
            var matchedAddress = result.AddressValidationResults.Single(a => a.Street1 == "Add1");
            Assert.True(matchedAddress.IsValid);
        }

        [Fact]
        public async Task ValidateAddressAsync_ReturnsCandidatesAsInvalid()
        {
            mock.Mock<IUspsWebClient>()
                .Setup(c => c.ValidateAddressAsync(It.IsAny<PersonAdapter>(), It.IsAny<UspsAccountEntity>()))
                .ReturnsAsync(new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = true,
                    IsCityStateZipOk = true,
                    ResidentialIndicator = ResidentialDeliveryIndicatorType.Yes,
                    MatchedAddress = new Address() { Address1 = "Add1" },
                    Candidates = new[] { new Address { Address1 = "Canidate1" } }
                });

            var testObject = mock.Create<StampsAddressValidationWebClient>();
            var result = await testObject.ValidateAddressAsync(new AddressAdapter());
            var matchedAddress = result.AddressValidationResults.Single(a => a.Street1 == "Canidate1");
            Assert.False(matchedAddress.IsValid);
        }

        [Fact]
        public async Task ValidateAddressAsync_ReturnsValidationError_WhenNotIsSuccessfulMatch()
        {
            mock.Mock<IUspsWebClient>()
                .Setup(c => c.ValidateAddressAsync(It.IsAny<PersonAdapter>(), It.IsAny<UspsAccountEntity>()))
                .ReturnsAsync(new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = false,
                    IsCityStateZipOk = true,
                    ResidentialIndicator = ResidentialDeliveryIndicatorType.Yes,
                    MatchedAddress = new Address() { Address1 = "Add1" },
                    Candidates = new[] { new Address { Address1 = "Canidate1" } },
                    BadAddressMessage = "bad address"
                });

            var testObject = mock.Create<StampsAddressValidationWebClient>();
            var result = await testObject.ValidateAddressAsync(new AddressAdapter());
            
            Assert.Equal("bad address", result.AddressValidationError);
        }

        [Fact]
        public async Task ValidateAddressAsync_ProvinceIsReturnedWhenStateIsNull()
        {
            mock.Mock<IUspsWebClient>()
                .Setup(c => c.ValidateAddressAsync(It.IsAny<PersonAdapter>(), It.IsAny<UspsAccountEntity>()))
                .ReturnsAsync(new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = true,
                    IsCityStateZipOk = true,
                    ResidentialIndicator = ResidentialDeliveryIndicatorType.Yes,
                    MatchedAddress = new Address { Address1 = "Add1", State = null, Province = "prov"},
                    Candidates = new[] { new Address { Address1 = "Canidate1" } }
                });

            var testObject = mock.Create<StampsAddressValidationWebClient>();
            var result = await testObject.ValidateAddressAsync(new AddressAdapter());
            var matchedAddress = result.AddressValidationResults.Single(a => a.Street1 == "Add1");
            Assert.Equal("prov", matchedAddress.StateProvCode); 
        }

        [Fact]
        public async Task ValidateAddressAsync_StateIsReturnedWhenStateAndProvinceHaveValue()
        {
            mock.Mock<IUspsWebClient>()
                .Setup(c => c.ValidateAddressAsync(It.IsAny<PersonAdapter>(), It.IsAny<UspsAccountEntity>()))
                .ReturnsAsync(new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = true,
                    IsCityStateZipOk = true,
                    ResidentialIndicator = ResidentialDeliveryIndicatorType.Yes,
                    MatchedAddress = new Address { Address1 = "Add1", State = "state", Province = "prov" },
                    Candidates = new[] { new Address { Address1 = "Canidate1" } }
                });

            var testObject = mock.Create<StampsAddressValidationWebClient>();
            var result = await testObject.ValidateAddressAsync(new AddressAdapter());
            var matchedAddress = result.AddressValidationResults.Single(a => a.Street1 == "Add1");
            Assert.Equal("state", matchedAddress.StateProvCode);
        }

        [Theory]
        [InlineData(AddressType.Invalid, true, false, "Y", "MO", true, ResidentialDeliveryIndicatorType.No)]
        [InlineData(AddressType.SecondaryNotFound, true, true, "H", "MO", true, ResidentialDeliveryIndicatorType.No)]
        [InlineData(AddressType.SecondaryNotFound, true, true, "S", "MO", true, ResidentialDeliveryIndicatorType.No)]
        [InlineData(AddressType.PrimaryNotFound, false, true, "", "MO", true, ResidentialDeliveryIndicatorType.No)]
        [InlineData(AddressType.Military, true, true, "Y", "MO", false, ResidentialDeliveryIndicatorType.No)]
        [InlineData(AddressType.UsTerritory, true, true, "", "PR", true, ResidentialDeliveryIndicatorType.No)]
        [InlineData(AddressType.PoBox, true, true, "", "MO", true, ResidentialDeliveryIndicatorType.No)]
        [InlineData(AddressType.Residential, true, true, "", "MO", false, ResidentialDeliveryIndicatorType.Yes)]
        [InlineData(AddressType.Commercial, true, true, "", "MO", false, ResidentialDeliveryIndicatorType.No)]
        [InlineData(AddressType.Valid, true, true, "", "MO", false, ResidentialDeliveryIndicatorType.Unknown)]
        public async Task ValidateAddressAsync_AddressTypeIsCorrect(
            AddressType expectedAddressType,
            bool isSuccessfulMatch,
            bool isCityStateZipOk,
            string footNoteValue,
            string matchedAddressState,
            bool? isPoBox,
            ResidentialDeliveryIndicatorType residentialIndicator)
        {
            SetupUpspsAddressValidationResults(isSuccessfulMatch, isCityStateZipOk, footNoteValue, matchedAddressState, isPoBox,
                residentialIndicator);

            var testObject = mock.Create<StampsAddressValidationWebClient>();
            var result = await testObject.ValidateAddressAsync(new AddressAdapter());

            Assert.Equal(expectedAddressType, result.AddressType);
        }

        public void SetupUpspsAddressValidationResults(
            bool isSuccessfulMatch,
            bool isCityStateZipOk,
            string footNoteValue,
            string matchedAddressState,
            bool? isPoBox,
            ResidentialDeliveryIndicatorType residentialIndicator)
        {
            mock.Mock<IUspsWebClient>()
                .Setup(c => c.ValidateAddressAsync(It.IsAny<PersonAdapter>(), It.IsAny<UspsAccountEntity>()))
                .ReturnsAsync(new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = isSuccessfulMatch,
                    IsCityStateZipOk = isCityStateZipOk,
                    ResidentialIndicator = residentialIndicator,
                    MatchedAddress = new Address() { Address1 = "Add1", State = matchedAddressState },
                    Candidates = new[] { new Address { Address1 = "Canidate1" } },
                    BadAddressMessage = "bad address",
                    IsPoBox = isPoBox,
                    StatusCodes = new StatusCodes()
                    {
                        Footnotes = new[]
                        {
                            new Footnote()
                            {
                                Value = footNoteValue
                            }
                        }
                    }
                });
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
