using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.Carriers.Postal.Usps
{
    public class UspsWebClientTest
    {
        [Theory]
        [InlineData(false, false, false, false, false, false, GlobalPostServiceAvailability.None)]
        [InlineData(true, false, false, false, false, false, GlobalPostServiceAvailability.GlobalPost)]
        [InlineData(false, true, false, false, false, false, GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(true, true, false, false, false, false, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(false, false, true, false, false, false, GlobalPostServiceAvailability.None)]
        [InlineData(true, false, true, false, false, false, GlobalPostServiceAvailability.GlobalPost)]
        [InlineData(false, true, true, false, false, false, GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(true, true, true, false, false, false, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(false, false, false, true, false, false, GlobalPostServiceAvailability.None)]
        [InlineData(true, false, false, true, false, false, GlobalPostServiceAvailability.GlobalPost)]
        [InlineData(false, true, false, true, false, false, GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(true, true, false, true, false, false, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(false, false, true, true, false, false, GlobalPostServiceAvailability.InternationalFirst)]
        [InlineData(true, false, true, true, false, false, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.InternationalFirst)]
        [InlineData(false, true, true, true, false, false, GlobalPostServiceAvailability.SmartSaver | GlobalPostServiceAvailability.InternationalFirst)]
        [InlineData(true, true, true, true, false, false, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.SmartSaver | GlobalPostServiceAvailability.InternationalFirst)]
        [InlineData(false, false, false, false, true, false, GlobalPostServiceAvailability.None)]
        [InlineData(true, false, false, false, true, false, GlobalPostServiceAvailability.GlobalPost)]
        [InlineData(false, true, false, false, true, false, GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(true, true, false, false, true, false, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(false, false, true, false, true, false, GlobalPostServiceAvailability.InternationalPriority)]
        [InlineData(true, false, true, false, true, false, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.InternationalPriority)]
        [InlineData(false, true, true, false, true, false, GlobalPostServiceAvailability.SmartSaver | GlobalPostServiceAvailability.InternationalPriority)]
        [InlineData(true, true, true, false, true, false, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.SmartSaver | GlobalPostServiceAvailability.InternationalPriority)]
        [InlineData(false, false, false, true, true, false, GlobalPostServiceAvailability.None)]
        [InlineData(true, false, false, true, true, false, GlobalPostServiceAvailability.GlobalPost)]
        [InlineData(false, true, false, true, true, false, GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(true, true, false, true, true, false, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(false, false, true, true, true, false, GlobalPostServiceAvailability.InternationalFirst | GlobalPostServiceAvailability.InternationalPriority)]
        [InlineData(true, false, true, true, true, false, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.InternationalFirst | GlobalPostServiceAvailability.InternationalPriority)]
        [InlineData(false, true, true, true, true, false, GlobalPostServiceAvailability.SmartSaver | GlobalPostServiceAvailability.InternationalFirst | GlobalPostServiceAvailability.InternationalPriority)]
        [InlineData(true, true, true, true, true, false, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.SmartSaver | GlobalPostServiceAvailability.InternationalFirst | GlobalPostServiceAvailability.InternationalPriority)]
        [InlineData(false, false, false, false, false, true, GlobalPostServiceAvailability.None)]
        [InlineData(true, false, false, false, false, true, GlobalPostServiceAvailability.GlobalPost)]
        [InlineData(false, true, false, false, false, true, GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(true, true, false, false, false, true, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(false, false, true, false, false, true, GlobalPostServiceAvailability.InternationalExpress)]
        [InlineData(true, false, true, false, false, true, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.InternationalExpress)]
        [InlineData(false, true, true, false, false, true, GlobalPostServiceAvailability.SmartSaver | GlobalPostServiceAvailability.InternationalExpress)]
        [InlineData(true, true, true, false, false, true, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.SmartSaver | GlobalPostServiceAvailability.InternationalExpress)]
        [InlineData(false, false, false, true, false, true, GlobalPostServiceAvailability.None)]
        [InlineData(true, false, false, true, false, true, GlobalPostServiceAvailability.GlobalPost)]
        [InlineData(false, true, false, true, false, true, GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(true, true, false, true, false, true, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(false, false, true, true, false, true, GlobalPostServiceAvailability.InternationalFirst | GlobalPostServiceAvailability.InternationalExpress)]
        [InlineData(true, false, true, true, false, true, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.InternationalFirst | GlobalPostServiceAvailability.InternationalExpress)]
        [InlineData(false, true, true, true, false, true, GlobalPostServiceAvailability.SmartSaver | GlobalPostServiceAvailability.InternationalFirst | GlobalPostServiceAvailability.InternationalExpress)]
        [InlineData(true, true, true, true, false, true, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.SmartSaver | GlobalPostServiceAvailability.InternationalFirst | GlobalPostServiceAvailability.InternationalExpress)]
        [InlineData(false, false, false, false, true, true, GlobalPostServiceAvailability.None)]
        [InlineData(true, false, false, false, true, true, GlobalPostServiceAvailability.GlobalPost)]
        [InlineData(false, true, false, false, true, true, GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(true, true, false, false, true, true, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(false, false, true, false, true, true, GlobalPostServiceAvailability.InternationalPriority | GlobalPostServiceAvailability.InternationalExpress)]
        [InlineData(true, false, true, false, true, true, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.InternationalPriority | GlobalPostServiceAvailability.InternationalExpress)]
        [InlineData(false, true, true, false, true, true, GlobalPostServiceAvailability.SmartSaver | GlobalPostServiceAvailability.InternationalPriority | GlobalPostServiceAvailability.InternationalExpress)]
        [InlineData(true, true, true, false, true, true, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.SmartSaver | GlobalPostServiceAvailability.InternationalPriority | GlobalPostServiceAvailability.InternationalExpress)]
        [InlineData(false, false, false, true, true, true, GlobalPostServiceAvailability.None)]
        [InlineData(true, false, false, true, true, true, GlobalPostServiceAvailability.GlobalPost)]
        [InlineData(false, true, false, true, true, true, GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(true, true, false, true, true, true, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.SmartSaver)]
        [InlineData(false, false, true, true, true, true, GlobalPostServiceAvailability.InternationalFirst | GlobalPostServiceAvailability.InternationalPriority | GlobalPostServiceAvailability.InternationalExpress)]
        [InlineData(true, false, true, true, true, true, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.InternationalFirst | GlobalPostServiceAvailability.InternationalPriority | GlobalPostServiceAvailability.InternationalExpress)]
        [InlineData(false, true, true, true, true, true, GlobalPostServiceAvailability.SmartSaver | GlobalPostServiceAvailability.InternationalFirst | GlobalPostServiceAvailability.InternationalPriority | GlobalPostServiceAvailability.InternationalExpress)]
        [InlineData(true, true, true, true, true, true, GlobalPostServiceAvailability.GlobalPost | GlobalPostServiceAvailability.SmartSaver | GlobalPostServiceAvailability.InternationalFirst | GlobalPostServiceAvailability.InternationalPriority | GlobalPostServiceAvailability.InternationalExpress)]
        public void GetGlobalPostServiceAvailability_All(
            bool canPrintGP,
            bool canPrintGPSmartSaver,
            bool canPrintPresort,
            bool canPrintFCI,
            bool canPrintPMI,
            bool canPrintPMEI,
            GlobalPostServiceAvailability expected)
        {
            var result = UspsWebClient.GetGlobalPostServiceAvailability(new CapabilitiesV18
            {
                CanPrintGP = canPrintGP,
                CanPrintGPSmartSaver = canPrintGPSmartSaver,
                CanPrintIntlPresortSinglePiece = canPrintPresort,
                CanPrintFCIPresort = canPrintFCI,
                CanPrintPMIPresort = canPrintPMI,
                CanPrintPMEIPresort = canPrintPMEI
            });

            Assert.Equal(expected, result);
        }
    }
}
