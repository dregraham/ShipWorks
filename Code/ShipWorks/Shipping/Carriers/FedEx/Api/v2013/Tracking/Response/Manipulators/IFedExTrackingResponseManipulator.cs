﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Tracking.Response.Manipulators
{
    /// <summary>
    /// An interface for manipulating a FedExShipmentEntity based on the data in a 
    /// FedEx tracking response.
    /// </summary>
    public interface IFedExTrackingResponseManipulator
    {
        /// <summary>
        /// Manipulates a FedExShipmentEntity based on the data in the carrierResponse
        /// </summary>
        /// <param name="carrierResponse">The carrier response.</param>
        void Manipulate(ICarrierResponse carrierResponse);
    }
}
