using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.OnLineTools.Api
{
    public class UpsApiShipClientTest
    {
        public UpsApiShipClientTest()
        {
        }

        [Fact]
        public void GetTermsOfShipmentApiCode_ReturnsCorrectValue()
        {
            Dictionary<UpsTermsOfSale, string> testList = new Dictionary<UpsTermsOfSale, string>();
            testList.Add(UpsTermsOfSale.NotSpecified, "");
            testList.Add(UpsTermsOfSale.CostFreight, "CFR");
            testList.Add(UpsTermsOfSale.CostInsuranceFreight, "CIF");
            testList.Add(UpsTermsOfSale.CarriageInsurancePaid, "CIP");
            testList.Add(UpsTermsOfSale.CarriagePaidTo, "CPT");
            testList.Add(UpsTermsOfSale.DeliveredAtFrontier, "DAF");
            testList.Add(UpsTermsOfSale.DeliveryDutyPaid, "DDP");
            testList.Add(UpsTermsOfSale.DeliveryDutyUnpaid, "DDU");
            testList.Add(UpsTermsOfSale.DeliveredExQuay, "DEQ");
            testList.Add(UpsTermsOfSale.DeliveredExShip, "DES");
            testList.Add(UpsTermsOfSale.ExWorks, "EXW");
            testList.Add(UpsTermsOfSale.FreeAlongsideShip, "FAS");
            testList.Add(UpsTermsOfSale.FreeCarrier, "FCA");
            testList.Add(UpsTermsOfSale.FreeOnBoard, "FOB");

            foreach (var entry in testList)
            {
                string testCode = UpsApiShipClient.GetTermsOfShipmentApiCode(entry.Key);
                Assert.Equal(entry.Value, testCode);
            }
        }
    }
}
