using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Insurance.InsureShip;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip
{
    [TestClass]
    public class InsureShipInsureShipmentRequestTest
    {
        private Mock<IInsureShipSettings> settings = new Mock<IInsureShipSettings>();
        private Mock<IInsureShipResponseFactory> responseFactory = new Mock<IInsureShipResponseFactory>();
        private Mock<ILog> log;
        private Mock<InsureShipInsureShipmentRequest> request;
        
        private ShipmentEntity shipment = new ShipmentEntity();
        private InsureShipAffiliate affiliate = new InsureShipAffiliate("89898889", "SWasdfasdf");

        [TestInitialize]
        public void Initialize()
        {
            settings.Setup(s => s.UseTestServer).Returns(true);
            settings.Setup(s => s.DistributorID).Returns("D00002");
            settings.Setup(s => s.Username).Returns("test2");
            settings.Setup(s => s.Password).Returns("password");
            settings.Setup(s => s.Url).Returns(new Uri("https://int.insureship.com/api/"));

            shipment = new ShipmentEntity(100031);
            shipment.ShipmentID = 2031;
            shipment.ShipmentType = (int)ShipmentTypeCode.FedEx;

            shipment.OriginEmail = "t.hughes@shipworks.com";
            shipment.OriginFirstName = "Tim";
            shipment.OriginLastName = "Hughes";

            shipment.ShipCity = "Saint Louis";
            shipment.ShipStateProvCode = "MO";
            shipment.ShipPostalCode = "63102";
            shipment.ShipCountryCode = "US";

            shipment.TrackingNumber = "1z88888888888888";

            shipment.OrderID = 2006;
            shipment.Order = new OrderEntity();
            shipment.Order.OrderNumber = 100;
            shipment.Order.OrderItems.Add(new OrderItemEntity() { Name = "Product A" });

            shipment.FedEx = new FedExShipmentEntity();
            shipment.FedEx.Packages.Add(new FedExPackageEntity() { InsuranceValue = 200 });

            log = new Mock<ILog>();
            log.Setup(l => l.Error(It.IsAny<object>()));

            responseFactory = new Mock<IInsureShipResponseFactory>();
        }

        [TestMethod]
        public void JustRunATest()
        {
            //request = new Mock<InsureShipInsureShipmentRequest>(responseFactory.Object, shipment, affiliate, settings.Object, log.Object);
            request = new Mock<InsureShipInsureShipmentRequest>(new InsureShipResponseFactory(), shipment, affiliate, settings.Object, log.Object);
            request.Setup(r => r.GetUniqueShipmentId()).Returns(Guid.NewGuid().ToString());
            request.Setup(r => r.Submit()).CallBase();
            request.Setup(r => r.PopulateShipmentOrder());
            request.Object.Submit();
        }
    }
}
