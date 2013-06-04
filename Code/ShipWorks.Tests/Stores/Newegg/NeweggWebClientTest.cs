using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Newegg;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Data.Model.EntityClasses;

using ShipWorks.Tests.Stores.Newegg.Mocked;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping;

namespace ShipWorks.Tests.Stores.Newegg
{
    /// <summary>
    /// Summary description for NeweggWebClientTest
    /// </summary>
    [TestClass]
    public class NeweggWebClientTest
    {
        private IRequestFactory requestFactory;
        private NeweggStoreEntity store;
        private NeweggWebClient testObject;

        private NeweggOrderItemEntity itemEntity;
        private NeweggOrderEntity orderEntity;
        private FedExShipmentEntity fedExEntity;
        private ShipmentEntity shipmentEntity;


        [TestInitialize]
        public void Initialize()
        {
            this.requestFactory = new ShipWorks.Tests.Stores.Newegg.Mocked.Success.MockRequestFactory();
            this.store = new NeweggStoreEntity();

            
            orderEntity = new NeweggOrderEntity { OrderNumber = 123456 };
            itemEntity = new NeweggOrderItemEntity { Order = orderEntity, SellerPartNumber = "9876ZYXW" };
            fedExEntity = new FedExShipmentEntity { Service = (int)FedExServiceType.FedExGround };
            shipmentEntity = new ShipmentEntity { Order = orderEntity, TrackingNumber = "ABCD1234", ShipDate = DateTime.UtcNow, ShipmentType = (int)ShipmentTypeCode.FedEx, FedEx = fedExEntity};
            
            testObject = new NeweggWebClient(store, requestFactory);
        }
        
        [TestMethod]
        public void AreCredentialsValid_ReturnsTrue_WhenAValidResponseIsReceived_Test()
        {
            // The initialization method has configured our test object (NeweggWebClient)
            // to always return a valid response (via our mocked request factory) regardless 
            // of the store entity's configuration
            Assert.IsTrue(testObject.AreCredentialsValid());
        }

        [TestMethod]
        public void AreCredentialsValid_ReturnsFalse_WhenAnInvalidResponseIsReceived_Test()
        {
            // We're going to override the default configuration that was setup in the 
            // initialize method since we're testing for failures
            this.requestFactory = new ShipWorks.Tests.Stores.Newegg.Mocked.Failure.MockRequestFactory();
            testObject = new NeweggWebClient(store, requestFactory);

            Assert.IsFalse(testObject.AreCredentialsValid());
        }

    }
}
