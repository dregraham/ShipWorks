﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Registration.Promotion
{
    [TestClass]
    public class UspsRegistrationPromotionTest
    {
        private readonly UspsIntuishipRegistrationPromotion testObject;

        public UspsRegistrationPromotionTest()
        {
            testObject = new UspsIntuishipRegistrationPromotion();
        }

        [TestMethod]
        public void GetPromoCode_ReturnsShipWorks3_WhenRegistrationTypeIsExpedited_Test()
        {
            string promo = testObject.GetPromoCode();

            Assert.AreEqual("ShipWorks3", promo);
        }
    }
}