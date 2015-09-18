using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using Xunit;
using ShipWorks.Shipping;
using ShipWorks.Users.Security;
using ShipWorks.Core.Common.Threading;

namespace ShipWorks.Tests.Shipping
{
    public class ShipmentAddressValidatorTest
    {
        private ShipmentAddressValidator testObject;
        private OrderEntity orderEntity;
        private ShipmentEntity shipmentEntity;
        private AddressValidationWebClientValidateAddressResult avwcvar;
        private Mock<IValidatedAddressManager> validatedAddressManager;
        private Mock<IAddressValidationWebClient> addressValidationWebClient;
        private Mock<IFilterHelper> filterHelper;

        public ShipmentAddressValidatorTest()
        {
            validatedAddressManager = new Mock<IValidatedAddressManager>();
            addressValidationWebClient = new Mock<IAddressValidationWebClient>();

            orderEntity = new OrderEntity(1006);
            shipmentEntity = new ShipmentEntity(1031);
            shipmentEntity.Order = orderEntity;

            validatedAddressManager.Setup(s => s.ValidateShipmentAsync(It.IsAny<ShipmentEntity>(), It.IsAny<AddressValidator>()))
                .Returns(TaskUtility.CompletedTask)
                .Verifiable();

            avwcvar = new AddressValidationWebClientValidateAddressResult();
            avwcvar.AddressValidationResults.Add(new AddressValidationResult()
            {
                City = "Saint Louis",
                CountryCode = "US",
                IsValid = true,
                POBox = ValidationDetailStatusType.No,
                PostalCode = "63102",
                ResidentialStatus = ValidationDetailStatusType.Yes,
                StateProvCode = "MO",
                Street1 = "1 Memorial Drive",
                Street2 = "",
                Street3 = ""
            });

            addressValidationWebClient.Setup(s => s.ValidateAddressAsync(It.IsAny<AddressAdapter>())).ReturnsAsync(avwcvar);

            filterHelper = new Mock<IFilterHelper>();
            filterHelper.Setup(s => s.EnsureFiltersUpToDate(It.IsAny<TimeSpan>())).Returns(true);

            var tcs = new TaskCompletionSource<bool>();
            tcs.SetResult(true);

            testObject = new ShipmentAddressValidator(validatedAddressManager.Object, addressValidationWebClient.Object, filterHelper.Object);
        }

        [Fact]
        public async void ShipmentAndSuccess_WhenOrderHasOneShipment_ReturnsThatShipment_Test()
        {
            await testObject.ValidateAsync(shipmentEntity);

            validatedAddressManager.VerifyAll();
        }
        
    }
}
