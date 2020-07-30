using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Wraps the ShippingManager
    /// </summary>
    public class ShippingManagerWrapper : IShippingManager
    {
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;
        private readonly IValidatedAddressScope validatedAddressScope;
        private readonly IDataProvider dataProvider;
        private readonly IReturnItemRepository returnItemRepository;
        private readonly ILog log;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingManagerWrapper(
            ICarrierShipmentAdapterFactory shipmentAdapterFactory,
            IValidatedAddressScope validatedAddressScope,
            IDataProvider dataProvider,
            IReturnItemRepository returnItemRepository,
            Func<Type, ILog> getLogger,
            ISqlAdapterFactory sqlAdapterFactory)
        {
            this.shipmentAdapterFactory = shipmentAdapterFactory;
            this.validatedAddressScope = validatedAddressScope;
            this.dataProvider = dataProvider;
            this.returnItemRepository = returnItemRepository;
            log = getLogger(GetType());
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Refresh the data for the given shipment, including the carrier specific data.  The order and the other siblings are not touched.
        /// If the shipment has been deleted, an ObjectDeletedException is thrown.
        /// </summary>
        public void RefreshShipment(ShipmentEntity shipment) =>
            ShippingManager.RefreshShipment(shipment);

        /// <summary>
        /// Update the label format of any unprocessed shipment with the given shipment type code
        /// </summary>
        public void UpdateLabelFormatOfUnprocessedShipments(ShipmentTypeCode shipmentTypeCode) =>
            ShippingManager.UpdateLabelFormatOfUnprocessedShipments(shipmentTypeCode);

        /// <summary>
        /// Ensure the specified shipment is fully loaded
        /// </summary>
        public ShipmentEntity EnsureShipmentLoaded(ShipmentEntity shipment)
        {
            ShippingManager.EnsureShipmentLoaded(shipment);
            return shipment;
        }

        /// <summary>
        /// Ensure the specified shipment is fully loaded
        /// </summary>
        /// <remarks>
        /// Normally, an async method should not just wrap a sync method in a Task.Run,
        /// but in this case, we know that the majority of the blocking time will be in the database.
        /// As time goes on, we can replace the original EnsureShipmentLoaded with an
        /// async-first method and then we can remove the Task.Run.
        /// </remarks>
        public Task<ShipmentEntity> EnsureShipmentLoadedAsync(ShipmentEntity shipment) =>
            Task.Run(() => EnsureShipmentLoaded(shipment));

        /// <summary>
        /// Get the shipment of the specified ID.  The Order will be attached.
        /// </summary>
        public ICarrierShipmentAdapter GetShipment(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            return GetShipmentAdapter(shipment);
        }

        /// <summary>
        /// Get the shipment of the specified ID.  The Order will be attached.
        /// </summary>
        public async Task<ICarrierShipmentAdapter> GetShipmentAsync(long shipmentID)
        {
            ShipmentEntity shipment = await dataProvider.GetEntityAsync<ShipmentEntity>(shipmentID).ConfigureAwait(false);
            if (shipment == null)
            {
                log.InfoFormat("Shipment {0} seems to be now deleted.", shipmentID);
                return null;
            }

            await EnsureShipmentLoadedAsync(shipment).ConfigureAwait(false);

            OrderEntity order = await dataProvider.GetEntityAsync<OrderEntity>(shipment.OrderID).ConfigureAwait(false);
            if (order == null)
            {
                log.InfoFormat("Order {0} seems to be now deleted.", shipment.OrderID);
                return null;
            }

            shipment.Order = order;

            return shipmentAdapterFactory.Get(shipment);
        }

        /// <summary>
        /// Gets the shipment adapter, order will be attached.
        /// </summary>
        public ICarrierShipmentAdapter GetShipmentAdapter(ShipmentEntity shipment)
        {
            EnsureShipmentLoaded(shipment);
            return shipmentAdapterFactory.Get(shipment);
        }

        /// <summary>
        /// Removes the specified shipment from the cache
        /// </summary>
        /// <param name="shipment">Shipment that should be removed from cache</param>
        /// <returns></returns>
        public void RemoveShipmentFromRatesCache(ShipmentEntity shipment)
        {
            ShippingManager.RemoveShipmentFromRatesCache(shipment);
        }

        /// <summary>
        /// Change the shipment type of the provided shipment and return it's shipment adapter
        /// </summary>
        public ICarrierShipmentAdapter ChangeShipmentType(ShipmentTypeCode shipmentTypeCode, ShipmentEntity shipment)
        {
            var originalDimensions = GetOriginalDimensions(shipment);
            var originalOriginID = shipment.OriginOriginID;

            shipment.ShipmentTypeCode = shipmentTypeCode;
            EnsureShipmentLoaded(shipment);

            ApplyDimensionsIfNecessary(shipment, originalDimensions);
            ChangeOriginIfNecessary(shipment, originalOriginID);

            return shipmentAdapterFactory.Get(shipment);
        }

        /// <summary>
        /// When switching to a new provider, change the origin if the new provider
        /// doesn't support the currently selected one
        /// </summary>
        private static void ChangeOriginIfNecessary(ShipmentEntity shipment, long originalOriginID)
        {
            var supportedOrigins = ShipmentTypeManager.GetType(shipment).GetOrigins();

            if (!supportedOrigins.Any(x => x.Value == originalOriginID))
            {
                shipment.OriginOriginID = 0;
            }
        }

        /// <summary>
        /// Apply existing dimensions to a shipment, if necessary
        /// </summary>
        private static void ApplyDimensionsIfNecessary(ShipmentEntity shipment, List<DimensionsAdapter> originalDimensions)
        {
            var newDimensions = ShipmentTypeManager.GetType(shipment)
                .GetPackageAdapters(shipment)
                .ToList();

            if (DimensionsAreDefaults(newDimensions, shipment) && newDimensions.Count() == originalDimensions.Count())
            {
                foreach (var (package, dimensions) in newDimensions.Zip(originalDimensions, (x, y) => (package: x, dimensions: y)))
                {
                    package.DimsProfileID = dimensions.ProfileID;
                    package.DimsLength = dimensions.Length;
                    package.DimsWidth = dimensions.Width;
                    package.DimsHeight = dimensions.Height;
                }
            }
        }

        /// <summary>
        /// Are all the dimensions in the list empty or item defaults
        /// </summary>
        public static bool DimensionsAreDefaults(List<IPackageAdapter> dimensions, ShipmentEntity shipment)
        {
            bool areDefaults = dimensions.All(x => x.DimsLength.IsEquivalentTo(0) && x.DimsWidth.IsEquivalentTo(0) && x.DimsHeight.IsEquivalentTo(0));

            if (areDefaults || shipment.Order?.OrderItems?.CompareCountTo(1) != ComparisonResult.Equal)
            {
                return areDefaults;
            }

            var orderItemDims = shipment.Order.OrderItems.Select(o => new
            {
                Length = (double) o.Length,
                Width = (double) o.Width,
                Height = (double) o.Height
            }).FirstOrDefault();

            areDefaults = orderItemDims != null &&
                          dimensions.All(x => x.DimsLength.IsEquivalentTo(orderItemDims.Length) &&
                                              x.DimsWidth.IsEquivalentTo(orderItemDims.Width) &&
                                              x.DimsHeight.IsEquivalentTo(orderItemDims.Height));

            return areDefaults;
        }

        /// <summary>
        /// Get a list of dimensions from a shipment
        /// </summary>
        private static List<DimensionsAdapter> GetOriginalDimensions(ShipmentEntity shipment) =>
            ShipmentTypeManager.GetType(shipment)
                .GetPackageAdapters(shipment)
                .Select(DimensionsAdapter.CreateFrom)
                .ToList();

        /// <summary>
        /// Save the shipment to the database
        /// </summary>
        public IDictionary<ShipmentEntity, Exception> SaveShipmentToDatabase(ShipmentEntity shipment, bool forceSave) =>
            SaveShipmentsToDatabase(new[] { shipment }, forceSave);

        /// <summary>
        /// Save the shipments to the database
        /// </summary>
        public IDictionary<ShipmentEntity, Exception> SaveShipmentsToDatabase(IEnumerable<ShipmentEntity> shipments, bool forceSave)
        {
            if (shipments == null || shipments.Any(s => s == null))
            {
                return new Dictionary<ShipmentEntity, Exception>();
            }

            Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();

            foreach (ShipmentEntity shipment in shipments)
            {
                try
                {
                    // Force the shipment to look dirty so it's forced to save. This is to make sure that if any other
                    // changes had been made by other users we pick up the concurrency violation.
                    if (forceSave && !shipment.IsDirty)
                    {
                        shipment.Fields[(int) ShipmentFieldIndex.BestRateEvents].IsChanged = true;
                        shipment.Fields.IsDirty = true;
                    }

                    ShippingManager.SaveShipment(shipment);

                    using (ISqlAdapter sqlAdapter = sqlAdapterFactory.CreateTransacted())
                    {
                        validatedAddressScope?.FlushAddressesToDatabase(sqlAdapter, shipment.ShipmentID, "Ship");
                        sqlAdapter.Commit();
                    }
                }
                catch (ObjectDeletedException ex)
                {
                    errors[shipment] = ex;
                }
                catch (SqlForeignKeyException ex)
                {
                    errors[shipment] = ex;
                }
                catch (ORMConcurrencyException ex)
                {
                    errors[shipment] = ex;
                }
            }

            return errors;
        }

        /// <summary>
        /// Gets the overridden store shipment.
        /// </summary>
        public ShipmentEntity GetOverriddenStoreShipment(ShipmentEntity shipment) =>
            ShippingManager.GetOverriddenStoreShipment(shipment);

        /// <summary>
        /// Indicates if the given shipment type code is enabled for selection in the shipping window
        /// </summary>
        public bool IsShipmentTypeEnabled(ShipmentTypeCode shipmentTypeCode) =>
            ShippingManager.IsShipmentTypeEnabled(shipmentTypeCode);

        /// <summary>
        /// Void the given shipment.  If the shipment is already voided, then no action is taken and no error is reported.  The fact that
        /// it was voided is logged to tango.
        /// </summary>
        public GenericResult<ICarrierShipmentAdapter> VoidShipment(long shipmentID, IShippingErrorManager errorManager)
        {
            try
            {
                ShipmentEntity shipment = ShippingManager.VoidShipment(shipmentID);
                return GenericResult.FromSuccess(shipmentAdapterFactory.Get(shipment));
            }
            catch (Exception ex) when (ex is ORMConcurrencyException ||
                                        ex is ObjectDeletedException ||
                                        ex is SqlForeignKeyException)
            {
                return GenericResult.FromError<ICarrierShipmentAdapter>(errorManager.SetShipmentErrorMessage(shipmentID, ex, "voided"));
            }
            catch (ShippingException ex)
            {
                return GenericResult.FromError<ICarrierShipmentAdapter>(errorManager.SetShipmentErrorMessage(shipmentID, ex));
            }
        }

        /// <summary>
        /// Indicates if the shipment type of the given type code has gone through the full setup wizard \ configuration
        /// </summary>
        public bool IsShipmentTypeConfigured(ShipmentTypeCode shipmentTypeCode) =>
            ShippingManager.IsShipmentTypeConfigured(shipmentTypeCode);

        /// <summary>
        /// Creates a new shipment for the given order ID
        /// </summary>
        public ShipmentEntity CreateShipment(long orderID) => ShippingManager.CreateShipment(orderID);

        /// <summary>
        /// Create a shipment as a copy of an existing shipment as a return
        /// </summary>
        public ShipmentEntity CreateReturnShipment(ShipmentEntity shipment)
        {
            return CreateShipmentCopy(shipment, x =>
            {
                x.IncludeReturn = false;
                x.ApplyReturnProfile = false;
                x.ReturnShipment = true;
                returnItemRepository.LoadReturnData(x, true);
            });
        }

        /// <summary>
        /// Create a shipment as a copy of an existing shipment
        /// </summary>
        public ShipmentEntity CreateShipmentCopy(ShipmentEntity shipment) => CreateShipmentCopy(shipment, null);

        /// <summary>
        /// Create a shipment as a copy of an existing shipment
        /// </summary>
        public ShipmentEntity CreateShipmentCopy(ShipmentEntity shipment, Action<ShipmentEntity> configuration)
        {
            ShipmentEntity copy = ShippingManager.CreateShipmentCopy(shipment);

            configuration?.Invoke(copy);

            IEnumerable<ValidatedAddressEntity> addressSuggestions = validatedAddressScope
                .LoadValidatedAddresses(shipment.OrderID, "Ship")
                .Select(EntityUtility.CloneAsNew);
            shipment.ValidatedAddress.AddRange(addressSuggestions);

            ShippingManager.SaveShipment(copy);

            return copy;
        }

        /// <summary>
        /// Gets the service used.
        /// </summary>
        public string GetActualServiceUsed(ShipmentEntity shipment) =>
            ShippingManager.GetActualServiceUsed(shipment);

        /// <summary>
        /// Gets the service used.
        /// </summary>
        public string GetOverriddenServiceUsed(ShipmentEntity shipment) =>
            ShippingManager.GetOverriddenServiceUsed(shipment);

        /// <summary>
        /// Gets the service used.
        /// </summary>
        public string GetCarrierName(ShipmentTypeCode shipmentTypeCode) =>
            ShippingManager.GetCarrierName(shipmentTypeCode);

        /// <summary>
        /// Get a description for the 'Other' carrier
        /// </summary>
        public CarrierDescription GetOtherCarrierDescription(ShipmentEntity shipmentTypeCode) =>
            ShippingManager.GetOtherCarrierDescription(shipmentTypeCode);

        /// <summary>
        /// Get the shipment of the specified ID.  The Order will be attached.
        /// </summary>
        public ShipmentEntity EnsureShipmentIsLoadedWithOrder(ShipmentEntity shipment)
        {
            EnsureShipmentLoaded(shipment);

            OrderEntity order = dataProvider.GetEntity(shipment.OrderID) as OrderEntity;
            if (order == null)
            {
                log.InfoFormat("Order {0} seems to now be deleted.", shipment.OrderID);
            }
            else
            {
                // Ensure order items are loaded to the order
                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    adapter.FetchEntityCollection(order.OrderItems, order.GetRelationInfoOrderItems());
                }
            }

            shipment.Order = order;

            return shipment;
        }

        /// <summary>
        /// Validate that the given store is licensed to ship.
        /// </summary>
        /// <remarks>
        /// If there is an error its stored in licenseCheckCache.  If there is already
        /// an error for the store in licenseCheckCache, then its reused and no trip to tango is taken.
        /// </remarks>
        public Exception ValidateLicense(StoreEntity store, IDictionary<long, Exception> licenseCheckCache) =>
            ShippingManager.ValidateLicense(store, licenseCheckCache);

        /// <summary>
        /// Get the carrier account associated with a shipment. Returns null if the account hasn't been set yet.
        /// </summary>
        public ICarrierAccount GetCarrierAccount(ShipmentEntity shipment) =>
            ShippingManager.GetCarrierAccount(shipment, GetShipmentAdapter(shipment).AccountId);

        /// <summary>
        /// Get the carrier account associated with a processed shipment.
        /// </summary>
        public ICarrierAccount GetCarrierAccount(ProcessedShipmentEntity processedShipment)
        {
            var id = GetShipment(processedShipment.ShipmentID).AccountId;
            return ShippingManager.GetCarrierAccount(processedShipment, id);
        }
    }
}
