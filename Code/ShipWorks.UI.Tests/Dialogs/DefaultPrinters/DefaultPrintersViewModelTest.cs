using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using ShipWorks.Templates.Printing;
using ShipWorks.Tests.Shared;
using ShipWorks.UI.Dialogs.DefaultPrinters;
using Xunit;
using static System.Drawing.Printing.PrinterSettings;

namespace ShipWorks.UI.Tests.Dialogs.DefaultPrinters
{
    public class DefaultPrintersViewModelTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IDialog> dialog;
        private readonly DefaultPrintersViewModel testObject;
        private readonly Mock<ITemplateManager> templateManager;
        private readonly Mock<IPrintUtility> printUtility;
        private readonly Mock<ISqlAdapter> sqlAdapter;

        private TemplateEntity configuredThermal;
        private TemplateEntity unconfiguredThermal;
        private TemplateEntity configuredStandard;
        private TemplateEntity unconfiguredStandard;

        public DefaultPrintersViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            sqlAdapter = mock.Mock<ISqlAdapter>();

            Mock<ISqlAdapterFactory> sqlAdapterFactory = mock.Mock<ISqlAdapterFactory>();
            sqlAdapterFactory.Setup(f => f.Create()).Returns(sqlAdapter.Object);

            templateManager = mock.Mock<ITemplateManager>();
            templateManager
                .Setup(m => m.GetComputerSettings(It.IsAny<TemplateEntity>()))
                .Returns((TemplateEntity e) => e.ComputerSettings.First());

            configuredThermal = CreateTemplate("ThermalName", TemplateType.Thermal);
            unconfiguredThermal = CreateTemplate("", TemplateType.Thermal);
            configuredStandard = CreateTemplate("StandardName", TemplateType.Standard);
            unconfiguredStandard = CreateTemplate("", TemplateType.Report);

            var allTemplates = new List<TemplateEntity>
            {
                configuredThermal, unconfiguredThermal, configuredStandard, unconfiguredStandard
            };

            templateManager
                .SetupGet(m => m.AllTemplates)
                .Returns(allTemplates);

            var printerSettings = mock.Mock<IPrinterSetting>();
            printerSettings.SetupGet(p => p.IsValid).Returns(true);
            printerSettings.SetupGet(p => p.PaperSources)
                .Returns(new PaperSourceCollection(new[] {
                    new System.Drawing.Printing.PaperSource()
                    {
                        RawKind = 42, SourceName = "foo"
                    }
                }));

            printUtility = mock.Mock<IPrintUtility>();
            printUtility.SetupGet(p => p.InstalledPrinters)
                .Returns(new List<string> { "printer1", "printer2" });
            printUtility.Setup(p => p.GetPrinterSettings(It.IsAny<string>()))
                .Returns(printerSettings.Object);


            dialog = mock.Mock<IDialog>();

            testObject = mock.Create<DefaultPrintersViewModel>();
            testObject.SelectedStandardPrinter = testObject.Printers.Last();
            testObject.SelectedStandardPaperSource = testObject.StandardPaperSources.Last();
            testObject.SelectedThermalPrinter = testObject.Printers.Last();
            testObject.SelectedThermalPaperSource = testObject.ThermalPaperSources.Last();
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
            await testObject.SetDefaultsAction(dialog.Object);

            Assert.Equal("printer2", unconfiguredStandard.ComputerSettings.First().PrinterName);
            Assert.Equal(42, unconfiguredStandard.ComputerSettings.First().PaperSource);
        }

        [Fact]
        public async Task DefaultPrintersViewModel_SetDefaultsAction_SetsThermal()
        {
            await testObject.SetDefaultsAction(dialog.Object);

            Assert.Equal("printer2", unconfiguredThermal.ComputerSettings.First().PrinterName);
            Assert.Equal(42, unconfiguredThermal.ComputerSettings.First().PaperSource);
        }

        [Fact]
        public async Task DefaultPrintersViewModel_SetDefaultsAction_DoNotOverwrite_WhenOverrideExistingPrintersIsFalse()
        {
            testObject.OverrideExistingPrinters = false;

            await testObject.SetDefaultsAction(dialog.Object);

            Assert.Equal("ThermalName", configuredThermal.ComputerSettings.First().PrinterName);
            Assert.Equal("StandardName", configuredStandard.ComputerSettings.First().PrinterName);
            sqlAdapter.Verify(a => a.SaveAndRefetchAsync(It.IsAny<TemplateEntity>()), Times.Exactly(2));
        }

        [Fact]
        public async Task DefaultPrintersViewModel_SetDefaultAction_Overwrites_WhenOverrideExistingPrintersIsTrue()
        {
            testObject.OverrideExistingPrinters = true;

            await testObject.SetDefaultsAction(dialog.Object);

            Assert.Equal("printer2", configuredThermal.ComputerSettings.First().PrinterName);
            Assert.Equal("printer2", configuredStandard.ComputerSettings.First().PrinterName);
            sqlAdapter.Verify(a => a.SaveAndRefetchAsync(It.IsAny<TemplateEntity>()), Times.Exactly(4));
        }

        [Fact]
        public async Task DefaultPrintersViewModel_SetDefaultAction_CallsCommit()
        {
            await testObject.SetDefaultsAction(dialog.Object);

            mock.Mock<ISqlAdapter>().Verify(a => a.Commit(), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
