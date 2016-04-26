using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;

namespace ShipWorks.Shipping.Carriers.UPS.LinkNewAccount.Request.Manipulators
{
    /// <summary>
    /// Add account information to Request
    /// </summary>
    public class UpsLinkNewAccountInfoManipulator : ICarrierRequestManipulator
    {
        private readonly UpsAccountEntity upsAccount;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLinkNewAccountInfoManipulator"/> class.
        /// </summary>
        public UpsLinkNewAccountInfoManipulator(UpsAccountEntity upsAccount)
        {
            this.upsAccount = upsAccount;
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public void Manipulate(CarrierRequest request)
        {
            ManageAccountRequest manageAccountRequest = (ManageAccountRequest) request.NativeRequest;

            manageAccountRequest.Username = upsAccount.UserID;
            manageAccountRequest.Password = upsAccount.Password;

            manageAccountRequest.ShipperAccount = new ShipperAccountType()
            {
                AccountName = upsAccount.AccountNumber,
                AccountNumber = upsAccount.AccountNumber,
                CountryCode = upsAccount.CountryCode,
                PostalCode = upsAccount.PostalCode
            };
        }
    }
}
