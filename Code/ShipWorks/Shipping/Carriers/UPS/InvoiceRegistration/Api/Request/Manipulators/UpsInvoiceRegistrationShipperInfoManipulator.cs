using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;

namespace ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request.Manipulators
{
    /// <summary>
    /// Adds shipper info to the request
    /// </summary>
    public class UpsInvoiceRegistrationShipperInfoManipulator : ICarrierRequestManipulator
    {
        private UpsAccountEntity upsAccount;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsInvoiceRegistrationShipperInfoManipulator"/> class.
        /// </summary>
        /// <param name="upsAccount">The ups account.</param>
        public UpsInvoiceRegistrationShipperInfoManipulator(UpsAccountEntity upsAccount)
        {
            this.upsAccount = upsAccount;
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            RegisterRequest registerRequest = (RegisterRequest)request.NativeRequest;

            if (upsAccount.AccountNumber == string.Empty)
            {
                registerRequest.ShipperAccount = null;
                return;
            }

            if (registerRequest.ShipperAccount == null)
            {
                registerRequest.ShipperAccount = new ShipperAccountType();
            }

            registerRequest.ShipperAccount.AccountName = "Interapptive";
            registerRequest.ShipperAccount.AccountNumber = upsAccount.AccountNumber;
            registerRequest.ShipperAccount.PostalCode = upsAccount.PostalCode;
            registerRequest.ShipperAccount.CountryCode = upsAccount.CountryCode;
        }
    }
}
