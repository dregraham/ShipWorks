using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// Creates a DHL eCommerce Manifest
    /// </summary>
    [Component]
    public class DhlEcommerceManifestCreator : IDhlEcommerceManifestCreator
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IShipEngineWebClient webClient;
        private readonly ISqlAdapterFactory adapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceManifestCreator(IDateTimeProvider dateTimeProvider,
            IShipEngineWebClient webClient,
            ISqlAdapterFactory adapterFactory)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.webClient = webClient;
            this.adapterFactory = adapterFactory;
        }

        /// <summary>
        /// Create a DHL eCommerce Manifest from today's shipments
        /// </summary>
        public async Task<Result> CreateManifest()
        {
            var currentTime = dateTimeProvider.GetUtcNow();

            // Create the predicate for the query to determine which shipments are eligible
            RelationPredicateBucket bucket = new RelationPredicateBucket
            (
                ShipmentFields.Processed == true &
                ShipmentFields.ProcessedDate >= currentTime.Date &
                ShipmentFields.ProcessedDate < currentTime.AddDays(1).Date &
                ShipmentFields.ReturnShipment == false &
                ShipmentFields.ShipmentType == (int) ShipmentTypeCode.DhlEcommerce
            );

            bucket.Relations.Add(ShipmentEntity.Relations.DhlEcommerceShipmentEntityUsingShipmentID);

            // We just need ShipEngineLabelID
            ResultsetFields resultFields = new ResultsetFields(2);
            resultFields.DefineField(DhlEcommerceShipmentFields.ShipEngineLabelID, 0, "ShipEngineLabelID", "");
            resultFields.DefineField(DhlEcommerceShipmentFields.DhlEcommerceAccountID, 1, "DhlEcommerceAccountID", "");

            Dictionary<long, List<string>> results = new Dictionary<long, List<string>>();

            // Do the fetch
            using (IDataReader reader = adapterFactory.Create().FetchDataReader(resultFields, bucket, CommandBehavior.CloseConnection, 0, false))
            {
                while (reader.Read())
                {
                    string labelID = reader.GetString(0);
                    long accountID = reader.GetInt64(1);
                    List<string> accountLabels;

                    if (!string.IsNullOrWhiteSpace(labelID))
                    {
                        if (!results.TryGetValue(accountID, out accountLabels))
                        {
                            // If this is the first label for this account, new up a list for it
                            accountLabels = new List<string>();
                            results[accountID] = accountLabels;
                        }

                        accountLabels.Add(labelID);
                    }
                }
            }

            if (results.None())
            {
                return Result.FromError("Could not find any shipments for manifest.");
            }

            return await CreateManifest(results).ConfigureAwait(false);
        }

        /// <summary>
        /// Create manifests for each account
        /// </summary>
        private async Task<Result> CreateManifest(Dictionary<long, List<string>> labels)
        {
            Result lastResult = Result.FromSuccess();

            foreach (List<string> keys in labels.Values)
            {
                var result = await webClient.CreateManifest(keys).ConfigureAwait(false);

                if (result.Failure)
                {
                    lastResult = result;
                }
            }

            return lastResult;
        }
    }
}
