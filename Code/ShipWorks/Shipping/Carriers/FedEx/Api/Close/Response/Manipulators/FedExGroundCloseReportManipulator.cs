using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response.Manipulators
{
    /// <summary>
    /// An IFedExCloseManipulator implementation for saving any end of day reports contained in the ground close response.
    /// </summary>
    public class FedExGroundCloseReportManipulator : IFedExCloseResponseManipulator
    {
        private readonly IFedExEndOfDayCloseRepository closeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExGroundCloseReportManipulator" /> class using the
        /// FedExEndOfDayCloseRepository.
        /// </summary>
        public FedExGroundCloseReportManipulator()
            : this(new FedExEndOfDayCloseRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExGroundCloseReportManipulator" /> class.
        /// </summary>
        /// <param name="closeRepository">The close repository.</param>
        public FedExGroundCloseReportManipulator(IFedExEndOfDayCloseRepository closeRepository)
        {
            this.closeRepository = closeRepository;
        }

        /// <summary>
        /// Manipulates a FedExEndOfDayCloseEntity contained in the carrierResponse
        /// </summary>
        /// <param name="carrierResponse">The carrier response.</param>
        /// <param name="closeEntity">The close entity.</param>
        public void Manipulate(ICarrierResponse carrierResponse, FedExEndOfDayCloseEntity closeEntity)
        {
            if (closeEntity == null)
            {
                throw new ArgumentNullException("closeEntity");
            }

            // We're going to need the FedEx accound ID and account number to populate the close entity, 
            // so pull this from the request that generated the response
            FedExAccountEntity account = carrierResponse.Request.CarrierAccountEntity as FedExAccountEntity;
            if (account == null)
            {
                throw new FedExException("An unexpected carrier account was provided.");
            }

            closeEntity.FedExAccountID = account.FedExAccountID;
            closeEntity.AccountNumber = account.AccountNumber;
            closeEntity.CloseDate = DateTime.UtcNow;
            closeEntity.IsSmartPost = false;

            // Save the end of day close data to the data source
            closeRepository.Save(closeEntity, carrierResponse.NativeResponse as GroundCloseReply);
        }
    }
}
