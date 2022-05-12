using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.ShipEngine.Manifest
{
    /// <summary>
    /// Class for getting and saving ShipEngine manifests
    /// </summary>
    [Component]
    public class ShipEngineManifestRepo : IShipEngineManifestRepo
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        public ShipEngineManifestRepo(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Save a ShipEngine Manifest to ShipWorks
        /// </summary>
        public async Task<Result> SaveManifest(CreateManifestResponse createManifestResponse)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get ShipEngineManifestEntities for a carrier account
        /// </summary>
        public async Task<List<ShipEngineManifestEntity>> GetManifests(ICarrierAccount account, int maxToReturn)
        {
            RelationPredicateBucket bucket = new RelationPredicateBucket(ShipEngineManifestFields.CarrierAccountID == account.AccountId);
            var collection = new ShipEngineManifestCollection();

            var queryParams = new QueryParameters
                {
                    CollectionToFetch = collection,
                    FilterToUse = ShipEngineManifestFields.CarrierAccountID == account.AccountId,
                    SorterToUse = new SortExpression(ShipEngineManifestFields.CreatedAt | SortOperator.Descending)
                };

            var sqlAdapter = sqlAdapterFactory.Create();
            await sqlAdapter.FetchEntityCollectionAsync(queryParams, CancellationToken.None).ConfigureAwait(false);

            return collection.Any() ? collection.ToList() : new List<ShipEngineManifestEntity>();
        }
    }
}
