using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// Repository for getting recent shipments
    /// </summary>
    public class UpsLocalRateRecentShipmentRepository : IUpsLocalRateRecentShipmentRepository
    {
        private readonly IShippingManager shippingManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        public UpsLocalRateRecentShipmentRepository(ISqlAdapterFactory sqlAdapterFactory, IShippingManager shippingManager)
        {
            this.shippingManager = shippingManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Gets the 10 most recent shipments that were processed using the given UPS account
        /// </summary>
        /// <param name="account">The account.</param>
        public IEnumerable<ShipmentEntity> GetRecentShipments(IUpsAccountEntity account)
        {
            RelationPredicateBucket bucket = new RelationPredicateBucket();
            bucket.Relations.Add(UpsShipmentEntity.Relations.ShipmentEntityUsingShipmentID);
            bucket.Relations.Add(ShipmentEntity.Relations.UpsShipmentEntityUsingShipmentID);
            bucket.Relations.Add(UpsShipmentEntity.Relations.UpsPackageEntityUsingShipmentID);
            bucket.Relations.Add(UpsPackageEntity.Relations.UpsShipmentEntityUsingShipmentID);
            bucket.PredicateExpression.Add(UpsShipmentFields.UpsAccountID == account.UpsAccountID);
            bucket.PredicateExpression.AddWithAnd(UpsShipmentFields.PayorType != UpsPayorType.ThirdParty);
            bucket.PredicateExpression.AddWithAnd(new FieldCompareRangePredicate(UpsShipmentFields.Service, null, UpsLocalRateTable.SupportedServiceTypesForLocalRating));
            bucket.PredicateExpression.AddWithAnd(UpsPackageFields.DryIceEnabled == false);
            bucket.PredicateExpression.AddWithAnd(ShipmentFields.Processed == true);

            ISortExpression sortExpression = new SortExpression(ShipmentFields.ProcessedDate | SortOperator.Descending);

            try
            {
                return GetShipments(bucket, sortExpression, 10);
            }
            catch (ShippingException ex)
            {
                throw new UpsLocalRatingException($"Failed to validate local rates:{Environment.NewLine}{Environment.NewLine}{ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the recent shipments.
        /// </summary>
        /// <param name="bucket">The predicate bucket to filter the shipments returned</param>
        /// <param name="sortExpression">The sort expression</param>
        /// <param name="maxNumberOfShipmentsToReturn">The max number of shipments to return</param>
        /// <returns></returns>
        /// <exception cref="UpsLocalRatingException"></exception>
        private IEnumerable<ShipmentEntity> GetShipments(RelationPredicateBucket bucket, ISortExpression sortExpression, int maxNumberOfShipmentsToReturn)
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
                shippingManager.EnsureShipmentLoaded(shipment);
            }

            return shipments;
        }
    }
}