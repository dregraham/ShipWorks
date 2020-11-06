using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using ShipWorks.Templates.Printing;

namespace ShipWorks.UI.Dialogs.DefaultPrinters
{
    /// <summary>
    /// Viewmodel for selecting default printers
    /// </summary>
    [Component(RegistrationType.Self)]
    public class DefaultPrintersViewModel : ViewModelBase
    {
        private readonly ITemplateManager templateManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IPrintUtility printUtility;
        private readonly IMessageHelper messageHelper;
        private bool overrideExistingPrinters;
        private string thermalPrinterName;
        private int thermalPaperSource;
        private string standardPrinterName;
        private int standardPaperSource;
        private ObservableCollection<KeyValuePair<string, string>> printers;
        private bool thermalPaperSourceEnabled = false;
        private bool standardPaperSourceEnable = false;
        private ObservableCollection<KeyValuePair<int, string>> thermalPaperSources;
        private ObservableCollection<KeyValuePair<int, string>> standardPaperSources;

        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultPrintersViewModel(
            ITemplateManager templateManager, 
            ISqlAdapterFactory sqlAdapterFactory,
            IPrintUtility printUtility, 
            IMessageHelper messageHelper)
        {
            printers = new ObservableCollection<KeyValuePair<string, string>>();

            SetDefaults = new RelayCommand<IDialog>(async dialog => SetDefaultsAction(dialog).ConfigureAwait(true));
            this.templateManager = templateManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.printUtility = printUtility;
            this.messageHelper = messageHelper;
            LoadPrinters();
            ThermalPrinterName = default;
            StandardPrinterName = default;
        }

        /// <summary>
        /// Load the printers collection
        /// </summary>
        private void LoadPrinters()
        {
            printers.Add(new KeyValuePair<string, string>(default, "(Select a Printer)"));
            foreach (var printer in printUtility.InstalledPrinters)
            {
                printers.Add(new KeyValuePair<string, string>(printer, printer));
            }
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
        public bool OverrideExistingPrinters
        {
            get => overrideExistingPrinters;
            set => Set(ref overrideExistingPrinters, value);
        }

        /// <summary>
        /// Thermal printer name
        /// </summary>
        [Obfuscation]
        public string ThermalPrinterName
        {
            get => thermalPrinterName;
            set
            {
                Set(ref thermalPrinterName, value);
                ThermalPaperSources = GetPaperSources(value);
                ThermalPaperSource = ThermalPaperSources.FirstOrDefault().Key;
                ThermalPaperSourceEnabled = ThermalPaperSources.Count > 1;
            }
        }

        private ObservableCollection<KeyValuePair<int, string>> GetPaperSources(string printerName)
        {
            if (string.IsNullOrWhiteSpace(printerName))
            {
                return new ObservableCollection<KeyValuePair<int, string>>() { new KeyValuePair<int, string>((int) PaperSourceKind.AutomaticFeed, "") };
            }

            IPrinterSetting printerSettings = printUtility.GetPrinterSettings(printerName);

            if (!printerSettings.IsValid)
            {
                messageHelper.ShowError(string.Format("The printer settings of the selected printer, '{0}', are invalid.  Please check your printer settings in Windows.", printerName));
                return new ObservableCollection<KeyValuePair<int, string>>() { new KeyValuePair<int, string>((int) PaperSourceKind.AutomaticFeed, "") };
            }

            var paperSources = new ObservableCollection<KeyValuePair<int, string>>();
            foreach(var source in printerSettings.PaperSources.Cast<PaperSource>().ToList())
            {
                paperSources.Add(new KeyValuePair<int, string>(source.RawKind, source.SourceName));
            }

            return paperSources;
        }

        [Obfuscation]
        public ObservableCollection<KeyValuePair<int, string>> ThermalPaperSources
        {
            get => thermalPaperSources;
            set => Set(ref thermalPaperSources, value);
        }

        /// <summary>
        /// Thermal papersource
        /// </summary>
        [Obfuscation]
        public int ThermalPaperSource
        {
            get => thermalPaperSource;
            set => Set(ref thermalPaperSource, value);
        }

        /// <summary>
        /// Is ThermalPapersource enabled
        /// </summary>
        [Obfuscation]
        public bool ThermalPaperSourceEnabled
        {
            get => thermalPaperSourceEnabled;
            set => Set(ref thermalPaperSourceEnabled, value);
        }


        /// <summary>
        /// Standard printername
        /// </summary>
        [Obfuscation]
        public string StandardPrinterName
        {
            get => standardPrinterName;
            set
            {
                Set(ref standardPrinterName, value);
                StandardPaperSources = GetPaperSources(value);

                StandardPaperSource = StandardPaperSources.FirstOrDefault().Key;
                StandardPaperSourceEnabled = StandardPaperSources.Count > 1;
            }
        }

        /// <summary>
        /// Standard paper source
        /// </summary>
        [Obfuscation]
        public int StandardPaperSource
        {
            get => standardPaperSource;
            set => Set(ref standardPaperSource, value);
        }

        /// <summary>
        /// Standard Paper Sources for selected printer
        /// </summary>
        [Obfuscation]
        public ObservableCollection<KeyValuePair<int, string>> StandardPaperSources
        {
            get => standardPaperSources;
            set => Set(ref standardPaperSources, value);
        }

        /// <summary>
        /// Is ThermalPapersource enabled
        /// </summary>
        [Obfuscation]
        public bool StandardPaperSourceEnabled
        {
            get => standardPaperSourceEnable;
            set => Set(ref standardPaperSourceEnable, value);
        }

        [Obfuscation]
        public ObservableCollection<KeyValuePair<string, string>> Printers
        {
            get => printers;
            set => Set(ref printers, value);
        }

        /// <summary>
        /// Loops through all the printers setting their name and source
        /// </summary>
        /// <returns></returns>
        public async Task SetDefaultsAction(IDialog dialog)
        {
            var templates = templateManager.AllTemplates;

            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                foreach (var template in templates)
                {
                    var computerSettings = templateManager.GetComputerSettings(template);
                    if (ShouldSkip(computerSettings))
                    {
                        continue;
                    }

                    if (template.Type == (int) TemplateType.Thermal)
                    {
                        computerSettings.PrinterName = ThermalPrinterName;
                        computerSettings.PaperSource = ThermalPaperSource;
                    }
                    else
                    {
                        computerSettings.PrinterName = StandardPrinterName;
                        computerSettings.PaperSource = StandardPaperSource;
                    }

                    await adapter.SaveEntityAsync(computerSettings).ConfigureAwait(true);
                }

                adapter.Commit();
            }

            dialog.DialogResult = true;
            dialog.Close();
        }

        /// <summary>
        /// Should this printer be overriden
        /// </summary>
        private bool ShouldSkip(TemplateComputerSettingsEntity computerSettings)
        {
            if (OverrideExistingPrinters)
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(computerSettings.PrinterName);
        }
    }
}
