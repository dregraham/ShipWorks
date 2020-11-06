using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Templates;
using ShipWorks.Tests.Shared;
using ShipWorks.UI.Dialogs.DefaultPrinters;
using Xunit;

namespace ShipWorks.UI.Tests.Dialogs.DefaultPrinters
{
    public class DefaultPrintersViewModelTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DefaultPrintersViewModel testObject;
        private readonly Mock<ITemplateManager> templateManager;

        private TemplateEntity configuredThermal;
        private TemplateEntity unconfiguredThermal;
        private TemplateEntity configuredStandard;
        private TemplateEntity unconfiguredStandard;        

        public DefaultPrintersViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            templateManager = mock.Mock<ITemplateManager>();
            templateManager
                .Setup(m => m.GetComputerSettings(It.IsAny<TemplateEntity>()))
                .Returns((TemplateEntity e) => e.ComputerSettings.First());

            configuredThermal = CreateTemplate("ThermalName", TemplateType.Thermal);
            unconfiguredThermal = CreateTemplate(default(string), TemplateType.Thermal);
            configuredStandard = CreateTemplate("StandardName", TemplateType.Standard);
            unconfiguredStandard = CreateTemplate(default, TemplateType.Report);

            var allTemplates = new List<TemplateEntity>
            {
                configuredThermal, unconfiguredThermal, configuredStandard, unconfiguredStandard
            };

            templateManager
                .SetupGet(m => m.AllTemplates)
                .Returns(allTemplates);

            testObject = mock.Create<DefaultPrintersViewModel>();
            testObject.StandardPaperSource = 42;
            testObject.StandardPrinterName = "spn";
            testObject.ThermalPaperSource = 43;
            testObject.ThermalPrinterName = "tpn";
            testObject.OverrideExistingPrinters = true;
        }

        private TemplateEntity CreateTemplate(string printerName, TemplateType templateType)
        {
            TemplateEntity retVal = new TemplateEntity()
            {
                Type = (int) templateType
            };

            retVal.ComputerSettings.Add(new TemplateComputerSettingsEntity
            {
                PrinterName = printerName
            });

            return retVal;
        }

        [Fact]
        public async Task DefaultPrintersViewModel_SetDefaultsAction_SetsStandard()
        {
            await testObject.SetDefaultsAction();

            Assert.Equal("spn", unconfiguredStandard.ComputerSettings.First().PrinterName);
            Assert.Equal(42, unconfiguredStandard.ComputerSettings.First().PaperSource);
        }

        [Fact]
        public async Task DefaultPrintersViewModel_SetDefaultsAction_SetsThermal()
        {
            await testObject.SetDefaultsAction();

            Assert.Equal("tpn", unconfiguredThermal.ComputerSettings.First().PrinterName);
            Assert.Equal(43, unconfiguredThermal.ComputerSettings.First().PaperSource);
        }

        [Fact]
        public async Task DefaultPrintersViewModel_SetDefaultsAction_DoNotOverwrite_WhenOverrideExistingPrintersIsFalse()
        {
            testObject.OverrideExistingPrinters = false;

            await testObject.SetDefaultsAction();

            Assert.Equal("ThermalName", configuredThermal.ComputerSettings.First().PrinterName);
            Assert.Equal("StandardName", configuredStandard.ComputerSettings.First().PrinterName);
            mock.Mock<ISqlAdapter>().Verify(a => a.SaveEntityAsync(It.IsAny<TemplateComputerSettingsEntity>()), Times.Exactly(2));
        }

        [Fact]
        public async Task DefaultPrintersViewModel_SetDefaultAction_Overwrites_WhenOverrideExistingPrintersIsTrue()
        {
            testObject.OverrideExistingPrinters = true;

            await testObject.SetDefaultsAction();

            Assert.Equal("tpn", configuredThermal.ComputerSettings.First().PrinterName);
            Assert.Equal("spn", configuredStandard.ComputerSettings.First().PrinterName);
            mock.Mock<ISqlAdapter>().Verify(a => a.SaveEntityAsync(It.IsAny<TemplateComputerSettingsEntity>()), Times.Exactly(4));
        }

        [Fact]
        public async Task DefaultPrintersViewModel_SetDefaultAction_CallsCommit()
        {
            await testObject.SetDefaultsAction();

            mock.Mock<ISqlAdapter>().Verify(a => a.Commit(), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
