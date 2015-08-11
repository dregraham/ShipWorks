using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Tests.Shipping.Insurance
{
    public class InsuranceChoiceTest
    {
        [Fact]
        public void AllFedExShipments_ReturnsTrue_WhenAllShipmentTypeCodesAreFedEx_Test()
        {
            Assert.IsTrue(InsuranceChoice.AllFedExShipments(BuildMatchingChoices(ShipmentTypeCode.FedEx)));
        }

        [Fact]
        public void AllFedExShipments_ReturnsFalse_WhenNotAllShipmentTypeCodesAreFedEx_Test()
        {
            Assert.IsFalse(InsuranceChoice.AllFedExShipments(BuildMixedShipmentTypeChoices()));
        }

        [Fact]
        public void AllUpsExShipments_ReturnsTrue_WhenAllShipmentTypeCodesAreUpsOnLineTools_Test()
        {
            Assert.IsTrue(InsuranceChoice.AllUpsShipments(BuildMatchingChoices(ShipmentTypeCode.UpsOnLineTools)));
        }

        [Fact]
        public void AllUpsExShipments_ReturnsTrue_WhenAllShipmentTypeCodesAreUpsWorldShip_Test()
        {
            Assert.IsTrue(InsuranceChoice.AllUpsShipments(BuildMatchingChoices(ShipmentTypeCode.UpsWorldShip)));
        }

        [Fact]
        public void AllUpsExShipments_ReturnsTrue_WhenAllShipmentTypeCodesAreUpsWorldShipOrUpsOnlineTools_Test()
        {
            List<InsuranceChoice> choices = new List<InsuranceChoice>();
            choices.AddRange(BuildMatchingChoices(ShipmentTypeCode.UpsWorldShip));
            choices.AddRange(BuildMatchingChoices(ShipmentTypeCode.UpsOnLineTools));

            Assert.IsTrue(InsuranceChoice.AllUpsShipments(choices));
        }

        [Fact]
        public void AllUpsShipments_ReturnsFalse_WhenNotAllShipmentTypeCodesAreUps_Test()
        {
            Assert.IsFalse(InsuranceChoice.AllUpsShipments(BuildMixedShipmentTypeChoices()));
        }

        [Fact]
        public void AllOnTracShipments_ReturnsTrue_WhenAllShipmentTypeCodesAreOnTrac_Test()
        {
            Assert.IsTrue(InsuranceChoice.AllOnTracShipments(BuildMatchingChoices(ShipmentTypeCode.OnTrac)));
        }

        [Fact]
        public void AllOnTracShipments_ReturnsFalse_WhenNotAllShipmentTypeCodesAreOnTrac_Test()
        {
            Assert.IsFalse(InsuranceChoice.AllOnTracShipments(BuildMixedShipmentTypeChoices()));
        }

        [Fact]
        public void AlliParcelShipments_ReturnsTrue_WhenAllShipmentTypeCodesAreiParcel_Test()
        {
            Assert.IsTrue(InsuranceChoice.AlliParcelShipments(BuildMatchingChoices(ShipmentTypeCode.iParcel)));
        }

        [Fact]
        public void AlliParcelShipments_ReturnsFalse_WhenNotAllShipmentTypeCodesAreiParcel_Test()
        {
            Assert.IsFalse(InsuranceChoice.AlliParcelShipments(BuildMixedShipmentTypeChoices()));
        }

        [Fact]
        public void AllEndiciaShipments_ReturnsTrue_WhenAllShipmentTypeCodesAreEndicia_Test()
        {
            Assert.IsTrue(InsuranceChoice.AllEndiciaShipments(BuildMatchingChoices(ShipmentTypeCode.Endicia)));
        }

        [Fact]
        public void AllEndiciaShipments_ReturnsFalse_WhenNotAllShipmentTypeCodesAreEndicia_Test()
        {
            Assert.IsFalse(InsuranceChoice.AllEndiciaShipments(BuildMixedShipmentTypeChoices()));
        }

        [Fact]
        public void AllUspsShipments_ReturnsTrue_WhenAllShipmentTypeCodesAreUsps_Test()
        {
            Assert.IsTrue(InsuranceChoice.AllUspsShipments(BuildMatchingChoices(ShipmentTypeCode.Usps)));
        }

        [Fact]
        public void AllUspsShipments_ReturnsFalse_WhenNotAllShipmentTypeCodesAreUsps_Test()
        {
            Assert.IsFalse(InsuranceChoice.AllUspsShipments(BuildMixedShipmentTypeChoices()));
        }

        private List<InsuranceChoice> BuildMatchingChoices(ShipmentTypeCode shipmentType)
        {
            return new List<InsuranceChoice>
            {
                new InsuranceChoice(new ShipmentEntity { ShipmentType = (int) shipmentType }, new ShipmentEntity(), new ShipmentEntity(), null),
                new InsuranceChoice(new ShipmentEntity { ShipmentType = (int) shipmentType }, new ShipmentEntity(), new ShipmentEntity(), null),
                new InsuranceChoice(new ShipmentEntity { ShipmentType = (int) shipmentType }, new ShipmentEntity(), new ShipmentEntity(), null)
            };
        }

        private List<InsuranceChoice> BuildMixedShipmentTypeChoices()
        {
            return new List<InsuranceChoice>
            {
                new InsuranceChoice(new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.FedEx }, new ShipmentEntity(), new ShipmentEntity(), null),
                new InsuranceChoice(new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.BestRate }, new ShipmentEntity(), new ShipmentEntity(), null),
                new InsuranceChoice(new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.Endicia }, new ShipmentEntity(), new ShipmentEntity(), null),
                new InsuranceChoice(new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.Express1Endicia }, new ShipmentEntity(), new ShipmentEntity(), null),
                new InsuranceChoice(new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.Express1Usps}, new ShipmentEntity(), new ShipmentEntity(), null),
                new InsuranceChoice(new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.None }, new ShipmentEntity(), new ShipmentEntity(), null),
                new InsuranceChoice(new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.OnTrac }, new ShipmentEntity(), new ShipmentEntity(), null),
                new InsuranceChoice(new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.Other }, new ShipmentEntity(), new ShipmentEntity(), null),
                new InsuranceChoice(new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.PostalWebTools }, new ShipmentEntity(), new ShipmentEntity(), null),
                new InsuranceChoice(new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.UpsOnLineTools }, new ShipmentEntity(), new ShipmentEntity(), null),
                new InsuranceChoice(new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.UpsWorldShip }, new ShipmentEntity(), new ShipmentEntity(), null),
                new InsuranceChoice(new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.Usps }, new ShipmentEntity(), new ShipmentEntity(), null),
                new InsuranceChoice(new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.iParcel }, new ShipmentEntity(), new ShipmentEntity(), null)
            };
        }


    }
}
