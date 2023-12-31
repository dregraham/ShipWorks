using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.AmeriCommerce.WebServices;

namespace ShipWorks.Stores.Platforms.AmeriCommerce
{
    /// <summary>
    /// Interface for communicating with the AmeriCommerce SOAP api
    /// </summary>
    public interface IAmeriCommerceWebClient
    {
        /// <summary>
        /// Tests communication
        /// </summary>
        void TestConnection();

        /// <summary>
        /// Gets a Store object from its code
        /// </summary>
        StoreTrans GetStore(int storeCode);

        /// <summary>
        /// Gets a list of stores associated with the online account
        /// </summary>
        List<StoreTrans> GetStores();

        /// <summary>
        /// Gets the available status codes for AmeriCommerce
        /// </summary>
        /// <returns></returns>
        List<OrderStatusTrans> GetStatusCodes();

        /// <summary>
        /// Get orders by modified time
        /// </summary>
        List<OrderTrans> GetOrders(DateTime? lastModified);

        /// <summary>
        /// Fills a stub OrderTrans object
        /// </summary>
        OrderTrans FillOrderDetail(OrderTrans order);

        /// <summary>
        /// Gets an AmeriCommerce address by key
        /// </summary>
        OrderAddressTrans GetAddress(int addressId);

        /// <summary>
        /// Returns the state name for a given State ID
        /// </summary>
        string GetStateCode(int stateId);

        /// <summary>
        /// Gets the country code for the americommerce country id
        /// </summary>
        string GetCountryCode(int countryId);

        /// <summary>
        /// Gets the AmeriCommerce customer by key
        /// </summary>
        CustomerTrans GetCustomer(int customerId);

        /// <summary>
        /// Gets the weight in pounds of the value passed in.
        ///
        /// Per Dustin Holmes email, there are only 2 weight unites (pounds and kg), so
        /// for now we'll just do a hard coded conversion but keep this here so it can easily
        /// be handed off to AmeriCommerce.
        /// </summary>
        double GetWeightInPounds(decimal weight, int weightUnitId);

        /// <summary>
        /// Update the online status of the specified order
        /// </summary>
        IResult UpdateOrderStatus(long orderNumber, int statusCode);

        /// <summary>
        /// Update the online status of the specified order
        /// </summary>
        IResult UploadShipmentDetails(long orderNumber, ShipmentEntity shipment);
    }
}