using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;

namespace ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request.Manipulators
{
    /// <summary>
    /// Add address and other company information ot request
    /// </summary>
    public class UpsInvoiceRegistrationAddressManipulator : ICarrierRequestManipulator
    {
        private UpsAccountEntity upsAccount;
        private readonly INetworkUtility networkUtility;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsInvoiceRegistrationAddressManipulator"/> class.
        /// </summary>
        public UpsInvoiceRegistrationAddressManipulator(UpsAccountEntity upsAccount, INetworkUtility networkUtility)
        {
            this.upsAccount = upsAccount;
            this.networkUtility = networkUtility;
        }

        /// <summary>
        /// Manipulates the specified carrier request.
        /// </summary>
        /// <param name="request">The carrier request.</param>
        public void Manipulate(CarrierRequest request)
        {
            RegisterRequest registerRequest = (RegisterRequest) request.NativeRequest;

            PersonAdapter accountAddress = new PersonAdapter(upsAccount, "");

            registerRequest.CompanyName = upsAccount.Company;
            registerRequest.CustomerName = new PersonName(accountAddress).FullName;

            AddressType address = new AddressType();
            registerRequest.Address = address;
            address.AddressLine = accountAddress.StreetLines;
            address.City = accountAddress.City;
            address.StateProvinceCode = accountAddress.StateProvCode;
            address.PostalCode = accountAddress.PostalCode;
            address.CountryCode = accountAddress.CountryCode;

            registerRequest.PhoneNumber = accountAddress.Phone10Digits;
            registerRequest.EmailAddress = accountAddress.Email;

            registerRequest.EndUserIPAddress = networkUtility.GetIPAddress();
        }
    }
}
