using System;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip
{
    public class InsureShipCarrierCodeTest : IDisposable
    {
        readonly AutoMock mock;

        public InsureShipCarrierCodeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(FedExServiceType.FedExGround, "FEDEX")]
        [InlineData(FedExServiceType.StandardOvernight, "FEDEX")]
        [InlineData(FedExServiceType.FirstOvernight, "FEDEX")]
        [InlineData(FedExServiceType.PriorityOvernight, "FEDEX")]
        [InlineData(FedExServiceType.FedEx1DayFreight, "FEDEX")]
        [InlineData(FedExServiceType.FedEx2DayFreight, "FEDEX")]
        [InlineData(FedExServiceType.FedEx3DayFreight, "FEDEX")]
        [InlineData(FedExServiceType.SmartPost, "E-FEDEX")]
        [InlineData(FedExServiceType.FedEx2Day, "FEDEX")]
        [InlineData(FedExServiceType.FedEx2DayAM, "FEDEX")]
        [InlineData(FedExServiceType.GroundHomeDelivery, "FEDEX")]
        [InlineData(FedExServiceType.InternationalPriorityFreight, "FEDEX")]
        [InlineData(FedExServiceType.InternationalEconomyFreight, "E-FEDEX")]
        [InlineData(FedExServiceType.FedExEconomyCanada, "FEDEX")]
        [InlineData(FedExServiceType.InternationalFirst, "FEDEX")]
        [InlineData(FedExServiceType.InternationalPriority, "FEDEX")]
        [InlineData(FedExServiceType.InternationalPriorityExpress, "FEDEX")]
        [InlineData(FedExServiceType.InternationalEconomy, "E-FEDEX")]
        [InlineData(FedExServiceType.FedExEuropeFirstInternationalPriority, "FEDEX")]
        [InlineData(FedExServiceType.FedExInternationalGround, "FEDEX")]
        [InlineData(FedExServiceType.OneRate2Day, "FEDEX")]
        [InlineData(FedExServiceType.OneRate2DayAM, "FEDEX")]
        [InlineData(FedExServiceType.OneRateExpressSaver, "FEDEX")]
        [InlineData(FedExServiceType.OneRateFirstOvernight, "FEDEX")]
        [InlineData(FedExServiceType.OneRatePriorityOvernight, "FEDEX")]
        [InlineData(FedExServiceType.OneRateStandardOvernight, "FEDEX")]
        [InlineData(FedExServiceType.FedExFreightEconomy, "FEDEX-F")]
        [InlineData(FedExServiceType.FedExFreightPriority, "FEDEX-F")]
        [InlineData(FedExServiceType.FirstFreight, "FEDEX")]
        [InlineData(FedExServiceType.FedExNextDayAfternoon, "FEDEX")]
        [InlineData(FedExServiceType.FedExNextDayEarlyMorning, "FEDEX")]
        [InlineData(FedExServiceType.FedExNextDayMidMorning, "FEDEX")]
        [InlineData(FedExServiceType.FedExNextDayEndOfDay, "FEDEX")]
        [InlineData(FedExServiceType.FedExDistanceDeferred, "FEDEX")]
        [InlineData(FedExServiceType.FedExNextDayFreight, "FEDEX")]
        [InlineData(FedExServiceType.FedExFimsMailView, "FEDEX")]
        [InlineData(FedExServiceType.FedExFimsMailViewLite, "FEDEX")]
        [InlineData(FedExServiceType.FedExFimsPremium, "FEDEX")]
        [InlineData(FedExServiceType.FedExFimsStandard, "FEDEX")]
        [InlineData(FedExServiceType.FedExExpressSaver, "FEDEX")]
        public void GetCarrierCode_ReturnsCorrectCode_ForDomesticFedExShipment(FedExServiceType service, string expected)
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f.WithPackage().Set(x => (FedExServiceType) x.Service, service))
                .Build();

            var result = InsureShipCarrierCode.GetCarrierCode(shipment, true);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(FedExServiceType.FedExGround)]
        [InlineData(FedExServiceType.StandardOvernight)]
        [InlineData(FedExServiceType.FirstOvernight)]
        [InlineData(FedExServiceType.PriorityOvernight)]
        [InlineData(FedExServiceType.FedEx1DayFreight)]
        [InlineData(FedExServiceType.FedEx2DayFreight)]
        [InlineData(FedExServiceType.FedEx3DayFreight)]
        [InlineData(FedExServiceType.FedEx2Day)]
        [InlineData(FedExServiceType.FedEx2DayAM)]
        [InlineData(FedExServiceType.GroundHomeDelivery)]
        [InlineData(FedExServiceType.InternationalPriorityFreight)]
        [InlineData(FedExServiceType.FedExEconomyCanada)]
        [InlineData(FedExServiceType.InternationalFirst)]
        [InlineData(FedExServiceType.InternationalPriority)]
        [InlineData(FedExServiceType.InternationalPriorityExpress)]
        [InlineData(FedExServiceType.FedExEuropeFirstInternationalPriority)]
        [InlineData(FedExServiceType.FedExInternationalGround)]
        [InlineData(FedExServiceType.OneRate2Day)]
        [InlineData(FedExServiceType.OneRate2DayAM)]
        [InlineData(FedExServiceType.OneRateExpressSaver)]
        [InlineData(FedExServiceType.OneRateFirstOvernight)]
        [InlineData(FedExServiceType.OneRatePriorityOvernight)]
        [InlineData(FedExServiceType.OneRateStandardOvernight)]
        [InlineData(FedExServiceType.FirstFreight)]
        [InlineData(FedExServiceType.FedExNextDayAfternoon)]
        [InlineData(FedExServiceType.FedExNextDayEarlyMorning)]
        [InlineData(FedExServiceType.FedExNextDayMidMorning)]
        [InlineData(FedExServiceType.FedExNextDayEndOfDay)]
        [InlineData(FedExServiceType.FedExDistanceDeferred)]
        [InlineData(FedExServiceType.FedExNextDayFreight)]
        [InlineData(FedExServiceType.FedExFimsMailView)]
        [InlineData(FedExServiceType.FedExFimsMailViewLite)]
        [InlineData(FedExServiceType.FedExFimsPremium)]
        [InlineData(FedExServiceType.FedExFimsStandard)]
        [InlineData(FedExServiceType.FedExExpressSaver)]
        public void GetCarrierCode_ReturnsFedExPennyOne_ForPennyOneFedExShipment(FedExServiceType service)
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f
                    .WithPackage(p => p.Set(x => x.InsurancePennyOne = true))
                    .Set(x => (FedExServiceType) x.Service, service))
                .Build();

            var result = InsureShipCarrierCode.GetCarrierCode(shipment, true);

            Assert.Equal("FEDEX-P1", result);
        }

        [Theory]
        [InlineData(FedExServiceType.FedExGround, "FEDEX-I")]
        [InlineData(FedExServiceType.StandardOvernight, "FEDEX-I")]
        [InlineData(FedExServiceType.FirstOvernight, "FEDEX-I")]
        [InlineData(FedExServiceType.PriorityOvernight, "FEDEX-I")]
        [InlineData(FedExServiceType.FedEx1DayFreight, "FEDEX-I")]
        [InlineData(FedExServiceType.FedEx2DayFreight, "FEDEX-I")]
        [InlineData(FedExServiceType.FedEx3DayFreight, "FEDEX-I")]
        [InlineData(FedExServiceType.SmartPost, "E-FEDEX-I")]
        [InlineData(FedExServiceType.FedEx2Day, "FEDEX-I")]
        [InlineData(FedExServiceType.FedEx2DayAM, "FEDEX-I")]
        [InlineData(FedExServiceType.GroundHomeDelivery, "FEDEX-I")]
        [InlineData(FedExServiceType.InternationalPriorityFreight, "FEDEX-I")]
        [InlineData(FedExServiceType.InternationalEconomyFreight, "E-FEDEX-I")]
        [InlineData(FedExServiceType.FedExEconomyCanada, "FEDEX-I")]
        [InlineData(FedExServiceType.InternationalFirst, "FEDEX-I")]
        [InlineData(FedExServiceType.InternationalPriority, "FEDEX-I")]
        [InlineData(FedExServiceType.InternationalPriorityExpress, "FEDEX-I")]
        [InlineData(FedExServiceType.InternationalEconomy, "E-FEDEX-I")]
        [InlineData(FedExServiceType.FedExEuropeFirstInternationalPriority, "FEDEX-I")]
        [InlineData(FedExServiceType.FedExInternationalGround, "FEDEX-I")]
        [InlineData(FedExServiceType.OneRate2Day, "FEDEX-I")]
        [InlineData(FedExServiceType.OneRate2DayAM, "FEDEX-I")]
        [InlineData(FedExServiceType.OneRateExpressSaver, "FEDEX-I")]
        [InlineData(FedExServiceType.OneRateFirstOvernight, "FEDEX-I")]
        [InlineData(FedExServiceType.OneRatePriorityOvernight, "FEDEX-I")]
        [InlineData(FedExServiceType.OneRateStandardOvernight, "FEDEX-I")]
        [InlineData(FedExServiceType.FedExFreightEconomy, "FEDEX-F")]
        [InlineData(FedExServiceType.FedExFreightPriority, "FEDEX-F")]
        [InlineData(FedExServiceType.FirstFreight, "FEDEX-I")]
        [InlineData(FedExServiceType.FedExNextDayAfternoon, "FEDEX-I")]
        [InlineData(FedExServiceType.FedExNextDayEarlyMorning, "FEDEX-I")]
        [InlineData(FedExServiceType.FedExNextDayMidMorning, "FEDEX-I")]
        [InlineData(FedExServiceType.FedExNextDayEndOfDay, "FEDEX-I")]
        [InlineData(FedExServiceType.FedExDistanceDeferred, "FEDEX-I")]
        [InlineData(FedExServiceType.FedExNextDayFreight, "FEDEX-I")]
        [InlineData(FedExServiceType.FedExFimsMailView, "FEDEX-I")]
        [InlineData(FedExServiceType.FedExFimsMailViewLite, "FEDEX-I")]
        [InlineData(FedExServiceType.FedExFimsPremium, "FEDEX-I")]
        [InlineData(FedExServiceType.FedExFimsStandard, "FEDEX-I")]
        [InlineData(FedExServiceType.FedExExpressSaver, "FEDEX-I")]
        public void GetCarrierCode_ReturnsCorrectCode_ForInternationalFedExShipment(FedExServiceType service, string expected)
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f.WithPackage().Set(x => (FedExServiceType) x.Service, service))
                .Build();

            var result = InsureShipCarrierCode.GetCarrierCode(shipment, false);

            Assert.Equal(expected, result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
