using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Close;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response
{
    /// <summary>
    /// An implementation of the IFedExEndOfDayCloseRepository interface that persists close data
    /// to the ShipWorks database.
    /// </summary>
    public class FedExEndOfDayCloseRepository : IFedExEndOfDayCloseRepository
    {
        /// <summary>
        /// Saves the specified close entity to the data source along with any documents/reports
        /// contained in the ground close response.
        /// </summary>
        /// <param name="closeEntity">The close entity.</param>
        /// <param name="closeResponse">The close response from FedEx.</param>
        public void Save(FedExEndOfDayCloseEntity closeEntity, GroundCloseReply closeResponse)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                adapter.SaveAndRefetch(closeEntity);

                if (closeResponse.CodReport != null)
                {
                    DataResourceManager.CreateFromBytes(closeResponse.CodReport, closeEntity.FedExEndOfDayCloseID, "COD Report");
                }

                if (closeResponse.HazMatCertificate != null)
                {
                    DataResourceManager.CreateFromBytes(closeResponse.HazMatCertificate, closeEntity.FedExEndOfDayCloseID, "HazMat Report");
                }

                if (closeResponse.MultiweightReport != null)
                {
                    DataResourceManager.CreateFromBytes(closeResponse.MultiweightReport, closeEntity.FedExEndOfDayCloseID, "Multiweight Report");
                }

                if (closeResponse.Manifest != null)
                {
                    DataResourceManager.CreateFromBytes(closeResponse.Manifest.File, closeEntity.FedExEndOfDayCloseID, closeResponse.Manifest.FileName);
                }

                adapter.Commit();
            }
        }

        /// <summary>
        /// Saves the specified close entity to the data source.
        /// </summary>
        /// <param name="closeEntity">The close entity.</param>
        public void Save(FedExEndOfDayCloseEntity closeEntity)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                adapter.SaveAndRefetch(closeEntity);
                adapter.Commit();
            }
        }
    }
}
