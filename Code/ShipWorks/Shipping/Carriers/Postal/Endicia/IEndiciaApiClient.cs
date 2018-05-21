using System.Collections.Generic;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.LabelService;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Represents the Endicia Api Client
    /// </summary>
    [Service]
    public interface IEndiciaApiClient
    {
        /// <summary>
        /// Get postal rates for the given shipment for all possible mail classes and rates.
        /// </summary>
        List<RateResult> GetRates(ShipmentEntity shipment, EndiciaShipmentType endiciaShipmentType);

        /// <summary>
        /// Purchase postage for the given amount
        /// </summary>
        void BuyPostage(EndiciaAccountEntity account, decimal amount);

        /// <summary>
        /// Get the account status of the account, including the current postage balance.
        /// </summary>
        EndiciaAccountStatus GetAccountStatus(EndiciaAccountEntity account);

        /// <summary>
        /// Change the api passphrase for the given account.  Returns the encrypted updated password if successful
        /// </summary>
        string ChangeApiPassphrase(string accountNumber, EndiciaReseller reseller, string oldPassword, string newPassword);
        
        /// <summary>
        /// request a refund for the given shipment
        /// </summary>
        void RequestRefund(ShipmentEntity shipment);

        /// <summary>
        /// Process the given shipment
        /// </summary>
        LabelRequestResponse ProcessShipment(ShipmentEntity shipment, EndiciaShipmentType endiciaShipmentType);

        /// <summary>
        /// Track the given shipment
        /// </summary>
        Tracking.TrackingResult TrackShipment(ShipmentEntity shipment);

        /// <summary>
        /// Generate a scan form for the given shipments
        /// </summary>
        SCANResponse GetScanForm(IEndiciaAccountEntity account, IEnumerable<IShipmentEntity> shipments);

        /// <summary>
        /// Signup for a new Endicia account
        /// </summary>
        EndiciaAccountEntity Signup(EndiciaAccountEntity account,
            EndiciaAccountType endiciaAccountType,
            PersonAdapter accountAddress,
            EndiciaNewAccountCredentials accountCredentials,
            EndiciaPaymentInfo paymentInfo);
    }
}
