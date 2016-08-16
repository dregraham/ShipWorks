using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api
{
    /// <summary>
    /// An interface for creating requests that communicate with a shipping carrier's API.
    /// </summary>
    public interface IUpsOpenAccountRequestFactory
    {
        /// <summary>
        /// Creates a request for opening a new account on UPS.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// to open a new UPS account via the UpsOpenAccount API.</returns>
        CarrierRequest CreateOpenAccountRequest(OpenAccountRequest request);

        /// <summary>
        /// Creates the link new account request factory.
        /// </summary>
        CarrierRequest CreateLinkNewAccountRequestFactory();
    }
}
