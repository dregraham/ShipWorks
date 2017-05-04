

using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Surcharges
{
    public class SignatureSurchargeTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Dictionary<UpsSurchargeType, double> surcharges;

        public SignatureSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            surcharges = new Dictionary<UpsSurchargeType, double>()
            {
                {UpsSurchargeType.NoSignature, 1.11},
                {UpsSurchargeType.SignatureRequired, 2.22},
                {UpsSurchargeType.AdultSignatureRequired, 3.33}
            };
        }

        [Theory]
        [InlineData(UpsDeliveryConfirmationType.NoSignature, 1, 1.11, "No Signature")]
        [InlineData(UpsDeliveryConfirmationType.Signature, 1, 2.22, "Signature Required")]
        [InlineData(UpsDeliveryConfirmationType.AdultSignature, 1, 3.33, "Adult Signature Required")]
        [InlineData(UpsDeliveryConfirmationType.AdultSignature, 2, 6.66, "Adult Signature Required")]
        public void Apply_CorrectSurchargeApplied(UpsDeliveryConfirmationType confirmationType, int numberOfPackages, decimal amount, string surchargeName)
        {

            UpsShipmentEntity shipment = new UpsShipmentEntity()
            {
                DeliveryConfirmation = (int) confirmationType
            };
            shipment.Packages.AddRange(Enumerable.Repeat(new UpsPackageEntity(), numberOfPackages));

            var localServiceRate = mock.CreateMock<IUpsLocalServiceRate>();

            var testObject = new SignatureSurcharge(surcharges);
            testObject.Apply(shipment, localServiceRate.Object);

            localServiceRate.Verify(s=>s.AddAmount(amount, surchargeName), Times.Once);
        }
        
        [Theory]
        [InlineData(UpsDeliveryConfirmationType.None)]
        [InlineData(UpsDeliveryConfirmationType.UspsDeliveryConfirmation)]
        public void Apply_NoSurchargeApplied(UpsDeliveryConfirmationType confirmationType)
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity()
            {
                DeliveryConfirmation = (int) confirmationType
            };
            shipment.Packages.Add(new UpsPackageEntity());

            var localServiceRate = mock.CreateMock<IUpsLocalServiceRate>();

            var testObject = new SignatureSurcharge(surcharges);
            testObject.Apply(shipment, localServiceRate.Object);

            localServiceRate.Verify(s => s.AddAmount(It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}