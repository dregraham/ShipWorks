using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Creates an Asendia Manifest
    /// </summary>
    [Component]
    public class AsendiaManifestCreator : IAsendiaManifestCreator
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IShipEngineWebClient webClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public AsendiaManifestCreator(IDateTimeProvider dateTimeProvider, IShipEngineWebClient webClient)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.webClient = webClient;
        }

        /// <summary>
        /// Create an Asendia Manifest from today's shipments
        /// </summary>
        public async Task<Result> CreateManifest()
        {
            // Create the predicate for the query to determine which shipments are eligible
            RelationPredicateBucket bucket = new RelationPredicateBucket(
                ShipmentFields.Processed == true &
                ShipmentFields.ProcessedDate >= dateTimeProvider.GetUtcNow().Date &
                ShipmentFields.ReturnShipment == false &
                ShipmentFields.ShipmentType == (int) ShipmentTypeCode.Asendia);

            // We just need ShipEngine Label IDs
            ResultsetFields resultFields = new ResultsetFields(1);
            resultFields.DefineField(AsendiaShipmentFields.ShipEngineLabelID, 0, "ShipEngineLabelID", "");

            // Do the fetch
            using (IDataReader reader = SqlAdapter.Default.FetchDataReader(resultFields, bucket, CommandBehavior.CloseConnection, 0, true))
            {
                List<string> keys = new List<string>();

                while (reader.Read())
                {
                    keys.Add(reader.GetString(0));
                }

                return await webClient.CreateAsendiaManifest(keys).ConfigureAwait(false);
            }
        }
    }
}
