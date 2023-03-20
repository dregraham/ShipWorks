using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Stores.Orders.Combine;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Stores.Platforms.Etsy.OnlineUpdating
{
    /// <summary>
    /// Updates payment and tracking information to Etsy.
    /// </summary>
    [Component]
    [KeyedComponent(typeof(IOnlineUpdater), StoreTypeCode.Etsy)]
    public class EtsyOnlineUpdater : IEtsyOnlineUpdater, IOnlineUpdater
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EtsyOnlineUpdater));
        private readonly Func<EtsyStoreEntity, IEtsyWebClient> createWebClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyOnlineUpdater(Func<EtsyStoreEntity, IEtsyWebClient> createWebClient)
        {
            this.createWebClient = createWebClient;
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        /// <param name="shipmentID">The shipment ID.</param>
        public async Task UploadShipmentDetails(EtsyStoreEntity store, long shipmentID, UnitOfWork2 unitOfWork)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);

            if (shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, shipment was deleted.", shipmentID);
                return;
            }

            EtsyOrderEntity order = shipment.Order as EtsyOrderEntity;

            if (order == null)
            {
                log.Error("shipment not associated with order in EtsyOnlineUpdater.UploadShipmentDetails");

                throw new EtsyException("Shipment not associated with order");
            }

            await UploadShipmentDetails(store, order, shipment, unitOfWork).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        public async Task UploadShipmentDetails(EtsyStoreEntity store, long orderID)
        {
            EtsyOrderEntity order = (EtsyOrderEntity) DataProvider.GetEntity(orderID);

            if (order == null)
            {
                throw new EtsyException("Order not found");
            }

            List<ShipmentEntity> shipments = ShippingManager.GetShipments(orderID, false);
            if (shipments == null)
            {
                log.InfoFormat("No shipments associated with order {0}.", orderID);
                return;
            }

            ShipmentEntity shipment = shipments.FirstOrDefault(x => !string.IsNullOrEmpty(x.TrackingNumber));
            if (shipment == null)
            {
                log.InfoFormat("No shipment with tracking number associated with order {0}.", orderID);
                return;
            }

            UnitOfWork2 unitOfWork = new ManagedConnectionUnitOfWork2();

            await UploadShipmentDetails(store, order, shipment, unitOfWork).ConfigureAwait(false);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="shipment">The shipment.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        private async Task UploadShipmentDetails(EtsyStoreEntity etsyStore, EtsyOrderEntity order, ShipmentEntity shipment, UnitOfWork2 unitOfWork)
        {
            ShippingManager.EnsureShipmentLoaded(shipment);

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                ICombineOrderNumberSearchProvider combineOrderNumber = scope.Resolve<ICombineOrderNumberSearchProvider>();

                IEnumerable<long> orderNumbers = await combineOrderNumber.GetOrderIdentifiers(order).ConfigureAwait(false);

                var exceptions = new List<EtsyException>();
                foreach (long orderNumber in orderNumbers)
                {
                    try
                    {
                        createWebClient(etsyStore).UploadShipmentDetails(etsyStore.EtsyShopID, orderNumber, shipment.TrackingNumber, GetEtsyCarrierCode(shipment));
                    }
                    catch (EtsyException ex)
                    {
                        if (!TryManageException(order, unitOfWork, ex))
                        {
                            exceptions.Add(ex);
                        }
                    }
                }

                exceptions.ThrowExceptions((msg, ex) => new EtsyException(msg, ex));
            }
        }

        /// <summary>
        /// Tries to see if exception was expected from Etsy.
        /// </summary>
        private static bool TryManageException(EtsyOrderEntity order, UnitOfWork2 unitOfWork, EtsyException ex)
        {
            bool exceptionManaged = false;

            if (ex.InnerException is WebException &&
                ((HttpWebResponse) ((WebException) ex.InnerException).Response).StatusCode == HttpStatusCode.NotFound)
            {
                //etsy couldn't find the order, mark status as not found.
                EtsyOrderStatusUtility.MarkOrderAsNotFound(order, unitOfWork);
                exceptionManaged = true;
            }

            return exceptionManaged;
        }

        /// <summary>
        /// Gets the etsy carrier code.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static string GetEtsyCarrierCode(ShipmentEntity shipment)
        {
            ShipmentTypeCode type = (ShipmentTypeCode) shipment.ShipmentType;

            switch (type)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return "ups";

                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                    // The shipment is an Endicia or Usps shipment, check to see if it's DHL
                    if (ShipmentTypeManager.IsDhl((PostalServiceType) shipment.Postal.Service))
                    {
                        // The DHL carrier for Endicia is:
                        return "dhl";
                    }

                    // Use the default carrier for other Endicia types
                    return "usps";

                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                    return "usps";
                case ShipmentTypeCode.FedEx:
                    return "fedex";
                case ShipmentTypeCode.OnTrac:
                    return "ontrac";
                case ShipmentTypeCode.iParcel:
                    return "iparcel";

                case ShipmentTypeCode.DhlEcommerce:
                    return "dhl-global-mail";

                case ShipmentTypeCode.None:
                    return "none";
                default:
                    return "other";
            }
        }

        /// <summary>
        /// Uploads shipment details for a particular shipment
        /// </summary>
        public async Task UploadShipmentDetails(StoreEntity store, List<ShipmentEntity> shipments)
        {
            foreach (var shipment in shipments)
            {
                bool? markAsPaid = null;//TODO: how to get status?
                bool? markAsShipped = null;
                string comment = string.Empty;

                UnitOfWork2 unitOfWork = new ManagedConnectionUnitOfWork2();

                await UploadShipmentDetails((EtsyStoreEntity) store, (EtsyOrderEntity) shipment.Order, shipment, unitOfWork).ConfigureAwait(false);

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    unitOfWork.Commit(adapter);
                    adapter.Commit();
                }
            }
        }
    }
}
