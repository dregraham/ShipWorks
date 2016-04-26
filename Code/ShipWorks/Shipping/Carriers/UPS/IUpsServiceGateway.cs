using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// An interface that encapsulates the communication with UPS.
    /// </summary>
    public interface IUpsServiceGateway
    {
        /// <summary>
        /// Intended to interact with the UPS OpenAccount API for opening a new account with UPS.
        /// </summary>
        /// <param name="openAccountRequest">The open account request.</param>
        /// <returns>The OpenAccountResponse received from UPS.</returns>
        OpenAccountResponse OpenAccount(OpenAccountRequest openAccountRequest);

        /// <summary>
        /// Intended to interact with the UPS registration API when adding an account to ShipWorks.
        /// </summary>
        RegisterResponse RegisterAccount(RegisterRequest registerRequest);

        /// <summary>
        /// Associates account number with profile.
        /// </summary>
        ManageAccountResponse LinkNewAccount(ManageAccountRequest request);
    }
}
