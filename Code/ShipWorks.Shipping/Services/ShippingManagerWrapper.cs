using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Wraps the ShippingManager
    /// </summary>
    public class ShippingManagerWrapper : IShippingManager
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;
        private readonly IValidatedAddressScope validatedAddressScope;
        private readonly IDataProvider dataProvider;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingManagerWrapper(
            ISqlAdapterFactory sqlAdapterFactory,
            ICarrierShipmentAdapterFactory shipmentAdapterFactory,
            IValidatedAddressScope validatedAddressScope,
            IDataProvider dataProvider,
            Func<Type, ILog> getLogger)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.shipmentAdapterFactory = shipmentAdapterFactory;
            this.validatedAddressScope = validatedAddressScope;
            this.dataProvider = dataProvider;
            log = getLogger(GetType());
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
        /// Get the shipment of the specified ID.  The Order will be attached.
        /// </summary>
        public ICarrierShipmentAdapter GetShipment(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
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
            shipment.ShipmentTypeCode = shipmentTypeCode;
            EnsureShipmentLoaded(shipment);
            return shipmentAdapterFactory.Get(shipment);
        }

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
                        shipment.Fields[(int) ShipmentFieldIndex.ShipmentType].IsChanged = true;
                        shipment.Fields.IsDirty = true;
                    }

                    ShippingManager.SaveShipment(shipment);

                    using (SqlAdapter sqlAdapter = new SqlAdapter(true))
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
            ShippingManager.GetOverriddenSerivceUsed(shipment);

        /// <summary>
        /// Gets the service used.
        /// </summary>
        public string GetCarrierName(ShipmentTypeCode shipmentTypeCode) =>
            ShippingManager.GetCarrierName(shipmentTypeCode);

        /// <summary>
        /// Get the shipment of the specified ID.  The Order will be attached.
        /// </summary>
        public ShipmentEntity EnsureShipmentIsLoadedWithOrder(ShipmentEntity shipment)
        {
            EnsureShipmentLoaded(shipment);

            OrderEntity order = dataProvider.GetEntity(shipment.OrderID) as OrderEntity;
            if (order == null)
            {
                log.InfoFormat("Order {0} seems to be now deleted.", shipment.OrderID);
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
        /// Gets the recent shipments.
        /// </summary>
        /// <param name="bucket">The predicate bucket to filter the shipments returned</param>
        /// <param name="sortExpression">The sort expression</param>
        /// <param name="maxNumberOfShipmentsToReturn">The max number of shipments to return</param>
        /// <returns></returns>
        /// <exception cref="UpsLocalRatingException"></exception>
        public IEnumerable<ShipmentEntity> GetShipments(RelationPredicateBucket bucket, ISortExpression sortExpression, int maxNumberOfShipmentsToReturn)
        {
            ShipmentCollection shipmentCollection = new ShipmentCollection();

            try
            {
                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    adapter.FetchEntityCollection(shipmentCollection, bucket, maxNumberOfShipmentsToReturn, sortExpression);
                }
            }
            catch (Exception ex) when (ex is ORMException || ex is SqlException)
            {
                throw new ShippingException($"Error retrieving list of shipments:{Environment.NewLine}{Environment.NewLine}{ex.Message}", ex);
            }

            IList<ShipmentEntity> shipments = shipmentCollection.Items;

            foreach (ShipmentEntity shipment in shipments)
            {
                EnsureShipmentLoaded(shipment);
            }

            return shipments;
        }
    }
}