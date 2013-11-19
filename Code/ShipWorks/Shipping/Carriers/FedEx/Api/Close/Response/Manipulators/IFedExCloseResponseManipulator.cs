using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response.Manipulators
{
    /// <summary>
    /// An interface for manipulating a FedExEndOfDayCloseEntity based on the data in a 
    /// FedEx close response.
    /// </summary>
    public interface IFedExCloseResponseManipulator
    {
        /// <summary>
        /// Manipulates a FedExEndOfDayCloseEntity based on the data in the carrierResponse
        /// </summary>
        /// <param name="carrierResponse">The carrier response.</param>
        /// <param name="closeEntity">The close entity.</param>
        void Manipulate(ICarrierResponse carrierResponse, FedExEndOfDayCloseEntity closeEntity);
    }
}
