using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Autofac.Extras.Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Stores;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class GetActiveStoresResponseTest
    {
        private string xmlOfStores =
            "<?xml version='1.0'?>                                                                                                                    " +
            "<GetActiveStoresActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>" +
            "<ActiveStore>" +
            "<license>E6N7A-EAMRZ-AMXI2-1IHNL-AMAZON-BRIAN@INTERAPPTIVE.COM</license>" +
            "<storeInfo>generic</storeInfo>" +
            "</ActiveStore>" +
            "<ActiveStore>" +
            "<license>A4TSF-PFMRA-RMHAM-G64X2-AMAZON-CREATESTORE@STAMPS.COM</license>" +
            "<storeInfo>generic</storeInfo>" +
            "</ActiveStore>" +
            "<ActiveStore>" +
            "<license>QZUGK-AMRP7-AM2PM-EXX52-AMAZON-CREATESTORE@STAMPS.COM</license>" +
            "<storeInfo>generic</storeInfo>" +
            "</ActiveStore>" +
            "<ActiveStore>" +
            "<license>AHZT4-DQWUR-A9IMA-IMXXX-AMAZON-CREATESTORE@STAMPS.COM</license>" +
            "<storeInfo>generic</storeInfo>" +
            "</ActiveStore>" +
            "<ActiveStore>" +
            "<license>ZU2HK-RLAMA-TNACN-4MX2D-AMAZON-CREATESTORE@STAMPS.COM</license>" +
            "<storeInfo>generic</storeInfo>" +
            "</ActiveStore>" +
            "<ActiveStore>" +
            "<license>EA57O-0AMBR-TRFXC-Y1AMX-AMAZON-CREATESTORE@STAMPS.COM</license>" +
            "<storeInfo>generic</storeInfo>" +
            "</ActiveStore>" +
            "<ActiveStore>" +
            "<license>KB4L7-AM0KX-MRALM-DXNE3-AMAZON-CREATESTORE@STAMPS.COM</license>" +
            "<storeInfo>generic</storeInfo>" +
            "</ActiveStore>" +
            "<ActiveStore>" +
            "<license>4HSLC-RAU61-MAPJM-9XQ5Q-AMAZON-CREATESTORE@STAMPS.COM</license>" +
            "<storeInfo>generic</storeInfo>" +
            "</ActiveStore>" +
            "<ActiveStore>" +
            "<license>Q2D2I-AMRUA-MBMX4-X6NG5-AMAZON-CREATESTORE@STAMPS.COM</license>" +
            "<storeInfo>generic</storeInfo>" +
            "</ActiveStore>" +
            "</GetActiveStoresActivityResponse>";

        [Fact]
        public void ActiveStores_ReturnsEmptyList_WhenNoStoresReturned()
        {
            var xml = new XmlDocument();
            xml.LoadXml("<?xml version='1.0'?><GetActiveStoresActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' />");

            var testObject = new GetActiveStoresResponse(xml);

            Assert.Empty(testObject.ActiveStores);
        }

        [Fact]
        public void ActiveStores_ReturnsIncludedStoreWithCorrectLicenseKey_WhenStoresAreReturned()
        {
            var xml = new XmlDocument();
            xml.LoadXml(xmlOfStores);

            var testObject = new GetActiveStoresResponse(xml);

            Assert.True(testObject.ActiveStores.Any(s=>s.StoreLicenseKey == "Q2D2I-AMRUA-MBMX4-X6NG5-AMAZON-CREATESTORE@STAMPS.COM"));
        }

        [Fact]
        public void ActiveStores_ReturnsCorrectNumberOfStores()
        {
            var xml = new XmlDocument();
            xml.LoadXml(xmlOfStores);

            var testObject = new GetActiveStoresResponse(xml);

            Assert.Equal(9, testObject.ActiveStores.Count);
        }
    }
}
