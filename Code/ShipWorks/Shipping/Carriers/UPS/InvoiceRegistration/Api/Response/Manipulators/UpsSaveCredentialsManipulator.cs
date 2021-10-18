using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Response.Manipulators
{
    /// <summary>
    /// Saves account username/password from the request.
    /// </summary>
    public class UpsSaveCredentialsManipulator : ICarrierResponseManipulator
    {
        /// <summary>
        /// Updates account username/password from the request.
        /// </summary>
        public void Manipulate(ICarrierResponse carrierResponse)
        {
            UpsInvoiceRegistrationResponse upsInvoiceRegistrationResponse = (UpsInvoiceRegistrationResponse) carrierResponse;
            RegisterRequest registerRequest = (RegisterRequest) upsInvoiceRegistrationResponse.Request.NativeRequest;

            UpsAccountEntity upsAccount = (UpsAccountEntity) upsInvoiceRegistrationResponse.Request.CarrierAccountEntity;

            upsAccount.UserID = registerRequest.Username;
            upsAccount.Password = registerRequest.Password;
            upsAccount.InvoiceAuth = true;
        }
    }
}
