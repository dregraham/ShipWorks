using Interapptive.Shared.Net;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Request.Manipulators
{
    /// <summary>
    /// Adds user information to Ups OpenAccount request.
    /// </summary>
    public class UpsOpenAccountAddEndUserInformation : ICarrierRequestManipulator
    {
        private readonly INetworkUtility networkUtility;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountAddEndUserInformation"/> class.
        /// </summary>
        /// <param name="networkUtility">The network utility.</param>
        public UpsOpenAccountAddEndUserInformation(INetworkUtility networkUtility) : base()
        {
            this.networkUtility = networkUtility;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountAddEndUserInformation"/> class.
        /// </summary>
        public UpsOpenAccountAddEndUserInformation() : base()
        {
            this.networkUtility = new NetworkUtility();
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        /// <exception cref="UpsOpenAccountException">Ups.com requires an IP address for registration, but ShipWorks could not obtain the IP address of this machine.</exception>
        public void Manipulate(CarrierRequest request)
        {
            OpenAccountRequest nativeAccountRequest = (OpenAccountRequest)request.NativeRequest;

            try
            {
                nativeAccountRequest.EndUserInformation = new EndUserInformationType()
                {
                    EndUserIPAddress = networkUtility.GetIPAddress()
                };
            }
            catch (NetworkException ex)
            {
                throw new UpsOpenAccountException("Ups.com requires an IP address for registration, but ShipWorks could not obtain the IP address of this machine.", ex);
            }
        }
    }
}