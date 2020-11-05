using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using ShipWorks.Data.Connection;
using ShipWorks.Templates;

namespace ShipWorks.UI.Dialogs.DefaultPrinters
{
    public class DefaultPrintersViewModel
    {
        private readonly ITemplateManager templateManager;
        private readonly ISqlAdapter adapter;

        public DefaultPrintersViewModel(ITemplateManager templateManager, ISqlAdapter adapter)
        {
            SetDefaults = new RelayCommand(async () => SetDefaultsAction().ConfigureAwait(true));
            this.templateManager = templateManager;
            this.adapter = adapter;
        }

        [Obfuscation]
        public ICommand SetDefaults { get; }

        [Obfuscation]
        public bool OverrideExistingPrinters { get; set; }

        [Obfuscation]
        public string ThermalPrinterName { get; set; }

        [Obfuscation]
        public int ThermalPaperSource { get; set; }

        [Obfuscation]
        public string StandardPrinterName { get; set; }

        [Obfuscation]
        public int StandardPaperSource { get; set; }

        private async Task SetDefaultsAction()
        {
            var templates = templateManager.Tree.AllTemplates;
            foreach (var template in templates)
            {
                var computerSettings = templateManager.GetComputerSettings(template);
                if(!OverrideExistingPrinters && !string.IsNullOrWhiteSpace(computerSettings.PrinterName))
                {
                    break;
                }

                if(template.Type == (int) TemplateType.Thermal)
                {
                    computerSettings.PrinterName = ThermalPrinterName;
                    computerSettings.PaperSource = ThermalPaperSource;
                }
                else
                {
                    computerSettings.PrinterName = StandardPrinterName;
                    computerSettings.PaperSource = StandardPaperSource;
                }
                await adapter.SaveEntityAsync(computerSettings);
            }
            adapter.Commit();
        }
    }

}
