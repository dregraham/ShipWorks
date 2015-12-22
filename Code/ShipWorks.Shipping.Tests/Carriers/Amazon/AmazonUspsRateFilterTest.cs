using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using Xunit;
using Autofac.Extras.Moq;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonUspsRateFilterTest
    {
        readonly RateResult uspsRate;
        readonly RateResult stampsRate;
        readonly AutoMock mock;

        public AmazonUspsRateFilterTest()
        {
            mock = AutoMock.GetLoose();
            stampsRate = new RateResult("Stamps Rate", "", 0, new AmazonRateTag() { CarrierName = "stamps" });
            uspsRate = new RateResult("u s p s Rate", "", 0, new AmazonRateTag() { CarrierName = "usps" });
        }

        [Fact]
        public void Filter_FooterNotAdded_NoUspsCarrier()
        {
            AmazonUspsRateFilter testObject = mock.Create<AmazonUspsRateFilter>();
            RateGroup rateGroup = new RateGroup(new List<RateResult>() { stampsRate });

            RateGroup filterredRateGroup = testObject.Filter(rateGroup);

            Assert.Equal(0, filterredRateGroup.FootnoteFactories?.Count() ?? 0);
        }

        [Fact]
        public void Filter_FooterAdded_HasUspsCarrier()
        {
            AmazonUspsRateFilter testObject = mock.Create<AmazonUspsRateFilter>();
            RateGroup rateGroup = new RateGroup(new List<RateResult>() { stampsRate, uspsRate });

            RateGroup filterredRateGroup = testObject.Filter(rateGroup);

            Assert.Equal(1, filterredRateGroup.FootnoteFactories?.Count() ?? 0);
        }

        [Fact]
        public void Filter_UspsRateRemoved_HasUspsCarrierRate()
        {
            AmazonUspsRateFilter testObject = mock.Create<AmazonUspsRateFilter>();
            RateGroup rateGroup = new RateGroup(new List<RateResult> { stampsRate, uspsRate });

            RateGroup filterredRateGroup = testObject.Filter(rateGroup);

            Assert.Contains(stampsRate, filterredRateGroup.Rates);
            Assert.DoesNotContain(uspsRate, filterredRateGroup.Rates);
        }
    }
}
