using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;

namespace ShipWorks.UI.Dialogs.DefaultPrinters
{
    /// <summary>
    /// Viewmodel for selecting default printers
    /// </summary>
    public class DefaultPrintersViewModel
    {
        private readonly ITemplateManager templateManager;
        private readonly ISqlAdapter adapter;

        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultPrintersViewModel(ITemplateManager templateManager, ISqlAdapter adapter)
        {
            SetDefaults = new RelayCommand(async () => SetDefaultsAction().ConfigureAwait(true));
            this.templateManager = templateManager;
            this.adapter = adapter;
        }

        /// <summary>
        /// Command to set the defaults
        /// </summary>
        [Obfuscation]
        public ICommand SetDefaults { get; }

        /// <summary>
        /// When true, overrides default printers when SetDefaults is called
        /// </summary>
        [Obfuscation]
        public bool OverrideExistingPrinters { get; set; }

        /// <summary>
        /// Thermal printername
        /// </summary>
        [Obfuscation]
        public string ThermalPrinterName { get; set; }

        /// <summary>
        /// Thermal papersource
        /// </summary>
        [Obfuscation]
        public int ThermalPaperSource { get; set; }

        /// <summary>
        /// Standard printername
        /// </summary>
        [Obfuscation]
        public string StandardPrinterName { get; set; }

        /// <summary>
        /// Standard paper source
        /// </summary>
        [Obfuscation]
        public int StandardPaperSource { get; set; }

        /// <summary>
        /// Loops through all the printers setting their name and source
        /// </summary>
        /// <returns></returns>
        public async Task SetDefaultsAction()
        {
            var templates = templateManager.AllTemplates;
            foreach (var template in templates)
            {
                var computerSettings = templateManager.GetComputerSettings(template);
                if (ShouldSkip(computerSettings))
                {
                    continue;
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

        /// <summary>
        /// Should this printer be overriden
        /// </summary>
        private bool ShouldSkip(TemplateComputerSettingsEntity computerSettings)
        {
            if(OverrideExistingPrinters)
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(computerSettings.PrinterName);
        }
    }

}
