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
        public void AllFedExShipments_ReturnsTrue_WhenAllShipmentTypeCodesAreFedEx()
        {
            Assert.True(InsuranceChoice.AllFedExShipments(BuildMatchingChoices(ShipmentTypeCode.FedEx)));
        }

        [Fact]
        public void AllFedExShipments_ReturnsFalse_WhenNotAllShipmentTypeCodesAreFedEx()
        {
            Assert.False(InsuranceChoice.AllFedExShipments(BuildMixedShipmentTypeChoices()));
        }

        [Fact]
        public void AllUpsExShipments_ReturnsTrue_WhenAllShipmentTypeCodesAreUpsOnLineTools()
        {
            Assert.True(InsuranceChoice.AllUpsShipments(BuildMatchingChoices(ShipmentTypeCode.UpsOnLineTools)));
        }

        [Fact]
        public void AllUpsExShipments_ReturnsTrue_WhenAllShipmentTypeCodesAreUpsWorldShip()
        {
            Assert.True(InsuranceChoice.AllUpsShipments(BuildMatchingChoices(ShipmentTypeCode.UpsWorldShip)));
        }

        [Fact]
        public void AllUpsExShipments_ReturnsTrue_WhenAllShipmentTypeCodesAreUpsWorldShipOrUpsOnlineTools()
        {
            List<InsuranceChoice> choices = new List<InsuranceChoice>();
            choices.AddRange(BuildMatchingChoices(ShipmentTypeCode.UpsWorldShip));
            choices.AddRange(BuildMatchingChoices(ShipmentTypeCode.UpsOnLineTools));

            Assert.True(InsuranceChoice.AllUpsShipments(choices));
        }

        [Fact]
        public void AllUpsShipments_ReturnsFalse_WhenNotAllShipmentTypeCodesAreUps()
        {
            Assert.False(InsuranceChoice.AllUpsShipments(BuildMixedShipmentTypeChoices()));
        }

        [Fact]
        public void AllOnTracShipments_ReturnsTrue_WhenAllShipmentTypeCodesAreOnTrac()
        {
            Assert.True(InsuranceChoice.AllOnTracShipments(BuildMatchingChoices(ShipmentTypeCode.OnTrac)));
        }

        [Fact]
        public void AllOnTracShipments_ReturnsFalse_WhenNotAllShipmentTypeCodesAreOnTrac()
        {
            Assert.False(InsuranceChoice.AllOnTracShipments(BuildMixedShipmentTypeChoices()));
        }

        [Fact]
        public void AlliParcelShipments_ReturnsTrue_WhenAllShipmentTypeCodesAreiParcel()
        {
            Assert.True(InsuranceChoice.AlliParcelShipments(BuildMatchingChoices(ShipmentTypeCode.iParcel)));
        }

        [Fact]
        public void AlliParcelShipments_ReturnsFalse_WhenNotAllShipmentTypeCodesAreiParcel()
        {
            Assert.False(InsuranceChoice.AlliParcelShipments(BuildMixedShipmentTypeChoices()));
        }

        [Fact]
        public void AllEndiciaShipments_ReturnsTrue_WhenAllShipmentTypeCodesAreEndicia()
        {
            Assert.True(InsuranceChoice.AllEndiciaShipments(BuildMatchingChoices(ShipmentTypeCode.Endicia)));
        }

        [Fact]
        public void AllEndiciaShipments_ReturnsFalse_WhenNotAllShipmentTypeCodesAreEndicia()
        {
            Assert.False(InsuranceChoice.AllEndiciaShipments(BuildMixedShipmentTypeChoices()));
        }

        [Fact]
        public void AllUspsShipments_ReturnsTrue_WhenAllShipmentTypeCodesAreUsps()
        {
            Assert.True(InsuranceChoice.AllUspsShipments(BuildMatchingChoices(ShipmentTypeCode.Usps)));
        }

        [Fact]
        public void AllUspsShipments_ReturnsFalse_WhenNotAllShipmentTypeCodesAreUsps()
        {
            Assert.False(InsuranceChoice.AllUspsShipments(BuildMixedShipmentTypeChoices()));
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
