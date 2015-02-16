using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Api;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Net
{
    /// <summary>
    /// Interacts with web services to buy postage and retrieve balance.
    /// </summary>
    public class UspsPostageWebClient : IPostageWebClient
    {
        private readonly UspsAccountEntity account;
        private decimal controlTotal;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsPostageWebClient" /> class.
        /// </summary>
        /// <param name="account">The account.</param>        
        public UspsPostageWebClient(UspsAccountEntity account)
        {
            this.account = account;
            controlTotal = decimal.MinValue;
        }

        /// <summary>
        /// Gets the shipment type code of the web client being used.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode
        {
            get
            {
                return PostalUtility.GetStampsShipmentTypeForStampsResellerType((StampsResellerType)account.UspsReseller).ShipmentTypeCode;
            }
        }

        /// <summary>
        /// Gets the value that will identify this account to the underlying provider (e.g. account number, username, etc.).
        /// </summary>
        public string AccountIdentifier
        {
            get { return account.Username; }
        }

        /// <summary>
        /// Gets the balance from the USPS postage provider.
        /// </summary>
        /// <returns>The available postage balance remaining.</returns>
        public decimal GetBalance()
        {
            decimal availablePostage = 0;

            IStampsWebClient client = CreateWebClient();
            if ((StampsResellerType) account.UspsReseller != StampsResellerType.Express1)
            {
                AccountInfo accountInfo = (AccountInfo)client.GetAccountInfo(account);

                // Make a note of the control total for purchasing purposes
                controlTotal = accountInfo.PostageBalance.ControlTotal;
                availablePostage = accountInfo.PostageBalance.AvailablePostage;
            }
            else
            {
                Stamps.WebServices.v29.AccountInfo accountInfo = (Stamps.WebServices.v29.AccountInfo)client.GetAccountInfo(account);

                // Make a note of the control total for purchasing purposes
                controlTotal = accountInfo.PostageBalance.ControlTotal;
                availablePostage = accountInfo.PostageBalance.AvailablePostage;
            }
            return availablePostage;
        }

        /// <summary>
        /// Purchases additional postage based on the amount specified.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public void Purchase(decimal amount)
        {
            if (controlTotal == decimal.MinValue)
            {
                // Don't yet have a valid value for the control total, so grab the balance
                // to populate this value
                GetBalance();
            }

            IStampsWebClient client = CreateWebClient();
            client.PurchasePostage(account, amount, controlTotal);
        }

        /// <summary>
        /// Creates the web client for communicating with the carrier API.
        /// </summary>
        /// <returns>An instance of an IStampsWebClient based on teh shipment type code.</returns>
        private IStampsWebClient CreateWebClient()
        {
            StampsShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode) as StampsShipmentType;
            return shipmentType.CreateWebClient();
        }
    }
}
