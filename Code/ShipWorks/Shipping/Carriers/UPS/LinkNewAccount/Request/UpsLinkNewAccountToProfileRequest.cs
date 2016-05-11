using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LinkNewAccount.Response;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.UPS.LinkNewAccount.Request
{
    /// <summary>
    /// Request to link new account to new profile.
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Carriers.Api.CarrierRequest" />
    public class UpsLinkNewAccountToProfileRequest : CarrierRequest
    {
        private readonly IUpsServiceGateway serviceGateway;
        private readonly UpsAccountEntity upsAccount;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLinkNewAccountToProfileRequest" /> class.
        /// </summary>
        public UpsLinkNewAccountToProfileRequest(
            IEnumerable<ICarrierRequestManipulator> manipulators,
            IUpsServiceGateway serviceGateway,
            UpsAccountEntity upsAccount)
            : base(manipulators, null)
        {
            this.serviceGateway = serviceGateway;
            this.upsAccount = upsAccount;

            NativeRequest = new ManageAccountRequest();
        }

        /// <summary>
        /// Gets the carrier account entity. This will always return null since there is not
        /// an account created yet. (Why else would you be using this request?)
        /// </summary>
        /// <value>The carrier account entity.</value>
        public override IEntity2 CarrierAccountEntity => upsAccount;

        /// <summary>
        /// Submits the request to the carrier API.
        /// </summary>
        public override ICarrierResponse Submit()
        {
            ApplyManipulators();

            try
            {
                ManageAccountResponse linkNewAccountResponse = serviceGateway.LinkNewAccount(NativeRequest as ManageAccountRequest);

                return new UpsLinkNewAccountToProfileResponse(linkNewAccountResponse, this);
            }
            catch (UpsWebServiceException ex)
            {
                throw new UpsOpenAccountException(ex.Message, UpsOpenAccountErrorCode.NotRegistered, ex);
            }
        }
    }
}
