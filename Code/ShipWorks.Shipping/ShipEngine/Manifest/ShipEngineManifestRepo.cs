using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.IO.Zip;
using Interapptive.Shared.Pdf;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data;
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
        private readonly IObjectReferenceManager objectReferenceManager;
        private readonly IDataResourceManager resourceManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEngineManifestRepo(ISqlAdapterFactory sqlAdapterFactory,
        IObjectReferenceManager objectReferenceManager,
            IDataResourceManager resourceManager)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.objectReferenceManager = objectReferenceManager;
            this.resourceManager = resourceManager;
        }

        /// <summary>
        /// Save a ShipEngine Manifest to ShipWorks
        /// </summary>
        public async Task<Result> SaveManifest(CreateManifestResponse createManifestResponse, ICarrierAccount account)
        {
            var failedManifests = new List<string>();

            foreach (var manifestResponse in createManifestResponse.Manifests)
            {
                try
                {
                    var manifest = new ShipEngineManifestEntity
                    {
                        CarrierAccountID = account.AccountId,
                        CreatedAt = manifestResponse.CreatedAt,
                        CarrierID = manifestResponse.CarrierId,
                        FormID = manifestResponse.FormId,
                        ManifestID = manifestResponse.ManifestId,
                        ManifestUrl = manifestResponse.ManifestDownload.Href,
                        PlatformWarehouseID = manifestResponse.PlatformWarehouseId ?? string.Empty,
                        ShipDate = manifestResponse.ShipDate,
                        ShipmentCount = manifestResponse.ShipmentCount,
                        ShipmentTypeCode = (int) account.ShipmentType,
                        SubmissionID = manifestResponse.SubmissionId,
                    };

                    using (var sqlAdapter = sqlAdapterFactory.Create())
                    {
                        await sqlAdapter.SaveAndRefetchAsync(manifest);
                        SaveManifestPdf(manifest.ShipEngineManifestID, manifest.ManifestUrl);
                    }
                }
                catch (Exception ex)
                {
                    failedManifests.Add($"{manifestResponse.ManifestId}: {ex.Message}");
                }
            }

            if (failedManifests.Any())
            {
                return Result.FromError($"Errors occurred saving the following manifests:\n{string.Join("\n", failedManifests)}");
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Save the PDF data to the database
        /// </summary>
        /// <returns></returns>
        private void SaveManifestPdf(long shipEngineManifestId, string url)
        {
            WebClient client = new WebClient();
            var data = client.DownloadData(new Uri(url));

            using (MemoryStream pdfBytes = new MemoryStream(data))
            {
                resourceManager.CreateFromPdf(PdfDocumentType.Color, 
                    pdfBytes, 
                    shipEngineManifestId,
                    "DHLeCommerceManifest", 
                    true);
            }
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

            using (var sqlAdapter = sqlAdapterFactory.Create())
            {
                await sqlAdapter.FetchEntityCollectionAsync(queryParams, CancellationToken.None).ConfigureAwait(false);

                return collection.Any() ? collection.ToList() : new List<ShipEngineManifestEntity>();
            }
        }
    }
}
