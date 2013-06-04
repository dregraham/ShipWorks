using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;


namespace ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request.Manipulators
{
    public class UpsInvoiceRegistrationNewProfileCredentialsManipulator : ICarrierRequestManipulator
    {
        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            RegisterRequest registerRequest = (RegisterRequest)request.NativeRequest;

            string userId = Guid.NewGuid().ToString("N").Substring(0, 16);
            string password = Guid.NewGuid().ToString("N").Substring(0, 8);

            registerRequest.Username = userId;
            registerRequest.Password = password;
            registerRequest.SuggestUsernameIndicator = "N";

            // Notification code to be sent if the account is about to expire.  '00' None, '01' Email. Given
            // our users wouldn't know what it means, we set to 00
            registerRequest.NotificationCode = "00";
        }
    }
}
