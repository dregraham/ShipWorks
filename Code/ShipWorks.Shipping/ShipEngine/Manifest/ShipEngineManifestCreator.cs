using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.ShipEngine.Manifest
{
    /// <summary>
    /// Creates a ShipEngine Manifest
    /// </summary>
    [Component]
    public class ShipEngineManifestCreator : IShipEngineManifestCreator
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IShipEngineWebClient webClient;
        private readonly ISqlAdapterFactory adapterFactory;
        private const int MaxLabelIdsToSend = 2000;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEngineManifestCreator(IDateTimeProvider dateTimeProvider,
            IShipEngineWebClient webClient,
            ISqlAdapterFactory adapterFactory,
            Func<Type, ILog> logFactory)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.webClient = webClient;
            this.adapterFactory = adapterFactory;
            log = logFactory(typeof(ShipEngineManifestCreator));
        }

        /// <summary>
        /// Create a Manifest from today's shipments, supports Dhl Ecommerce and FedEx
        /// </summary>
        public async Task<List<GenericResult<CreateManifestResponse>>> CreateManifest(ICarrierAccount carrierAccount, IProgressReporter progress)
        {
            progress.Starting();
            progress.PercentComplete = 33;

            var currentTime = dateTimeProvider.GetUtcNow();
            ResultsetFields resultFields = new ResultsetFields(2);

            // Create the predicate for the query to determine which shipments are eligible
            RelationPredicateBucket bucket = new RelationPredicateBucket
            (
                ShipmentFields.Voided == false &
                ShipmentFields.Processed == true &
                ShipmentFields.ProcessedDate >= currentTime.Date &
                ShipmentFields.ProcessedDate < currentTime.AddDays(1).Date &
                ShipmentFields.ReturnShipment == false &
                ShipmentFields.ShipmentType == (int) carrierAccount.ShipmentType
            );

            if (carrierAccount.ShipmentType == ShipmentTypeCode.DhlEcommerce)
            {
                bucket.Relations.Add(ShipmentEntity.Relations.DhlEcommerceShipmentEntityUsingShipmentID);
                bucket.PredicateExpression.AddWithAnd(DhlEcommerceShipmentFields.DhlEcommerceAccountID == carrierAccount.AccountId);
                resultFields.DefineField(DhlEcommerceShipmentFields.ShipEngineLabelID, 0, "ShipEngineLabelID", "");
                resultFields.DefineField(DhlEcommerceShipmentFields.DhlEcommerceAccountID, 1, "ShipEngineAccountID", "");
            }
            else if (carrierAccount.ShipmentType == ShipmentTypeCode.FedEx)
            {
                bucket.Relations.Add(ShipmentEntity.Relations.FedExShipmentEntityUsingShipmentID);
                bucket.PredicateExpression.AddWithAnd(FedExShipmentFields.FedExAccountID == carrierAccount.AccountId);
                resultFields.DefineField(FedExShipmentFields.ShipEngineLabelId, 0, "ShipEngineLabelID", "");
                resultFields.DefineField(FedExShipmentFields.FedExAccountID, 1, "ShipEngineAccountID", "");
            }
            else
            {
                throw new ShipEngineException($"Shipment Type {EnumHelper.GetDescription(carrierAccount.ShipmentType)} does not currently support generating manifests in ShipWorks.");
            }

            Dictionary<long, List<string>> results = new Dictionary<long, List<string>>();

            // Do the fetch
            using (IDataReader reader = adapterFactory.Create().FetchDataReader(resultFields, bucket, CommandBehavior.CloseConnection, 0, false))
            {
                var index = 1;

                while (reader.Read())
                {
                    progress.Detail = $"Loading Shipments: {index}";

                    string labelID = reader.IsDBNull(0) ? null : reader.GetString(0);
                    long accountID = reader.GetInt64(1);

                    if (!string.IsNullOrWhiteSpace(labelID))
                    {
                        if (!results.TryGetValue(accountID, out var accountLabels))
                        {
                            // If this is the first label for this account, new up a list for it
                            accountLabels = new List<string>();
                            results[accountID] = accountLabels;
                        }

                        accountLabels.Add(labelID);
                    }

                    index++;
                }
            }

            if (results.None())
            {
                var errorResult = new List<GenericResult<CreateManifestResponse>>();
                var genRes = GenericResult.FromError<CreateManifestResponse>("Could not find any shipments for manifest.");
                errorResult.Add(genRes);
                return errorResult;
            }

            progress.Detail = "Creating Manifest";
            progress.PercentComplete = 66;

            return await CreateManifest(results).ConfigureAwait(false);
        }

        /// <summary>
        /// Create manifests for each account
        /// </summary>
        private async Task<List<GenericResult<CreateManifestResponse>>> CreateManifest(Dictionary<long, List<string>> labels)
        {
            var results = new List<GenericResult<CreateManifestResponse>>();

            foreach (List<string> labelIds in labels.Values)
            {
                foreach (var chunk in labelIds.SplitIntoChunksOf(MaxLabelIdsToSend))
                {
                    var result = await webClient.CreateManifest(chunk.ToList(), log).ConfigureAwait(false);
                    results.Add(result);
                }
            }

            return results;
        }
    }
}
