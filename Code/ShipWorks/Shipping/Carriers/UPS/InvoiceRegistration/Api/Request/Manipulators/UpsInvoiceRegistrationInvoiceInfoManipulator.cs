using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;


namespace ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request.Manipulators
{
    /// <summary>
    /// Adds Invoice information to request.
    /// </summary>
    public class UpsInvoiceRegistrationInvoiceInfoManipulator : ICarrierRequestManipulator
    {
        private UpsOltInvoiceAuthorizationData invoiceAuthorization;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsInvoiceRegistrationInvoiceInfoManipulator"/> class.
        /// </summary>
        /// <param name="invoiceAuthorization">The invoice authorization.</param>
        public UpsInvoiceRegistrationInvoiceInfoManipulator(UpsOltInvoiceAuthorizationData invoiceAuthorization)
        {
            this.invoiceAuthorization = invoiceAuthorization;
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            RegisterRequest registerRequest = (RegisterRequest) request.NativeRequest;

            if (registerRequest.ShipperAccount == null)
            {
                registerRequest.ShipperAccount= new ShipperAccountType();
            }

            // Ups allows for registration without the invoice info
            // if the account is new and has never been invoiced before
            if (invoiceAuthorization != null && !string.IsNullOrWhiteSpace(invoiceAuthorization.InvoiceNumber))
            {
                registerRequest.ShipperAccount.InvoiceInfo = new InvoiceInfoType
                {
                    CurrencyCode = UpsUtility.GetCurrency((UpsAccountEntity)request.CarrierAccountEntity),
                    InvoiceNumber = invoiceAuthorization.InvoiceNumber,
                    InvoiceDate = invoiceAuthorization.InvoiceDate.ToString("yyyMMdd"),
                    InvoiceAmount = invoiceAuthorization.InvoiceAmount.ToString("0.00"),
                    ControlID = invoiceAuthorization.ControlID
                };
            }
        }
    }
}
