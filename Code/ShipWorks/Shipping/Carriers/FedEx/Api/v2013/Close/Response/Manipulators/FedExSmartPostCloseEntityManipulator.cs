﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Close.Response.Manipulators
{
    /// <summary>
    /// An IFedExCloseManipulator implementation for saving the FedExEndOfDayCloseEntity for a SmartPost close response.
    /// </summary>
    public class FedExSmartPostCloseEntityManipulator : IFedExCloseResponseManipulator
    {
        private readonly IFedExEndOfDayCloseRepository closeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExGroundCloseReportManipulator" /> class using the
        /// FedExEndOfDayCloseRepository.
        /// </summary>
        public FedExSmartPostCloseEntityManipulator()
            : this(new FedExEndOfDayCloseRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExGroundCloseReportManipulator" /> class.
        /// </summary>
        /// <param name="closeRepository">The close repository.</param>
        public FedExSmartPostCloseEntityManipulator(IFedExEndOfDayCloseRepository closeRepository)
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
            closeEntity.IsSmartPost = true;

            // Save the end of day close data to the data source
            closeRepository.Save(closeEntity);
        }
    }
}
