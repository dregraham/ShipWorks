using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using ShipWorks.Templates.Printing;
using Cursor = System.Windows.Forms.Cursor;
using Cursors = System.Windows.Forms.Cursors;

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
        private KeyValuePair<string, string> selectedThermalPrinter;
        private KeyValuePair<int, string> selectedThermalPaperSource;
        private KeyValuePair<string, string> selectedStandardPrinter;
        private KeyValuePair<int, string> selectedStandardPaperSource;
        private ObservableCollection<KeyValuePair<string, string>> printers;
        private bool thermalPaperSourceEnabled = false;
        private bool standardPaperSourceEnable = false;
        private ObservableCollection<KeyValuePair<int, string>> thermalPaperSources;
        private ObservableCollection<KeyValuePair<int, string>> standardPaperSources;
        private readonly Lazy<IPrinterSetting> defaultPrinter;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultPrintersViewModel(
            ITemplateManager templateManager,
            ISqlAdapterFactory sqlAdapterFactory,
            IPrintUtility printUtility,
            IMessageHelper messageHelper,
            Func<Type, ILog> logFactory)
        {
            printers = new ObservableCollection<KeyValuePair<string, string>>();

            SetDefaults = new RelayCommand<IDialog>(async dialog => SetDefaultsAction(dialog).ConfigureAwait(true));
            SetStandardAsDefault = new RelayCommand(async () => SetAsDefaultAction(false));
            SetThermalAsDefault = new RelayCommand(async () => SetAsDefaultAction(true));
            this.templateManager = templateManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.printUtility = printUtility;
            this.messageHelper = messageHelper;
            LoadPrinters();
            SelectedThermalPrinter = default;
            SelectedStandardPrinter = default;
            defaultPrinter = new Lazy<IPrinterSetting>(printUtility.GetDefaultPrinterSettings);
            log = logFactory(typeof(DefaultPrintersViewModel));
        }

        /// <summary>
        /// Set printer as the default. 
        /// </summary>
        /// <param name="forThermal">Set thermal else standard</param>
        private async Task SetAsDefaultAction(bool forThermal)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (forThermal)
            {
                SelectedThermalPrinter = Printers.SingleOrDefault(p => p.Key == defaultPrinter.Value.PrinterName);
                SelectedThermalPaperSource = ThermalPaperSources.SingleOrDefault(s => s.Key == defaultPrinter.Value.PaperSource.RawKind);
            }
            else
            {
                SelectedStandardPrinter = Printers.SingleOrDefault(p => p.Key == defaultPrinter.Value.PrinterName);
                SelectedStandardPaperSource = StandardPaperSources.SingleOrDefault(s => s.Key == defaultPrinter.Value.PaperSource.RawKind);
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// List of printers that are setup on the machine
        /// </summary>
        [Obfuscation]
        public ObservableCollection<KeyValuePair<string, string>> Printers
        {
            get => printers;
            set => Set(ref printers, value);
        }

        /// <summary>
        /// Standard printername
        /// </summary>
        [Obfuscation]
        public KeyValuePair<string, string> SelectedStandardPrinter
        {
            get => selectedStandardPrinter;
            set
            {
                Set(ref selectedStandardPrinter, value);
                StandardPaperSources = GetPaperSources(value.Key);

                StandardPaperSourceEnabled = StandardPaperSources.Count > 1;
                SelectedStandardPaperSource = StandardPaperSources.FirstOrDefault();
            }
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
        /// Standard paper source
        /// </summary>
        [Obfuscation]
        public KeyValuePair<int, string> SelectedStandardPaperSource
        {
            get => selectedStandardPaperSource;
            set => Set(ref selectedStandardPaperSource, value);
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

        /// <summary>
        /// Thermal printer name
        /// </summary>
        [Obfuscation]
        public KeyValuePair<string, string> SelectedThermalPrinter
        {
            get => selectedThermalPrinter;
            set
            {
                Set(ref selectedThermalPrinter, value);
                ThermalPaperSources = GetPaperSources(value.Key);
                SelectedThermalPaperSource = ThermalPaperSources.FirstOrDefault();
                ThermalPaperSourceEnabled = ThermalPaperSources.Count > 1;
            }
        }

        /// <summary>
        /// Paper sources for the selected thermal printer
        /// </summary>
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
        public KeyValuePair<int, string> SelectedThermalPaperSource
        {
            get => selectedThermalPaperSource;
            set => Set(ref selectedThermalPaperSource, value);
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
        /// Command to set the defaults
        /// </summary>
        [Obfuscation]
        public ICommand SetDefaults { get; }

        /// <summary>
        /// Command to set the thermal as default
        /// </summary>
        [Obfuscation]
        public ICommand SetThermalAsDefault { get; }

        /// <summary>
        /// Command to set the standard pritner to default
        /// </summary>
        [Obfuscation]
        public ICommand SetStandardAsDefault { get; }

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
        /// Loops through all the printers setting their name and source
        /// </summary>
        public async Task SetDefaultsAction(IDialog dialog)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                var templates = templateManager.AllTemplates;

                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    foreach (var template in templates)
                    {
                        var computerSettings = templateManager.GetComputerSettings(template);
                        if (ShouldSkip(template, computerSettings))
                        {
                            continue;
                        }

                        if (template.Type == (int) TemplateType.Thermal)
                        {
                            computerSettings.PrinterName = SelectedThermalPrinter.Key;
                            computerSettings.PaperSource = SelectedThermalPaperSource.Key;
                        }
                        else
                        {
                            computerSettings.PrinterName = SelectedStandardPrinter.Key;
                            computerSettings.PaperSource = SelectedStandardPaperSource.Key;
                        }

                        template.IsDirty = true;

                        await adapter.SaveAndRefetchAsync(template).ConfigureAwait(true);
                    }

                    adapter.Commit();
                }

                dialog.DialogResult = true;
                dialog.Close();
            }
            catch (Exception e)
            {
                log.Error(e);
                messageHelper.ShowError("An error occured setting the default printer");
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Get a collection of paper sources for the given printer
        /// </summary>
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
            foreach (var source in printerSettings.PaperSources.Cast<System.Drawing.Printing.PaperSource>().ToList())
            {
                paperSources.Add(new KeyValuePair<int, string>(source.RawKind, source.SourceName));
            }

            return paperSources;
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
        /// Should this printer be overriden
        /// </summary>
        private bool ShouldSkip(TemplateEntity template, TemplateComputerSettingsEntity computerSettings)
        {
            if (OverrideExistingPrinters)
            {
                return false;
            }

            if (template.Type == (int) TemplateType.Thermal && SelectedThermalPrinter.Key == null)
            {
                return true;
            }

            if (template.Type != (int) TemplateType.Thermal && SelectedStandardPrinter.Key == null)
            {
                return true;
            }

            return !string.IsNullOrWhiteSpace(computerSettings.PrinterName);
        }
    }
}