using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Fims
{
    /// <summary>
    /// Responsible for saving retrieved FedEx FIMS Labels to Database
    /// </summary>
    public interface IFimsLabelRepository : ILabelRepository
    {
        /// <summary>
        /// Responsible for saving retrieved FedEx FIMS Labels to Database
        /// </summary>
        void SaveLabel(IFimsShipResponse fimsShipResponse, long ownerID);
    }
}
