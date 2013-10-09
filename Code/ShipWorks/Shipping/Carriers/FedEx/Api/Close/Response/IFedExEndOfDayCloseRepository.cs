using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Close;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response
{
    /// <summary>
    /// A repository interface for saving the results of an end of day close.
    /// </summary>
    public interface IFedExEndOfDayCloseRepository
    {
        /// <summary>
        /// Saves the specified close entity to the data source along with any documents/reports
        /// contained in the ground close response.
        /// </summary>
        /// <param name="closeEntity">The close entity.</param>
        /// <param name="closeResponse">The close response from FedEx.</param>
        void Save(FedExEndOfDayCloseEntity closeEntity, GroundCloseReply closeResponse);

        /// <summary>
        /// Saves the specified close entity to the data source.
        /// </summary>
        /// <param name="closeEntity">The close entity.</param>
        void Save(FedExEndOfDayCloseEntity closeEntity);
    }
}
